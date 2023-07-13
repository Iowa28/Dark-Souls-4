using UnityEngine;

namespace DS
{
    [CreateAssetMenu(menuName = "AI/Enemy Actions/Attack Action")]
    public class EnemyAttackAction : EnemyAction
    {
        [SerializeField]
        private int attackScore = 3;
        [SerializeField]
        private float recoveryTime = 2;

        [SerializeField]
        private float minAttackAngle = -35;
        [SerializeField]
        private float maxAttackAngle = 35;

        [SerializeField]
        private float minDistanceNeededToAttack = 0;
        [SerializeField]
        private float maxDistanceNeededToAttack = 3;

        #region Getters

        public int GetAttackScore() => attackScore;

        public float GetRecoveryTime() => recoveryTime;

        public float GetMinAttackAngle() => minAttackAngle;

        public float GetMaxAttackAngle() => maxAttackAngle;

        public float GetMinDistanceNeededToAttack() => minDistanceNeededToAttack;
        
        public float GetMaxDistanceNeededToAttack() => maxDistanceNeededToAttack;

        #endregion
    }
}