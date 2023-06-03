using UnityEngine;
using UnityEngine.UI;

namespace DS
{
    public class WeaponInventorySlot : MonoBehaviour
    {
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
    }
}