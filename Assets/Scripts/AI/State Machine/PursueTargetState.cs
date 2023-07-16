using UnityEngine;

namespace DS
{
    public class PursueTargetState : State
    {
        [SerializeField]
        private float rotationSpeed = 15f;

        [SerializeField]
        private CombatStanceState combatStanceState;
        
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager animationManager)
        {
            if (enemyManager.isPerformingAction)
                return this;

            float distanceFromTarget = enemyManager.DistanceFromTarget();
            
            if (distanceFromTarget > enemyManager.GetMaxAttackRange())
            {
                animationManager.SetFloat("Vertical", 1, Time.deltaTime);
            }

            HandleRotateTowardsTarget(enemyManager);
            
            enemyManager.navMeshAgent.transform.localPosition = Vector3.zero;
            enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity;

            if (distanceFromTarget <= enemyManager.GetMaxAttackRange())
            {
                return combatStanceState;
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
                    rotationSpeed / Time.deltaTime);
            }
            else
            {
                Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;

                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.enemyRigidbody.velocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation,
                    enemyManager.navMeshAgent.transform.rotation, rotationSpeed / Time.deltaTime);
            }
        }
    }
}