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

        [Header("One Handed Attack Animations")]
        [SerializeField]
        private string lightAttack1;
        [SerializeField]
        private string lightAttack2;
        [SerializeField]
        private string heavyAttack1;
        [SerializeField]
        private string heavyAttack2;

        public GameObject GetModelPrefab()
        {
            return modelPrefab;
        }

        public string GetLightAttack1()
        {
            return lightAttack1;
        }
        
        public string GetLightAttack2()
        {
            return lightAttack2;
        }

        public string GetHeavyAttack1()
        {
            return heavyAttack1;
        }
        
        public string GetHeavyAttack2()
        {
            return heavyAttack2;
        }
    }
}