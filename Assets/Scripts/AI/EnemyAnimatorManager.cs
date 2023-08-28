using UnityEngine;

namespace DS
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        private EnemyManager enemyManager;
        private EnemyStats enemyStats;
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
            enemyManager = GetComponentInParent<EnemyManager>();
            enemyStats = GetComponentInParent<EnemyStats>();
        }

        public override void TakeCriticalDamage()
        {
            enemyStats.TakeDamageWithoutAnimation(enemyManager.pendingCriticalDamage);
            enemyManager.pendingCriticalDamage = 0;
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;

            enemyManager.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidbody.velocity = velocity;
        }
        
        #region Animation Events
        
        public void EnableCombo()
        {
            // SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            // SetBool("canDoCombo", false);
        }
        
        #endregion
    }
}