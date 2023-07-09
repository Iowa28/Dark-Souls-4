using UnityEngine;

namespace DS 
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        private EnemyManager enemyManager;

        public CharacterStats currentTarget;
        
        [SerializeField]
        private LayerMask detectionLayer;

        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
        }

        public void HandleDetection()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.GetDetectionRadius(), detectionLayer);

            foreach (Collider c in colliders)
            {
                CharacterStats characterStats = c.transform.GetComponent<CharacterStats>();

                if (characterStats != null)
                {
                    Vector3 targetDirection = characterStats.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if (viewableAngle > enemyManager.GetMinDetectionAngle() &&
                        viewableAngle < enemyManager.GetMaxDetectionAngle())
                    {
                        currentTarget = characterStats;
                    }
                }
            }
        }

        #region Getters

        public CharacterStats GetCurrentTarget()
        {
            return currentTarget;
        }

        #endregion
    }
}