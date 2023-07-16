using UnityEngine;
using UnityEngine.AI;

namespace DS 
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        private EnemyManager enemyManager;
        private EnemyAnimationManager enemyAnimationManager;


        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            enemyAnimationManager = GetComponentInChildren<EnemyAnimationManager>();

            
        }

        private void Start()
        {
            
        }

        // public void CalculateDistanceFromTarget()
        // {
        //     if (enemyManager.currentTarget != null)
        //     {
        //         distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);
        //     }
        // }



        public void HandleMoveToTarget()
        {
            
        }


        //
        // public bool IsCloseToTarget() => distanceFromTarget <= stoppingDistance;
    }
}