using UnityEngine;
using UnityEngine.UI;

namespace DS
{
    public class WeaponInventorySlot : MonoBehaviour
    {
        private PlayerInventory playerInventory;
        private WeaponSlotManager weaponSlotManager;
        private UIManager uiManager;

        private void Awake()
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
            weaponSlotManager = FindObjectOfType<WeaponSlotManager>();
            uiManager = FindObjectOfType<UIManager>();
        }

        [SerializeField]
        private Image icon;
        private WeaponItem item;

        public void AddItem(WeaponItem newItem)
        {
            item = newItem;
            icon.sprite = item.GetItemIcon();
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearInventorySlot()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        public void EquipThisItem()
        {
            uiManager.EquipItem(item);
        }
    }
}