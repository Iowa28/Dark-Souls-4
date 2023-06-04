using UnityEngine;
using UnityEngine.UI;

namespace DS
{
    public class QuickSlotsUI : MonoBehaviour
    {
        [SerializeField]
        private Image leftWeaponIcon;
        [SerializeField]
        private Image rightWeaponIcon;

        public void UpdateWeaponQuickSlotsUI(WeaponItem weapon, bool isLeft)
        {
            if (isLeft)
            {
                if (weapon.GetItemIcon() != null)
                {
                    leftWeaponIcon.sprite = weapon.GetItemIcon();
                    leftWeaponIcon.enabled = true;
                }
                else
                {
                    leftWeaponIcon.sprite = null;
                    leftWeaponIcon.enabled = false;
                }
            }
            else
            {
                if (weapon.GetItemIcon() != null)
                {
                    rightWeaponIcon.sprite = weapon.GetItemIcon();
                    rightWeaponIcon.enabled = true;
                }
                else
                {
                    rightWeaponIcon.sprite = null;
                    rightWeaponIcon.enabled = false;
                }
            }
        }
    }
}