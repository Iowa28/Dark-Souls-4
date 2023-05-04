using UnityEngine;

namespace DS
{
    public class PlayerManager : MonoBehaviour
    {
        private CameraHandler cameraHandler;
        private InputHandler inputHandler;
        private PlayerLocomotion playerLocomotion;
        private Animator animator;
        // private AnimatorHandler animatorHandler;
        
        public bool isInteracting { get; set; }
        
        public bool isSprinting { get; set; }

        public bool isInAir { get; set; }
        public bool isGrounded { get; set; }
        
        private void Awake()
        {
            cameraHandler = CameraHandler.singleton;
        }

        private void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            animator = GetComponentInChildren<Animator>();
            // animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        private void Update()
        {
            isInteracting = animator.GetBool("isInteracting");

            float delta = Time.deltaTime;
            inputHandler.TickInput(delta);
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        }
        
        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }
        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;
            isSprinting = inputHandler.bInput;

            if (isInAir)
            {
                playerLocomotion.inAirTimer += Time.deltaTime;
            }
        }
    }
}