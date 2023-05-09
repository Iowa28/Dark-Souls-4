using UnityEngine;

namespace DS
{
    public class WeaponSlotManager : MonoBehaviour
    {
        private WeaponHolderSlot leftHandSlot;
        private WeaponHolderSlot rightHandSlot;

        private DamageCollider leftHandDamageCollider;
        private DamageCollider rightHandDamageCollider;

        private Animator animator;
        private const float fadeDuration = .2f;

        private QuickSlotsUI quickSlotsUI;

        private PlayerStats playerStats;
        public WeaponItem attackingWeapon { get; set; }

        private void Awake()
        {
            animator = GetComponent<Animator>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            playerStats = GetComponentInParent<PlayerStats>();
            
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
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();

                animator.CrossFade(weaponItem != null ? weaponItem.GetLeftHandIdle() : "Left Arm Empty", fadeDuration);
            }
            else
            {
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                
                animator.CrossFade(weaponItem != null ? weaponItem.GetRightHandIdle() : "Right Arm Empty", fadeDuration);
            }
            
            quickSlotsUI.UpdateWeaponQuickSlotsUI(weaponItem, isLeft);
        }

        #region Handle Weapon's Damage Collider

        private void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }
        
        private void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        public void OpenLeftDamageCollider()
        {
            leftHandDamageCollider.EnableDamageCollider();
        }
        
        public void OpenRightDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }
        
        public void CloseLeftDamageCollider()
        {
            leftHandDamageCollider.DisableDamageCollider();
        }
        
        public void CloseRightDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
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
    }
}