using UnityEngine;

namespace DS 
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        private EnemyManager enemyManager;
        private EnemyAnimationManager enemyAnimationManager;

        [SerializeField]
        private CapsuleCollider characterCollider;
        [SerializeField]
        private CapsuleCollider characterCollisionBlockerCollider;

        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            enemyAnimationManager = GetComponentInChildren<EnemyAnimationManager>();
        }

        private void Start()
        {
            Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
        }
    }
}