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
            {
                animationManager.SetFloat("Vertical", 0, Time.deltaTime);
                return this;
            }

            float distanceFromTarget = enemyManager.DistanceFromTarget();
            
            if (distanceFromTarget > enemyManager.GetMaxAttackRange())
            {
                animationManager.SetFloat("Vertical", 1, Time.deltaTime);
            }

            HandleRotateTowardsTarget(enemyManager, distanceFromTarget);
            
            enemyManager.navMeshAgent.transform.localPosition = Vector3.zero;
            enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity;

            if (distanceFromTarget <= enemyManager.GetMaxAttackRange())
            {
                return combatStanceState;
            }

            return this;
        }
        
        private void HandleRotateTowardsTarget(EnemyManager enemyManager, float distanceFromTarget)
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
                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation,
                    enemyManager.navMeshAgent.transform.rotation, rotationSpeed / Time.deltaTime);

                // enemyManager.navMeshAgent.enabled = true;
                // enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                //
                // float rotationToApplyToDynamicEnemy = Quaternion.Angle(enemyManager.transform.rotation, Quaternion.LookRotation(enemyManager.navMeshAgent.desiredVelocity.normalized));
                // if (distanceFromTarget > 5)
                // {
                //     enemyManager.navMeshAgent.angularSpeed = 500f;
                // }
                // else if (distanceFromTarget < 5 && Mathf.Abs(rotationToApplyToDynamicEnemy) < 30)
                // {
                //     enemyManager.navMeshAgent.angularSpeed = 50f;
                // }
                // else if (distanceFromTarget < 5 && Mathf.Abs(rotationToApplyToDynamicEnemy) > 30)
                // {
                //     enemyManager.navMeshAgent.angularSpeed = 500f;
                // }
                //
                // Vector3 targetDirection = enemyManager.TargetDirection();
                // Quaternion rotationToApplyToStaticEnemy = Quaternion.LookRotation(targetDirection);
                //
                // if (enemyManager.navMeshAgent.desiredVelocity.magnitude > 0)
                // {
                //     enemyManager.navMeshAgent.updateRotation = false;
                //     enemyManager.transform.rotation = Quaternion.RotateTowards(enemyManager.transform.rotation,
                //         Quaternion.LookRotation(enemyManager.navMeshAgent.desiredVelocity.normalized), enemyManager.navMeshAgent.angularSpeed * Time.deltaTime);
                // }
                // else
                // {
                //     enemyManager.transform.rotation = Quaternion.RotateTowards(enemyManager.transform.rotation, rotationToApplyToStaticEnemy, enemyManager.navMeshAgent.angularSpeed * Time.deltaTime);
                // }
            }
        }
    }
}