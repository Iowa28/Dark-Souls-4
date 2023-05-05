using UnityEngine;

namespace DS
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        [SerializeField] 
        private GameObject modelPrefab;
        [SerializeField]
        private bool isUnarmed;

        public GameObject GetModelPrefab()
        {
            return modelPrefab;
        }
    }
}