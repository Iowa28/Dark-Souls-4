using UnityEngine;

namespace DS 
{
    public class EquipmentWeaponUI : MonoBehaviour
    {
        public bool rightHandSlot01Selected { get; private set; }
        public bool rightHandSLot02Selected { get; private set; }
        public bool leftHandSlot01Selected { get; private set; }
        public bool leftHandSlot02Selected { get; private set; }

        private HandEquipmentSlotUI[] handEquipmentSlots;

        public void LoadWeaponsOnEquipmentScreen(PlayerInventory playerInventory)
        {
            handEquipmentSlots ??= GetComponentsInChildren<HandEquipmentSlotUI>();

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

        public void ResetAllSelectedSlots()
        {
            rightHandSlot01Selected = false;
            rightHandSLot02Selected = false;
            leftHandSlot01Selected = false;
            leftHandSlot02Selected = false;
        }
    }
}