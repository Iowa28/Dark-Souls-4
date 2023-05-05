using System;
using UnityEngine;

namespace DS
{
    public class PlayerInventory : MonoBehaviour
    {
        private WeaponSlotManager weaponSlotManager;
        
        [SerializeField]
        private WeaponItem rightWeapon;
        [SerializeField]
        private WeaponItem leftWeapon;

        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        private void Start()
        {
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
        }

        public WeaponItem GetRightWeapon()
        {
            return rightWeapon;
        }

        public WeaponItem GetLeftWeapon()
        {
            return leftWeapon;
        }
    }
}