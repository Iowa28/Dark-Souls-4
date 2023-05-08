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

        private bool bInput;
        public bool rbInput { get; set; }
        public bool rtInput { get; set; }
        public bool dPadUp { get; set; }
        public bool dPadDown { get; set; }
        public bool dPadLeft { get; set; }
        public bool dPadRight { get; set; }
        
        public bool rollFlag { get; set; }
        public bool sprintFlag { get; set; }
        public bool comboFlag { get; set; }
        private float rollInputTimer;

        private PlayerControls inputActions;
        private PlayerAttacker playerAttacker;
        private PlayerInventory playerInventory;
        private PlayerManager playerManager;

        private Vector2 movementInput;
        private Vector2 cameraInput;

        private void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
        }

        private void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += input => movementInput = input.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += input => cameraInput = input.ReadValue<Vector2>();
                inputActions.PlayerActions.RB.performed += i => rbInput = true;
                inputActions.PlayerActions.RT.performed += i => rtInput = true;
                inputActions.PlayerQuickSlots.DPadRight.performed += i => dPadRight = true;
                inputActions.PlayerQuickSlots.DPadLeft.performed += i => dPadLeft = true;
                inputActions.PlayerQuickSlots.DPadUp.performed += i => dPadUp = true;
                inputActions.PlayerQuickSlots.DPadDown.performed += i => dPadDown = true;
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
            HandleAttackInput(delta);
            HandleQuickSlotsInput();
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

        private void HandleAttackInput(float delta)
        {
            if (rbInput)
            {
                if (playerManager.canDoCombo)
                {
                    comboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.GetRightWeapon());
                    comboFlag = false;
                }
                else if (!playerManager.isInteracting)
                {
                    playerAttacker.HandleLightAttack(playerInventory.GetRightWeapon());   
                }
            }
            
            if (rtInput)
            {
                playerAttacker.HandleHeavyAttack(playerInventory.GetRightWeapon());
            }
        }

        private void HandleQuickSlotsInput()
        {
            if (dPadRight)
            {
                playerInventory.ChangeRightWeapon();
            }
            else if (dPadLeft)
            {
                playerInventory.ChangeLeftWeapon();
            }
        }
    }
}