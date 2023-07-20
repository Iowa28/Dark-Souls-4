using UnityEngine;

namespace DS
{
    public class AmbushState : State
    {
        [SerializeField]
        private float detectionRadius = 2;

        [SerializeField] 
        private string sleepAnimation;

        [SerializeField]
        private string wakeAnimation;
        
        [SerializeField]
        private LayerMask detectionLayer;

        [SerializeField]
        private PursueTargetState pursueTargetState;
        
        private bool isSleeping = true;
        
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager animationManager)
        {
            if (isSleeping && !enemyManager.isInteracting)
            {
                animationManager.PlayTargetAnimation(sleepAnimation, true);
            }

            #region Handle Target Detection
            
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
                        isSleeping = false;
                        animationManager.PlayTargetAnimation(wakeAnimation, true);
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