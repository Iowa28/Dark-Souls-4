using System.Collections.Generic;
using UnityEngine;

namespace DS
{
    public class PlayerInventory : MonoBehaviour
    {
        private WeaponSlotManager weaponSlotManager;

        [SerializeField]
        private SpellItem spellItem;
        [SerializeField]
        private WeaponItem rightWeapon;
        [SerializeField]
        private WeaponItem leftWeapon;

        [SerializeField] 
        private WeaponItem[] weaponsInRightHandSlots;
        [SerializeField] 
        private WeaponItem[] weaponsInLeftHandSlots;
        [SerializeField]
        private WeaponItem unarmedWeapon;

        private int currentRightWeaponIndex = -1;
        private int currentLeftWeaponIndex = -1;

        public List<WeaponItem> weaponsInventory { get; private set; }

        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();

            weaponsInventory = new List<WeaponItem>();
        }

        private void Start()
        {
            rightWeapon = weaponsInRightHandSlots[0];
            leftWeapon = weaponsInLeftHandSlots[0];
            LoadWeaponsOnSlot();
        }

        public void LoadWeaponsOnSlot()
        {
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
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

        public void UpdateWeapons()
        {
            rightWeapon = currentRightWeaponIndex > -1 ? weaponsInRightHandSlots[currentRightWeaponIndex] : weaponsInRightHandSlots[0];
            
            leftWeapon = currentLeftWeaponIndex > -1 ? weaponsInLeftHandSlots[currentLeftWeaponIndex] : weaponsInLeftHandSlots[0];
        }

        #region Getters

        public SpellItem GetSpellItem() => spellItem;

        public WeaponItem GetRightWeapon() => rightWeapon;

        public WeaponItem GetLeftWeapon() => leftWeapon;

        public WeaponItem[] GetWeaponsInRightHandSlots() => weaponsInRightHandSlots;

        public WeaponItem[] GetWeaponsInLeftHandSlots() => weaponsInLeftHandSlots;

        #endregion
    }
}