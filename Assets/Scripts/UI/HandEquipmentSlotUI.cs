using UnityEngine;
using UnityEngine.UI;

namespace DS 
{
    public class HandEquipmentSlotUI : MonoBehaviour
    {
        [SerializeField]
        private Image icon;

        private WeaponItem weapon;

        [SerializeField]
        private bool rightHandSlot01;
        [SerializeField]
        private bool rightHandSlot02;
        [SerializeField]
        private bool leftHandSlot01;
        [SerializeField]
        private bool leftHandSlot02;

        public void AddItem(WeaponItem newWeapon)
        {
            weapon = newWeapon;
            icon.sprite = weapon.GetItemIcon();
            icon.enabled = true;
            gameObject.SetActive(true);
        }
        
        public void ClearInventorySlot()
        {
            weapon = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        public void SelectThisSlot()
        {
            
        }

        #region Getters

        public bool IsRightHandSlot01() => rightHandSlot01;

        public bool IsRightHandSlot02() => rightHandSlot02;

        public bool IsLeftHandSlot01() => leftHandSlot01;

        public bool IsLeftHandSlot02() => leftHandSlot02;

        #endregion
    }
}