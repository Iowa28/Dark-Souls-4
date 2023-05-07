using UnityEngine;

namespace DS
{
    public class PlayerManager : MonoBehaviour
    {
        private CameraHandler cameraHandler;
        private InputHandler inputHandler;
        private PlayerLocomotion playerLocomotion;
        private AnimatorHandler animatorHandler;

        public bool isInteracting { get; private set; }
        public bool isSprinting { get; set; }
        public bool isInAir { get; set; }
        public bool isGrounded { get; set; }
        public bool canDoCombo { get; private set; }
        
        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
        }

        private void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        private void Update()
        {
            isInteracting = animatorHandler.GetBool("isInteracting");
            canDoCombo = animatorHandler.GetBool("canDoCombo");

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
            inputHandler.rbInput = false;
            inputHandler.rtInput = false;

            if (isInAir)
            {
                playerLocomotion.inAirTimer += Time.deltaTime;
            }
        }
    }
}