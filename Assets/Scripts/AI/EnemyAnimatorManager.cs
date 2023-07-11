using UnityEngine;

namespace DS
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        private EnemyLocomotionManager enemyLocomotionManager;
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
            enemyLocomotionManager = GetComponentInParent<EnemyLocomotionManager>();
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;

            enemyLocomotionManager.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyLocomotionManager.enemyRigidbody.velocity = velocity;
        }
    }
}