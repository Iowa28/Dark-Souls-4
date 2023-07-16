using UnityEngine;
using UnityEngine.AI;

namespace DS 
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        private EnemyManager enemyManager;
        private EnemyAnimationManager enemyAnimationManager;
        private NavMeshAgent navMeshAgent;
        public Rigidbody enemyRigidbody { get; private set; }

        [SerializeField]
        private float stoppingDistance = .5f;

        private float distanceFromTarget;

        [SerializeField]
        private float rotationSpeed = 15f;

        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            enemyAnimationManager = GetComponentInChildren<EnemyAnimationManager>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyRigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            navMeshAgent.enabled = false;
            enemyRigidbody.isKinematic = false;
        }

        public void CalculateDistanceFromTarget()
        {
            if (enemyManager.currentTarget != null)
            {
                distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);
            }
        }



        public void HandleMoveToTarget()
        {
            if (enemyManager.isPerformingAction)
                return;
            
            // Vector3 targetDirection = currentTarget.transform.position - transform.position;
            // distanceFromTarget = targetDirection.magnitude;
            // float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            if (enemyManager.isPerformingAction)
            {
                enemyAnimationManager.SetFloat("Vertical", 0, Time.deltaTime);
                navMeshAgent.enabled = false;
            }
            else if (distanceFromTarget > stoppingDistance)
            {
                enemyAnimationManager.SetFloat("Vertical", 1, Time.deltaTime);
            }
            else if (distanceFromTarget <= stoppingDistance)
            {
                enemyAnimationManager.SetFloat("Vertical", 0, Time.deltaTime);
            }
            
            HandleRotateTowardsTarget();
            
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;
        }

        private void HandleRotateTowardsTarget()
        {
            if (enemyManager.isPerformingAction)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
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
                navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyRigidbody.velocity = targetVelocity;
                transform.rotation = Quaternion.Slerp(transform.rotation, navMeshAgent.transform.rotation,
                    rotationSpeed / Time.deltaTime);
            }
        }

        public bool IsCloseToTarget() => distanceFromTarget <= stoppingDistance;
    }
}