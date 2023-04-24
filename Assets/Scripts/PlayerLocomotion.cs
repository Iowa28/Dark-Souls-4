using UnityEngine;

namespace DS
{
    public class PlayerLocomotion : MonoBehaviour
    {
        private Transform cameraObject;
        private InputHandler inputHandler;
        private Vector3 moveDirection;

        private new Rigidbody rigidbody;
        private GameObject normalCamera;
        private AnimatorHandler animatorHandler;

        [Header("Stats")] 
        [SerializeField] 
        private float movementSpeed = 5;

        [SerializeField]
        private float rotationSpeed = 10;
        
        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            cameraObject = GameObject.FindWithTag("MainCamera").transform;
            animatorHandler.Initialize();
            animatorHandler.CanRotate();
        }

        private void Update()
        {
            inputHandler.TickInput(Time.deltaTime);

            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;
            moveDirection *= movementSpeed;

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;
            
            animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0);

            if (animatorHandler.canRotate)
            {
                HandleRotation(Time.deltaTime);
            }
        }

        #region Movement

        private Vector3 normalVector;
        private Vector3 targetPosition;

        private void HandleRotation(float delta)
        {
            float moveOverride = inputHandler.moveAmount;

            Vector3 targetDirection = cameraObject.forward * inputHandler.vertical;
            targetDirection += cameraObject.right * inputHandler.horizontal;
            
            targetDirection.Normalize();
            targetDirection.y = 0;

            if (targetDirection == Vector3.zero)
            {
                targetDirection = transform.forward;
            }

            Quaternion quaternion = Quaternion.LookRotation(targetDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, quaternion, rotationSpeed * delta);

            transform.rotation = targetRotation;
        }

        #endregion
        
    }
}