using UnityEngine;

namespace DS
{
    public class EnemyManager : CharacterManager
    {
        [Header("AI Settings")]
        [SerializeField]
        private float detectionRadius = 20f;

        [SerializeField]
        private float minDetectionAngle = -50f;

        [SerializeField]
        private float maxDetectionAngle = 50f;

        public bool isPerformingAction { get; set; }

        private EnemyLocomotionManager enemyLocomotionManager;

        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        }

        private void Update()
        {
            
        }

        private void FixedUpdate()
        {
            HandleCurrentAction();   
        }

        private void HandleCurrentAction()
        {
            if (enemyLocomotionManager.GetCurrentTarget() == null)
            {
                enemyLocomotionManager.HandleDetection();
            }
            else
            {
                enemyLocomotionManager.HandleMoveToTarget();
            }
        }

        #region Getters

        public float GetDetectionRadius()
        {
            return detectionRadius;
        }

        public float GetMinDetectionAngle()
        {
            return minDetectionAngle;
        }        
        
        public float GetMaxDetectionAngle()
        {
            return maxDetectionAngle;
        }

        #endregion
    }
}