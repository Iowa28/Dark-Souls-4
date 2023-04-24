using System;
using UnityEngine;

namespace DS
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal { get; private set; }
        public float vertical { get; private set; }
        public float moveAmount { get; private set; }
        public float mouseX;
        public float mouseY;

        private PlayerControls inputActions;
        private CameraHandler cameraHandler;

        private Vector2 movementInput;
        private Vector2 cameraInput;

        private void Awake()
        {
            cameraHandler = CameraHandler.singleton;
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
            }
        }

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
        }

        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }
    }
}