using UnityEngine;

namespace DS
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        [SerializeField]
        private Transform parentOverride;

        [SerializeField] 
        private bool isLeftHandSlot;
        [SerializeField] 
        private bool isRightHandSlot;
        [SerializeField]
        private bool isBackSLot;

        public GameObject currentWeaponModel { get; private set; }
        public WeaponItem currentWeapon { get; set; }

        private void UnloadWeapon()
        {
            if (currentWeaponModel != null)
            {
                currentWeaponModel.SetActive(false);
            }
        }

        public void UnloadWeaponAndDestroy()
        {
            if (currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }

        public void LoadWeaponModel(WeaponItem weaponItem)
        {
            UnloadWeaponAndDestroy();

            if (weaponItem == null)
            {
                UnloadWeapon();
                return;
            }

            GameObject model = Instantiate(weaponItem.GetModelPrefab());
            if (model != null)
            {
                model.transform.parent = parentOverride != null ? parentOverride : transform;
                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
            }

            currentWeaponModel = model;
        }

        #region Getters

        public bool IsLeftHandSlot() => isLeftHandSlot;

        public bool IsRightHandSlot() => isRightHandSlot;

        public bool IsBackSLot() => isBackSLot;

        #endregion
    }
}