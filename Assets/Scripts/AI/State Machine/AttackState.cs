using UnityEngine;

namespace DS
{
    public class AttackState : State
    {
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager animationManager)
        {
            return this;
        }
    }
}