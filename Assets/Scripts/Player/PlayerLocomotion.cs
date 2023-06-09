﻿using UnityEngine;

namespace DS
{
    public class PlayerLocomotion : MonoBehaviour
    {
        private PlayerManager playerManager;
        // private Transform cameraObject;
        private CameraHandler cameraHandler;
        private InputHandler inputHandler;
        public Vector3 moveDirection { get; private set; }

        public new Rigidbody rigidbody { get; private set; }
        private GameObject normalCamera { get; set; }
        private AnimatorHandler animatorHandler;
        
        private Vector3 normalVector { get; set; }
        private Vector3 targetPosition { get; set; }

        [Header("Ground & Air Detection Stats")]
        [SerializeField]
        private float groundDetectionRayStartPoint = .5f;
        [SerializeField]
        private float beginFallMinimumDistance = 1f;
        [SerializeField]
        private float groundDirectionRayDistance = .2f;

        private LayerMask ignoreGroundCheck { get; set; }
        public float inAirTimer { get; set; }

        [Header("Movement Stats")] 
        [SerializeField] 
        private float movementSpeed = 5;
        [SerializeField] 
        private float walkingSpeed = 1;
        [SerializeField]
        private float sprintSpeed = 7;
        [SerializeField]
        private float rotationSpeed = 10;
        [SerializeField]
        private float fallSpeed = 45;

        private void Start()
        {
            playerManager = GetComponent<PlayerManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            animatorHandler.Initialize();
            animatorHandler.CanRotate();

            playerManager.isGrounded = true;
            ignoreGroundCheck = ~(1 << 8 | 1 << 11);
        }

        #region Movement
        
        public void HandleMovement(float delta)
        {
            if (inputHandler.rollFlag || playerManager.isInteracting)
                return;

            moveDirection = cameraHandler.cameraTransform.forward * inputHandler.vertical;
            moveDirection += cameraHandler.cameraTransform.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
            
            float speed = movementSpeed;

            if (inputHandler.sprintFlag && inputHandler.moveAmount > .5)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;
            }
            else
            {
                if (inputHandler.moveAmount < .5)
                {
                    moveDirection *= walkingSpeed;
                }
                else
                {
                    moveDirection *= speed;
                }
                playerManager.isSprinting = false;
            }

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;

            if (inputHandler.lockOnFlag && !inputHandler.sprintFlag)
            {
                animatorHandler.UpdateAnimatorValues(inputHandler.vertical, inputHandler.horizontal, playerManager.isSprinting, delta);
            }
            else
            {
                animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting, delta);
            }

            if (animatorHandler.canRotate)
            {
                HandleRotation(delta);
            }
        }

        private void HandleRotation(float delta)
        {
            Vector3 targetDirection;
            
            if (!inputHandler.lockOnFlag || inputHandler.sprintFlag || inputHandler.rollFlag)
            {
                targetDirection = cameraHandler.cameraTransform.forward * inputHandler.vertical;
                targetDirection += cameraHandler.cameraTransform.right * inputHandler.horizontal;
                targetDirection.Normalize();
                targetDirection.y = 0;

                if (targetDirection == Vector3.zero)
                {
                    targetDirection = transform.forward;
                }
            }
            else
            {
                targetDirection = cameraHandler.currentLockOnTarget.position - transform.position;
                targetDirection.y = 0;
                targetDirection.Normalize();
            }
            
            Quaternion quaternion = Quaternion.LookRotation(targetDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, quaternion, rotationSpeed * delta);

            transform.rotation = targetRotation;
        }

        public void HandleRollingAndSprinting(float delta)
        {
            if (animatorHandler.GetBool("isInteracting"))
                return;

            if (inputHandler.rollFlag)
            {
                moveDirection = cameraHandler.cameraTransform.forward * inputHandler.vertical;
                moveDirection += cameraHandler.cameraTransform.right * inputHandler.horizontal;
                
                if (inputHandler.moveAmount > 0)
                {
                    animatorHandler.PlayTargetAnimation("Rolling", true);
                    moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    transform.rotation = rollRotation;
                }
                else
                {
                    animatorHandler.PlayTargetAnimation("Backstep", true);
                }
            }
        }

        public void HandleFalling(float delta, Vector3 direction)
        {
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = transform.position;
            origin.y += groundDetectionRayStartPoint;

            if (Physics.Raycast(origin, transform.forward, out hit, .4f))
            {
                moveDirection = Vector3.zero;
            }

            if (playerManager.isInAir)
            {
                rigidbody.AddForce(Vector3.down * fallSpeed);
                rigidbody.AddForce(moveDirection * fallSpeed / 10f);
            }
            
            direction.Normalize();
            origin += direction * groundDirectionRayDistance;

            targetPosition = transform.position;
            
            Debug.DrawRay(origin, Vector3.down * beginFallMinimumDistance, Color.red, .1f, false);
            if (Physics.Raycast(origin, Vector3.down, out hit, beginFallMinimumDistance))
            {
                normalVector = hit.normal;
                playerManager.isGrounded = true;
                targetPosition = new Vector3(targetPosition.x, hit.point.y, targetPosition.z);

                if (playerManager.isInAir)
                {
                    if (inAirTimer > .5f)
                    {
                        Debug.Log("You were in air for " + inAirTimer);
                        animatorHandler.PlayTargetAnimation("Land", true);
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Empty", false);
                    }

                    inAirTimer = 0f;
                    playerManager.isInAir = false;
                }
            }
            else
            {
                if (playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                if (!playerManager.isInAir)
                {
                    if (!playerManager.isInteracting)
                    {
                        animatorHandler.PlayTargetAnimation("Falling", true);
                    }

                    rigidbody.velocity = rigidbody.velocity.normalized * (movementSpeed / 2);
                    playerManager.isInAir = true;
                }
            }

            if (playerManager.isInteracting || inputHandler.moveAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, delta / .1f);
            }
            else
            {
                transform.position = targetPosition;
            }
        }

        public void HandleJumping()
        {
            if (playerManager.isInteracting)
                return;

            if (inputHandler.jumpInput && inputHandler.moveAmount > 0)
            {
                Vector3 direction = cameraHandler.cameraTransform.forward * inputHandler.vertical;
                direction += cameraHandler.cameraTransform.right * inputHandler.horizontal;
                direction.y = 0;
                moveDirection = direction;
                animatorHandler.PlayTargetAnimation("Jump", true);
                Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = jumpRotation;
            }
        }

        #endregion
        
    }
}