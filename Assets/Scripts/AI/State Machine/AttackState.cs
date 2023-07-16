using System.Linq;
using UnityEngine;

namespace DS
{
    public class AttackState : State
    {
        [SerializeField]
        private CombatStanceState combatStanceState;
        
        [SerializeField]
        private EnemyAttackAction[] enemyAttackActions;
        
        private EnemyAttackAction currentAttack;
        
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager animationManager)
        {
            if (enemyManager.isPerformingAction)
                return combatStanceState;
            
            float distanceFromTarget = enemyManager.DistanceFromTarget();
            float viewableAngle = enemyManager.ViewableAngle();

            if (currentAttack != null)
            {
                if (distanceFromTarget < currentAttack.GetMinDistanceNeededToAttack())
                {
                    return this;
                }

                if (distanceFromTarget < currentAttack.GetMaxDistanceNeededToAttack() && 
                    viewableAngle <= currentAttack.GetMaxAttackAngle() &&
                    viewableAngle >= currentAttack.GetMinAttackAngle() &&
                    enemyManager.currentRecoveryTime <= 0 && !enemyManager.isPerformingAction)
                {
                    animationManager.SetFloat("Vertical", 0, Time.deltaTime);
                    animationManager.SetFloat("Horizontal", 0, Time.deltaTime);
                    animationManager.PlayTargetAnimation(currentAttack.GetActionAnimation(), true);
                    enemyManager.isPerformingAction = true;
                    enemyManager.currentRecoveryTime = currentAttack.GetRecoveryTime();
                    currentAttack = null;
                    return this;
                }
            }
            else
            {
                GetNewAttack(enemyManager);
            }

            return combatStanceState;
        }

        private void GetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);
            float distanceFromTarget = enemyManager.DistanceFromTarget();
        
            int maxScore = enemyAttackActions.Where(enemyAttackAction => 
                    distanceFromTarget <= enemyAttackAction.GetMaxDistanceNeededToAttack() 
                    && distanceFromTarget >= enemyAttackAction.GetMinDistanceNeededToAttack() 
                    && viewableAngle <= enemyAttackAction.GetMaxAttackAngle() 
                    && viewableAngle >= enemyAttackAction.GetMinAttackAngle()
                )
                .Sum(enemyAttackAction => enemyAttackAction.GetAttackScore());
        
            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;
            
            foreach (EnemyAttackAction enemyAttackAction in enemyAttackActions)
            {
                if (distanceFromTarget <= enemyAttackAction.GetMaxDistanceNeededToAttack()
                    && distanceFromTarget >= enemyAttackAction.GetMinDistanceNeededToAttack()
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
    }
}