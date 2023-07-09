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

        private bool isPerformingAction;

        private EnemyLocomotionManager enemyLocomotionManager;

        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        }

        private void Update()
        {
            HandleCurrentAction();   
        }

        private void HandleCurrentAction()
        {
            if (enemyLocomotionManager.GetCurrentTarget() == null)
            {
                enemyLocomotionManager.HandleDetection();
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