using UnityEngine;
using UnityEngine.AI;

namespace DS
{
    public class EnemyManager : CharacterManager
    {
        private EnemyAnimatorManager enemyAnimationManager;
        private EnemyStats enemyStats;
        public NavMeshAgent navMeshAgent { get; private set; }
        public Rigidbody enemyRigidbody { get; private set; }
        
        public bool isPerformingAction { get; set; }
        public bool isInteracting { get; private set; }
        
        [Header("AI Settings")]
        [SerializeField]
        private float minDetectionAngle = -50f;

        [SerializeField]
        private float maxDetectionAngle = 50f;

        public float currentRecoveryTime { get; set; }

        [SerializeField]
        private float maxAttackRange = 1.5f;
        
        [SerializeField]
        private float rotationSpeed = 15f;

        [SerializeField]
        private State currentState;
        
        public CharacterStats currentTarget { get; set; }

        private void Awake()
        {
            enemyAnimationManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyStats = GetComponent<EnemyStats>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyRigidbody = GetComponent<Rigidbody>();
            backStabCollider = GetComponentInChildren<BackStabCollider>();
        }

        private void Start()
        {
            navMeshAgent.enabled = false;
            enemyRigidbody.isKinematic = false;
        }

        private void Update()
        {
            HandleRecoveryTime();

            isInteracting = enemyAnimationManager.GetBool("isInteracting");
            enemyAnimationManager.SetBool("isDead", enemyStats.isDead);
        }

        private void FixedUpdate()
        {
            HandleStateMachine();   
        }

        private void HandleRecoveryTime()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPerformingAction && currentRecoveryTime <= 0)
            {
                isPerformingAction = false;
            }
        }

        private void HandleStateMachine()
        {
            if (currentState == null || enemyStats.isDead)
                return;
            
            if (currentTarget != null && currentTarget.isDead)
            {
                currentTarget = null;
                currentState = null;
                return;
            }
                
            State nextState = currentState.Tick(this, enemyStats, enemyAnimationManager);

            if (nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }

        private void SwitchToNextState(State state)
        {
            currentState = state;
        }

        public float DistanceFromTarget() => Vector3.Distance(currentTarget.transform.position, transform.position);

        public Vector3 TargetDirection() => currentTarget.transform.position - transform.position;

        public float ViewableAngle() => Vector3.Angle(TargetDirection(), transform.forward);

        #region Getters

        public float GetMinDetectionAngle() => minDetectionAngle;

        public float GetMaxDetectionAngle() => maxDetectionAngle;

        public float GetCurrentRecoveryTime() => currentRecoveryTime;

        public float GetMaxAttackRange() => maxAttackRange;

        public float GetRotationSpeed() => rotationSpeed;

        #endregion
    }
}