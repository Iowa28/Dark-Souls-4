using UnityEngine;

namespace DS 
{
    public class EquipmentWeaponUI : MonoBehaviour
    {
        private bool rightHandSlot01Selected;
        private bool rightHandSLot02Selected;
        private bool leftHandSlot01Selected;
        private bool leftHandSlot02Selected;

        private HandEquipmentSlotUI[] handEquipmentSlots;

        private void Start()
        {
            handEquipmentSlots = GetComponentsInChildren<HandEquipmentSlotUI>();
        }

        public void LoadWeaponsOnEquipmentScreen(PlayerInventory playerInventory)
        {
            foreach (HandEquipmentSlotUI handEquipmentSlot in handEquipmentSlots)
            {
                if (handEquipmentSlot.IsRightHandSlot01())
                {
                    handEquipmentSlot.AddItem(playerInventory.GetWeaponsInRightHandSlots()[0]);
                }
                else if (handEquipmentSlot.IsRightHandSlot02())
                {
                    handEquipmentSlot.AddItem(playerInventory.GetWeaponsInRightHandSlots()[1]);
                }
                else if (handEquipmentSlot.IsLeftHandSlot01())
                {
                    handEquipmentSlot.AddItem(playerInventory.GetWeaponsInLeftHandSlots()[0]);
                }
                else
                {
                    handEquipmentSlot.AddItem(playerInventory.GetWeaponsInLeftHandSlots()[1]);
                }
            }
        }

        public void SelectRightHandSlot01()
        {
            rightHandSlot01Selected = true;
        }
        
        public void SelectRightHandSlot02()
        {
            rightHandSLot02Selected = true;
        }

        public void SelectLeftHandSlot01()
        {
            leftHandSlot01Selected = true;
        }
        
        public void SelectLeftHandSlot02()
        {
            leftHandSlot02Selected = true;
        }
    }
}