using UnityEngine;
using UnityEngine.UI;

namespace DS
{
    public class WeaponInventorySlot : MonoBehaviour
    {
        private UIManager uiManager;

        private void Awake()
        {
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