using UnityEngine;

namespace DS
{
    public class IdleState : State
    {
        [SerializeField]
        private float detectionRadius = 20f;
        
        [SerializeField]
        private LayerMask detectionLayer;

        [SerializeField]
        private PursueTargetState pursueTargetState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager animationManager)
        {
            #region Handle Enemy Target Detection

            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

            foreach (Collider c in colliders)
            {
                CharacterStats characterStats = c.transform.GetComponent<CharacterStats>();

                if (characterStats != null && !characterStats.isDead)
                {
                    Vector3 targetDirection = characterStats.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if (viewableAngle > enemyManager.GetMinDetectionAngle() &&
                        viewableAngle < enemyManager.GetMaxDetectionAngle())
                    {
                        enemyManager.currentTarget = characterStats;
                        return pursueTargetState;
                    }
                }
            }
            
            #endregion

            if (enemyManager.currentTarget != null)
            {
                return pursueTargetState;
            }

            return this;
        }
    }
}