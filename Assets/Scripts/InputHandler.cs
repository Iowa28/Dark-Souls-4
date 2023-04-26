using UnityEngine;

namespace DS
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal { get; private set; }
        public float vertical { get; private set; }
        public float moveAmount { get; private set; }
        public float mouseX { get; private set; }
        public float mouseY { get; private set; }
        
        public bool bInput { get; private set; }
        
        public bool rollFlag { get; set; }
        public bool sprintFlag { get; set; }
        private float rollInputTimer;

        private PlayerControls inputActions;

        private Vector2 movementInput;
        private Vector2 cameraInput;

        private void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += input => movementInput = input.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += input => cameraInput = input.ReadValue<Vector2>();
            }
            
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleRollInput(delta);
        }

        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        private void HandleRollInput(float delta)
        {
            bInput = inputActions.PlayerActions.Roll.IsPressed();

            if (bInput)
            {
                rollInputTimer += delta;
                sprintFlag = true;
            }
            else
            {
                if (rollInputTimer > 0 && rollInputTimer < .5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }
            
                rollInputTimer = 0;
            }
        }
    }
}