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
        public bool selectInput { get; private set; }
        private bool twoHandInput;
        private bool rbInput;
        private bool rtInput;
        public bool jumpInput { get; private set; }
        private bool inventoryInput;
        private bool lockOnInput;
        private bool leftLockOnInput;
        private bool rightLockOnInput;

        public bool dPadUp { get; set; }
        public bool dPadDown { get; set; }
        private bool dPadLeft;
        private bool dPadRight;

        public bool rollFlag { get; private set; }
        public bool twoHandFlag { get; private set; }
        public bool sprintFlag { get; private set; }
        public bool comboFlag { get; set; }
        public bool lockOnFlag { get; private set; }
        private bool inventoryFlag { get; set; }
        private float rollInputTimer;

        private PlayerControls inputActions;
        private PlayerAttacker playerAttacker;
        private PlayerInventory playerInventory;
        private PlayerManager playerManager;
        private WeaponSlotManager weaponSlotManager;
        private CameraHandler cameraHandler;
        private UIManager uiManager;
        private AnimatorHandler animatorHandler;

        private Vector2 movementInput;
        private Vector2 cameraInput;

        private void Awake()
        {
            playerAttacker = GetComponentInChildren<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            uiManager = FindObjectOfType<UIManager>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
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
                inputActions.PlayerActions.Select.performed += _ => selectInput = true;
                inputActions.PlayerActions.TwoHand.performed += _ => twoHandInput = true;
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

        public void ResetInputFlags()
        {
            rollFlag = false;
            selectInput = false;
            twoHandInput = false;
            rbInput = false;
            rtInput = false;
            dPadRight = false;
            dPadLeft = false;
            dPadUp = false;
            dPadDown = false;
            jumpInput = false;
            inventoryInput = false;
            lockOnInput = false;
        }

        public void TickInput(float delta)
        {
            HandleMoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
            HandleQuickSlotsInput();
            HandleInventoryInput();
            HandleLockOnInput();
            HandleTwoHandInput();
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
                playerAttacker.HandleRBAction();
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

        private void HandleTwoHandInput()
        {
            if (twoHandInput)
            {
                twoHandFlag = !twoHandFlag;

                if (twoHandInput)
                {
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.GetRightWeapon(), false);
                }
                else
                {
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.GetLeftWeapon(), true);
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.GetRightWeapon(), false);
                }
            }
        }
    }
}