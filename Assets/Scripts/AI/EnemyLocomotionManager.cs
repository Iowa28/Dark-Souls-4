using UnityEngine;
using UnityEngine.AI;

namespace DS 
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        private EnemyManager enemyManager;
        private EnemyAnimatorManager enemyAnimatorManager;
        private NavMeshAgent navMeshAgent;
        public Rigidbody enemyRigidbody { get; private set; }
        
        private CharacterStats currentTarget;
        
        [SerializeField]
        private LayerMask detectionLayer;

        [SerializeField]
        private float stoppingDistance = .5f;
        private float distanceFromTarget;

        [SerializeField]
        private float rotationSpeed = 15f;

        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyRigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            navMeshAgent.enabled = false;
            enemyRigidbody.isKinematic = false;
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

        public void HandleMoveToTarget()
        {
            Vector3 targetDirection = currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
            distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
            

            if (enemyManager.isPerformingAction)
            {
                enemyAnimatorManager.SetFloat("Vertical", 0, Time.deltaTime);
                navMeshAgent.enabled = false;
            }
            else if (distanceFromTarget > stoppingDistance)
            {
                enemyAnimatorManager.SetFloat("Vertical", 1, Time.deltaTime);
            }
            else if (distanceFromTarget <= stoppingDistance)
            {
                enemyAnimatorManager.SetFloat("Vertical", 0, Time.deltaTime);
            }
            
            HandleRotateTowardsTarget();
            
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;
        }

        private void HandleRotateTowardsTarget()
        {
            if (enemyManager.isPerformingAction)
            {
                Vector3 direction = currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed / Time.deltaTime);
            }
            else
            {
                Vector3 relativeDirection = transform.InverseTransformDirection(navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyRigidbody.velocity;

                navMeshAgent.enabled = true;
                navMeshAgent.SetDestination(currentTarget.transform.position);
                enemyRigidbody.velocity = targetVelocity;
                transform.rotation = Quaternion.Slerp(transform.rotation, navMeshAgent.transform.rotation,
                    rotationSpeed / Time.deltaTime);
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