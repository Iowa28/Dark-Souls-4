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

        [Header("Idle Animations")]
        [SerializeField]
        private string rightHandIdle;
        [SerializeField]
        private string leftHandIdle;
        [SerializeField]
        private string twoHandIdle;

        [Header("Attack Animations")]
        [SerializeField]
        private string lightAttack1;
        [SerializeField]
        private string lightAttack2;
        [SerializeField]
        private string lightAttack3;
        [SerializeField]
        private string heavyAttack1;

        [Header("Stamina Costs")] 
        [SerializeField]
        private int baseStamina;
        [SerializeField]
        private int lightAttackMultiplier;
        [SerializeField]
        private int heavyAttackMultiplier;

        #region Getters
        
        public GameObject GetModelPrefab()
        {
            return modelPrefab;
        }

        public bool IsUnarmed()
        {
            return isUnarmed;
        }

        public string GetRightHandIdle()
        {
            return rightHandIdle;
        }

        public string GetLeftHandIdle()
        {
            return leftHandIdle;
        }

        public string GetTwoHandIdle()
        {
            return twoHandIdle;
        }

        public string GetLightAttack1()
        {
            return lightAttack1;
        }
        
        public string GetLightAttack2()
        {
            return lightAttack2;
        }
        
        public string GetLightAttack3()
        {
            return lightAttack3;
        }

        public string GetHeavyAttack1()
        {
            return heavyAttack1;
        }

        public int GetLightAttackStamina()
        {
            return baseStamina * lightAttackMultiplier;
        }

        public int GetHeavyAttackStamina()
        {
            return baseStamina * heavyAttackMultiplier;
        }

        #endregion
    }
}