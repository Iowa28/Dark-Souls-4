using UnityEngine;

namespace DS
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI Windows")]
        [SerializeField]
        private GameObject hudWindow;
        [SerializeField]
        private GameObject selectWindow;
        [SerializeField]
        private GameObject weaponInventoryWindow;

        [Header("Weapon Inventory")]
        [SerializeField]
        private GameObject weaponInventorySlotPrefab;
        [SerializeField]
        private Transform weaponInventorySlotsParent;

        private WeaponInventorySlot[] weaponInventorySlots;

        private PlayerInventory playerInventory;
        private EquipmentWeaponUI equipmentWeaponUI;

        private void Awake()
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
            equipmentWeaponUI = GetComponentInChildren<EquipmentWeaponUI>();
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
            weaponInventoryWindow.SetActive(false);
        }
    }
}