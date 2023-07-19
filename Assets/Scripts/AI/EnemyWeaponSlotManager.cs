using UnityEngine;

namespace DS 
{
    public class EnemyWeaponSlotManager : MonoBehaviour
    {
        [SerializeField]
        private WeaponItem rightHandWeapon;
        [SerializeField]
        private WeaponItem leftHandWeapon;
    
        private WeaponHolderSlot rightHandSlot;
        private WeaponHolderSlot leftHandSlot;

        private DamageCollider leftHandDamageCollider;
        private DamageCollider rightHandDamageCollider;

        private void Awake()
        {
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

        private void Start()
        {
            LoadWeaponsOnBothHands();
        }

        private void LoadWeaponsOnBothHands()
        {
            if (rightHandWeapon != null)
            {
                LoadWeaponOnSlot(rightHandWeapon, false);
            }

            if (leftHandWeapon != null)
            {
                LoadWeaponOnSlot(leftHandWeapon, true);
            }
        }

        private void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weapon;
                leftHandSlot.LoadWeaponModel(weapon);
            }
            else
            {
                rightHandSlot.currentWeapon = weapon;
                rightHandSlot.LoadWeaponModel(weapon);
            }
            
            LoadWeaponsDamageCollider(isLeft);
        }
        
        #region Handle Weapon's Damage Collider

        private void LoadWeaponsDamageCollider(bool isLeft)
        {
            if (isLeft)
            {
                leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            }
            else
            {
                rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            }
        }
        
        public void OpenDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }

        public void CloseDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }
        
        #endregion
        
        #region Handle Weapon's Stamina Drainage

        public void DrainStaminaLightAttack()
        {
            // drain stamina light attack
        }
        
        public void DrainStaminaHeavyAttack()
        {
            // drain stamina heavy attack
        }
        
        #endregion
    }
}