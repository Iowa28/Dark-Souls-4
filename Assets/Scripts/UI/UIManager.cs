using UnityEngine;

namespace DS
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private EquipmentWeaponUI equipmentWeaponUI;
        private PlayerInventory playerInventory;

        [Header("UI Windows")]
        [SerializeField]
        private GameObject hudWindow;
        [SerializeField]
        private GameObject selectWindow;
        [SerializeField]
        private GameObject equipmentScreenWindow;
        [SerializeField]
        private GameObject weaponInventoryWindow;

        [Header("Weapon Inventory")]
        [SerializeField]
        private GameObject weaponInventorySlotPrefab;
        [SerializeField]
        private Transform weaponInventorySlotsParent;

        private WeaponInventorySlot[] weaponInventorySlots;

        private void Awake()
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
        }

        private void Start()
        {
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
            equipmentWeaponUI.LoadWeaponsOnEquipmentScreen(playerInventory);
        }

        public void UpdateUI()
        {
            #region Weapon Inventory Slots

            for (int i = 0; i < weaponInventorySlots.Length; i++)
            {
                if (i < playerInventory.weaponsInventory.Count)
                {
                    if (weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
                    {
                        Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }
                    weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
                }
                else
                {
                    weaponInventorySlots[i].ClearInventorySlot();
                }
            }

            #endregion
        }

        public void OpenHudWindow()
        {
            hudWindow.SetActive(true);
        }

        public void CloseHudWindow()
        {
            hudWindow.SetActive(false);
        }

        public void OpenSelectWindow()
        {
            selectWindow.SetActive(true);
        }

        public void CloseSelectWindow()
        {
            selectWindow.SetActive(false);
        }

        public void CloseAllInventoryWindows()
        {
            equipmentWeaponUI.ResetAllSelectedSlots();
            weaponInventoryWindow.SetActive(false);
            equipmentScreenWindow.SetActive(false);
        }

        public void EquipItem(WeaponItem item)
        {
            if (equipmentWeaponUI.rightHandSlot01Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.GetWeaponsInRightHandSlots()[0]);
                playerInventory.GetWeaponsInRightHandSlots()[0] = item;
            }
            else if (equipmentWeaponUI.rightHandSLot02Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.GetWeaponsInRightHandSlots()[1]);
                playerInventory.GetWeaponsInRightHandSlots()[1] = item;
            }
            else if (equipmentWeaponUI.leftHandSlot01Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.GetWeaponsInLeftHandSlots()[0]);
                playerInventory.GetWeaponsInLeftHandSlots()[0] = item;
            }
            else if (equipmentWeaponUI.leftHandSlot02Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.GetWeaponsInLeftHandSlots()[1]);
                playerInventory.GetWeaponsInLeftHandSlots()[1] = item; 
            }
            else
            {
                return;
            }

            playerInventory.weaponsInventory.Remove(item);
            
            playerInventory.UpdateWeapons();
            playerInventory.LoadWeaponsOnSlot();
            
            equipmentWeaponUI.LoadWeaponsOnEquipmentScreen(playerInventory);
            equipmentWeaponUI.ResetAllSelectedSlots();
        }

        // #region Getters
        //
        // public bool IsRightHandSlot01Selected()
        // {
        //     return equipmentWeaponUI.rightHandSlot01Selected;
        // }
        //
        // public bool IsRightHandSlot02Selected()
        // {
        //     return equipmentWeaponUI.rightHandSLot02Selected;
        // }
        //
        // public bool IsLeftHandSlot01Selected()
        // {
        //     return equipmentWeaponUI.leftHandSlot01Selected;
        // }
        //
        // public bool IsLeftHandSlot02Selected()
        // {
        //     return equipmentWeaponUI.leftHandSlot02Selected;
        // }
        //
        // #endregion
    }
}