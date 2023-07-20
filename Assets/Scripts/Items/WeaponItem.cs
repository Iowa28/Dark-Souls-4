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
        [SerializeField]
        private string twoHandLightAttack1;
        [SerializeField]
        private string twoHandLightAttack2;
        [SerializeField]
        private string twoHandHeavyAttack1;
        
        [Header("Stamina Costs")] 
        [SerializeField]
        private int baseStamina;
        [SerializeField]
        private int lightAttackMultiplier;
        [SerializeField]
        private int heavyAttackMultiplier;

        [SerializeField]
        private WeaponType weaponType;

        #region Getters
        
        public GameObject GetModelPrefab() => modelPrefab;

        public bool IsUnarmed() => isUnarmed;

        public string GetRightHandIdle() => rightHandIdle;

        public string GetLeftHandIdle() => leftHandIdle;

        public string GetTwoHandIdle() => twoHandIdle;

        public string GetLightAttack1() => lightAttack1;

        public string GetLightAttack2() => lightAttack2;

        public string GetLightAttack3() => lightAttack3;

        public string GetTwoHandLightAttack1() => twoHandLightAttack1;

        public string GetTwoHandLightAttack2() => twoHandLightAttack2;

        public string GetHeavyAttack1() => heavyAttack1;

        public string GetTwoHandHeavyAttack1() => twoHandHeavyAttack1;

        public int GetLightAttackStamina() => baseStamina * lightAttackMultiplier;

        public int GetHeavyAttackStamina() => baseStamina * heavyAttackMultiplier;

        public WeaponType GetWeaponType() => weaponType;

        #endregion
    }
}