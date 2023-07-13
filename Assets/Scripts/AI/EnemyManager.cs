using System.Linq;
using UnityEngine;

namespace DS
{
    public class EnemyManager : CharacterManager
    {
        private EnemyLocomotionManager enemyLocomotionManager;
        private EnemyAnimationManager enemyAnimationManager;
        
        public bool isPerformingAction { get; set; }
        
        [Header("AI Settings")]
        [SerializeField]
        private float detectionRadius = 20f;

        [SerializeField]
        private float minDetectionAngle = -50f;

        [SerializeField]
        private float maxDetectionAngle = 50f;

        [SerializeField]
        private EnemyAttackAction[] enemyAttackActions;

        private EnemyAttackAction currentAttack;

        [SerializeField]
        private float currentRecoveryTime = 0;

        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimationManager = GetComponentInChildren<EnemyAnimationManager>();
        }

        private void Update()
        {
            HandleRecoveryTime();
        }

        private void FixedUpdate()
        {
            HandleCurrentAction();   
        }

        private void HandleCurrentAction()
        {
            enemyLocomotionManager.CalculateDistanceFromTarget();

            if (enemyLocomotionManager.currentTarget == null)
            {
                enemyLocomotionManager.HandleDetection();
            }
            else if (enemyLocomotionManager.IsCloseToTarget())
            {
                AttackTarget();
            }
            else
            {
                enemyLocomotionManager.HandleMoveToTarget();
            }
        }
        
        private void HandleRecoveryTime()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPerformingAction && currentRecoveryTime <= 0)
            {
                isPerformingAction = false;
            }
        }

        #region Attacks

        private void AttackTarget()
        {
            if (isPerformingAction)
                return;
        
            if (currentAttack == null)
            {
                GetNewAttack();
            }
            else
            {
                isPerformingAction = true;
                currentRecoveryTime = currentAttack.GetRecoveryTime();
                enemyAnimationManager.PlayTargetAnimation(currentAttack.GetActionAnimation(), true);
                currentAttack = null;
            }
        }

        private void GetNewAttack()
        {
            Vector3 targetDirection = enemyLocomotionManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            int maxScore = enemyAttackActions.Where(enemyAttackAction => 
                    enemyLocomotionManager.distanceFromTarget <= enemyAttackAction.GetMaxDistanceNeededToAttack() 
                    && enemyLocomotionManager.distanceFromTarget >= enemyAttackAction.GetMinDistanceNeededToAttack() 
                    && viewableAngle <= enemyAttackAction.GetMaxAttackAngle() 
                    && viewableAngle >= enemyAttackAction.GetMinAttackAngle()
                )
                .Sum(enemyAttackAction => enemyAttackAction.GetAttackScore());

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;
            
            foreach (EnemyAttackAction enemyAttackAction in enemyAttackActions)
            {
                if (enemyLocomotionManager.distanceFromTarget <= enemyAttackAction.GetMaxDistanceNeededToAttack()
                    && enemyLocomotionManager.distanceFromTarget >= enemyAttackAction.GetMinDistanceNeededToAttack()
                    && viewableAngle <= enemyAttackAction.GetMaxAttackAngle()
                    && viewableAngle >= enemyAttackAction.GetMinAttackAngle())
                {
                    temporaryScore += enemyAttackAction.GetAttackScore();

                    if (temporaryScore > randomValue)
                    {
                        currentAttack = enemyAttackAction;
                        break;
                    }
                }
            }
        }

        #endregion

        #region Getters

        public float GetDetectionRadius() => detectionRadius;

        public float GetMinDetectionAngle() => minDetectionAngle;

        public float GetMaxDetectionAngle() => maxDetectionAngle;

        #endregion
    }
}