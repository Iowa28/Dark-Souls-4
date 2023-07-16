using UnityEngine;

namespace DS
{
    public class EnemyAnimationManager : AnimatorManager
    {
        private EnemyManager enemyManager;
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
            enemyManager = GetComponentInParent<EnemyManager>();
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
    }
}