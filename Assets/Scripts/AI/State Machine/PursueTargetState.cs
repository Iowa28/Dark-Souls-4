using UnityEngine;

namespace DS
{
    public class PursueTargetState : State
    {
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager animationManager)
        {
            return this;
        }
    }
}