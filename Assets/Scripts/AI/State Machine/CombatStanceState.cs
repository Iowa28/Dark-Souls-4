using UnityEngine;

namespace DS
{
    public class CombatStanceState : State
    {
        [SerializeField] 
        private AttackState attackState;
        [SerializeField]
        private PursueTargetState pursueTargetState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager animationManager)
        {
            HandleRotateTowardsTarget(enemyManager);
        
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
        
        private void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            if (enemyManager.isPerformingAction)
            {
                Vector3 direction = enemyManager.TargetDirection();
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation,
                    enemyManager.GetRotationSpeed() / Time.deltaTime);
            }
            else
            {
                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation,
                    enemyManager.navMeshAgent.transform.rotation, enemyManager.GetRotationSpeed() / Time.deltaTime);
            }
        }
    }
}