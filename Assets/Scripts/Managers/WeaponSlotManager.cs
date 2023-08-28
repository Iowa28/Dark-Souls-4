using UnityEngine;

namespace DS
{
    public class WeaponSlotManager : MonoBehaviour
    {
        public WeaponItem attackingWeapon { get; set; }
        
        private WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot { get; private set;  }
        private WeaponHolderSlot backSlot;

        public DamageCollider leftHandDamageCollider { get; private set;  }
        public DamageCollider rightHandDamageCollider { get; private set;  }

        private Animator animator;
        private const float fadeDuration = .2f;

        private QuickSlotsUI quickSlotsUI;

        private PlayerStats playerStats;
        private InputHandler inputHandler;
        private PlayerManager playerManager;
        private PlayerInventory playerInventory;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            playerStats = GetComponentInParent<PlayerStats>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponHolderSlot in weaponHolderSlots)
            {
                if (weaponHolderSlot.IsLeftHandSlot())
                {
                    leftHandSlot = weaponHolderSlot;
                }
                else if (weaponHolderSlot.IsRightHandSlot())
                {
                    rightHandSlot = weaponHolderSlot;
                }
                else if (weaponHolderSlot.IsBackSlot())
                {
                    backSlot = weaponHolderSlot;
                }
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                animator.CrossFade(weaponItem != null ? weaponItem.GetLeftHandIdle() : "Left Arm Empty", fadeDuration);
                LoadLeftWeaponDamageCollider();
            }
            else
            {
                if (inputHandler.twoHandFlag)
                {
                    backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                    leftHandSlot.UnloadWeaponAndDestroy();
                    animator.CrossFade(weaponItem.GetTwoHandIdle(), fadeDuration);
                }
                else
                {
                    backSlot.UnloadWeaponAndDestroy();
                    animator.CrossFade("Both Arms Empty", fadeDuration);
                    animator.CrossFade(weaponItem != null ? weaponItem.GetRightHandIdle() : "Right Arm Empty", fadeDuration);
            
                    if (leftHandSlot.currentWeaponModel == null)
                    {
                        leftHandSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                    }
                }
            
                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
            }
            
            quickSlotsUI.UpdateWeaponQuickSlotsUI(weaponItem, isLeft);
        }

        #region Handle Weapon's Damage Collider

        private void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

            if (leftHandDamageCollider)
            {
                leftHandDamageCollider.currentWeaponDamage = playerInventory.GetLeftWeapon().GetBaseDamage();
            }
        }
        
        private void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.currentWeaponDamage = playerInventory.GetRightWeapon().GetBaseDamage();
        }

        public void OpenDamageCollider()
        {
            if (playerManager.isUsingLeftHand)
            {
                leftHandDamageCollider.EnableDamageCollider();
            }
            else
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
        }

        public void CloseDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
            if (leftHandDamageCollider)
            {
                leftHandDamageCollider.DisableDamageCollider();   
            }
        }

        #endregion

        #region Handle Weapon's Stamina Drainage

        public void DrainStaminaLightAttack()
        {
            playerStats.DecreaseStamina(attackingWeapon.GetLightAttackStamina());
        }
        
        public void DrainStaminaHeavyAttack()
        {
            playerStats.DecreaseStamina(attackingWeapon.GetHeavyAttackStamina());
        }
        
        #endregion

        // #region Getters
        //
        // public DamageCollider GetLeftHandDamageCollider() => leftHandDamageCollider;
        //
        // public DamageCollider GetRightHandDamageCollider() => rightHandDamageCollider;
        //
        // #endregion
    }
}