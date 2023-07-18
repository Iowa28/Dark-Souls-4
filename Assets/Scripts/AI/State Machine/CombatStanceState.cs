using UnityEngine;

namespace DS
{
    public class CombatStanceState : State
    {
        [SerializeField] 
        private AttackState attackState;
        [SerializeField]
        private PursueTargetState pursueTargetState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager animationManager)
        {
            float distanceFromTarget = enemyManager.DistanceFromTarget();

            if (enemyManager.isPerformingAction)
            {
                animationManager.SetFloat("Vertical", 0, Time.deltaTime);
            }
            
            if (enemyManager.GetCurrentRecoveryTime() <= 0 && distanceFromTarget <= enemyManager.GetMaxAttackRange())
            {
                return attackState;
            }
            if (distanceFromTarget > enemyManager.GetMaxAttackRange())
            {
                return pursueTargetState;
            }

            return this;
        }
    }
}