using UnityEngine;

namespace DS
{
    public class PlayerInventory : MonoBehaviour
    {
        private WeaponSlotManager weaponSlotManager;
        
        private WeaponItem rightWeapon;
        private WeaponItem leftWeapon;

        [SerializeField] 
        private WeaponItem[] weaponsInRightHandSlots;
        [SerializeField] 
        private WeaponItem[] weaponsInLeftHandSlots;
        [SerializeField]
        private WeaponItem unarmedWeapon;

        private int currentRightWeaponIndex = -1;
        private int currentLeftWeaponIndex = -1;

        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        private void Start()
        {
            rightWeapon = unarmedWeapon;
            leftWeapon = unarmedWeapon;
        }

        public void ChangeRightWeapon()
        {
            currentRightWeaponIndex++;

            if (currentRightWeaponIndex < weaponsInRightHandSlots.Length)
            {
                if (weaponsInRightHandSlots[currentRightWeaponIndex] != null)
                {
                    rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                    weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
                }
                else
                {
                    currentRightWeaponIndex++;
                }
            }
            
            if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
            {
                currentRightWeaponIndex = -1;
                rightWeapon = unarmedWeapon;
                weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            }
        }
        
        public void ChangeLeftWeapon()
        {
            currentLeftWeaponIndex++;

            if (currentLeftWeaponIndex < weaponsInLeftHandSlots.Length)
            {
                if (weaponsInLeftHandSlots[currentLeftWeaponIndex] != null)
                {
                    leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
                    weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
                }
                else
                {
                    currentLeftWeaponIndex++;
                }
            }

            if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
            {
                currentLeftWeaponIndex = -1;
                leftWeapon = unarmedWeapon;
                weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
            }
        }

        #region Getters

        public WeaponItem GetRightWeapon()
        {
            return rightWeapon;
        }

        public WeaponItem GetLeftWeapon()
        {
            return leftWeapon;
        }

        #endregion
    }
}