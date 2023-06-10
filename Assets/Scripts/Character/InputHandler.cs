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
        public bool eInput { get; set; }
        public bool jumpInput { get; set; }
        public bool inventoryInput { get; set; }
        public bool lockOnInput { get; set; }
        public bool leftLockOnInput { get; set; }
        public bool rightLockOnInput { get; set; }

        public bool dPadUp { get; set; }
        public bool dPadDown { get; set; }
        public bool dPadLeft { get; set; }
        public bool dPadRight { get; set; }

        public bool rollFlag { get; set; }
        public bool sprintFlag { get; private set; }
        public bool comboFlag { get; private set; }
        public bool lockOnFlag { get; private set; }
        private bool inventoryFlag { get; set; }
        private float rollInputTimer;

        private PlayerControls inputActions;
        private PlayerAttacker playerAttacker;
        private PlayerInventory playerInventory;
        private PlayerManager playerManager;
        private CameraHandler cameraHandler;
        private UIManager uiManager;

        private Vector2 movementInput;
        private Vector2 cameraInput;

        private void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            uiManager = FindObjectOfType<UIManager>();
        }

        private void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerMovement.LockOnTargetLeft.performed += _ => leftLockOnInput = true;
                inputActions.PlayerMovement.LockOnTargetRight.performed += _ => rightLockOnInput = true;
                inputActions.PlayerActions.RB.performed += _ => rbInput = true;
                inputActions.PlayerActions.RT.performed += _ => rtInput = true;
                inputActions.PlayerActions.E.performed += _ => eInput = true;
                inputActions.PlayerActions.Jump.performed += _ => jumpInput = true;
                inputActions.PlayerActions.Inventory.performed += _ => inventoryInput = true;
                inputActions.PlayerActions.LockOn.performed += _ => lockOnInput = true;
                inputActions.PlayerQuickSlots.DPadRight.performed += _ => dPadRight = true;
                inputActions.PlayerQuickSlots.DPadLeft.performed += _ => dPadLeft = true;
                inputActions.PlayerQuickSlots.DPadUp.performed += _ => dPadUp = true;
                inputActions.PlayerQuickSlots.DPadDown.performed += _ => dPadDown = true;
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            HandleMoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
            HandleQuickSlotsInput();
            HandleInventoryInput();
            HandleLockOnInput();
        }

        private void HandleMoveInput(float delta)
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
            sprintFlag = bInput;

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
            if (inventoryFlag)
                return;
            
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

        private void HandleInventoryInput()
        {
            if (inventoryInput)
            {
                inventoryFlag = !inventoryFlag;

                if (inventoryFlag)
                {
                    uiManager.CloseHudWindow();
                    uiManager.OpenSelectWindow();
                    uiManager.UpdateUI();
                }
                else
                {
                    uiManager.CloseSelectWindow();
                    uiManager.CloseAllInventoryWindows();
                    uiManager.OpenHudWindow();
                }
            }

            if (inventoryFlag)
            {
                uiManager.CloseHudWindow();
                uiManager.OpenSelectWindow();
                uiManager.UpdateUI();
            }
            else
            {
                uiManager.CloseSelectWindow();
                uiManager.CloseAllInventoryWindows();
                uiManager.OpenHudWindow();
            }
        }

        private void HandleLockOnInput()
        {
            if (lockOnInput && !lockOnFlag)
            {
                cameraHandler.HandleLockOn();
                if (cameraHandler.IsLockOnActive())
                {
                    lockOnFlag = true;
                }
            }
            else if (lockOnInput && lockOnFlag)
            {
                cameraHandler.ClearLockOnTarget();
                lockOnFlag = false;
            }

            if (lockOnFlag && leftLockOnInput)
            {
                leftLockOnInput = false;
                cameraHandler.HandleLockOn();
                cameraHandler.SwitchToLeftLockOn();
            }
            else if (lockOnFlag && rightLockOnInput)
            {
                rightLockOnInput = false;
                cameraHandler.HandleLockOn();
                cameraHandler.SwitchToRightLockOn();
            }
        }
    }
}