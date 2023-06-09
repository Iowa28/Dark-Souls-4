using UnityEngine;

namespace DS
{
    public class CameraHandler : MonoBehaviour
    {
        private InputHandler inputHandler;
        private PlayerManager playerManager;
        
        [Header("Target and pivot transforms")]
        [SerializeField]
        private Transform targetTransform;
        [SerializeField]
        private Transform cameraPivotTransform;
        public Transform cameraTransform { get; private set; }
        private Vector3 cameraTransformPosition;
        public LayerMask ignoreLayers { get; private set; }
        private LayerMask environmentLayer;
        private Vector3 cameraFollowVelocity = Vector3.zero;

        public static CameraHandler singleton { get; private set; }

        [Header("Camera rotation settings")]
        [SerializeField]
        private float lookSpeed = .1f;
        [SerializeField]
        private float followSpeed = .1f;
        [SerializeField]
        private float pivotSpeed = .03f;

        private float targetPosition;
        private float defaultPosition;
        private float lookAngle;
        private float pivotAngle;
        [SerializeField]
        private float minimumPivot = -35;
        [SerializeField]
        private float maximumPivot = 35;

        [SerializeField]
        private float cameraSphereRadius = .2f;
        [SerializeField]
        private float cameraCollisionOffset = .2f;
        [SerializeField]
        private float minimumCollisionOffset = .2f;

        private float lockedPivotPosition = 2.25f;
        private float unlockedPivotPosition = 1.65f;

        [Header("Lock on settings")]
        [SerializeField]
        private float maxLockOnDistance = 26f;
        [SerializeField]
        private float maxViewableAngle = 50f;

        public Transform currentLockOnTarget { get; private set; }
        private Transform leftLockOnTarget;
        private Transform rightLockOnTarget;
        
        private void Awake()
        {
            singleton = this;
            cameraTransform = FindObjectOfType<Camera>().transform;
            defaultPosition = cameraTransform.localPosition.z;
            inputHandler = FindObjectOfType<InputHandler>();
            playerManager = FindObjectOfType<PlayerManager>();
            targetTransform = playerManager.transform;
        }

        private void Start()
        {
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
            environmentLayer = LayerMask.NameToLayer("Environment");
        }

        public void FollowTarget(float delta)
        {
            Vector3 target = Vector3.SmoothDamp(transform.position, targetTransform.position,
                ref cameraFollowVelocity, delta / followSpeed);
            transform.position = target;
            
            HandleCameraCollisions(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            if (inputHandler.lockOnFlag && currentLockOnTarget != null)
            {
                RotateToCurrentLockOnTarget();
                return;
            }
            
            lookAngle += mouseXInput * lookSpeed * delta;
            pivotAngle -= mouseYInput * pivotSpeed * delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);
            
            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            transform.rotation = targetRotation;
            
            rotation = Vector3.zero;
            rotation.x = pivotAngle;
            
            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
        }

        private void RotateToCurrentLockOnTarget()
        {
            Vector3 direction = currentLockOnTarget.position - transform.position;
            direction.Normalize();
            direction.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;

            // ReSharper disable once Unity.InefficientPropertyAccess
            direction = currentLockOnTarget.position - cameraPivotTransform.position;
            direction.Normalize();

            targetRotation = Quaternion.LookRotation(direction);
            Vector3 eulerAngle = targetRotation.eulerAngles;
            eulerAngle.y = 0;
            cameraPivotTransform.localEulerAngles = eulerAngle;
        }

        private void HandleCameraCollisions(float delta)
        {
            targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit,
                    Mathf.Abs(targetPosition), ignoreLayers))
            {
                float distance = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(distance - cameraCollisionOffset);
            }

            if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
            {
                targetPosition = -minimumCollisionOffset;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / .2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }

        public void HandleLockOn()
        {
            float shortestDistance = Mathf.Infinity;
            float leftTargetShortestDistance = Mathf.Infinity;
            float rightTargetShortestDistance = Mathf.Infinity;
            
            Transform nearestLockOnTarget = null;

            Collider[] colliders = Physics.OverlapSphere(targetTransform.position, maxLockOnDistance);

            foreach (Collider characterCollider in colliders)
            {
                CharacterManager character = characterCollider.GetComponent<CharacterManager>();

                if (character == null || character.transform.root == targetTransform.transform.root)
                {
                    continue;
                }

                Vector3 characterPosition = character.transform.position;
                Vector3 lockTargetDirection = characterPosition - targetTransform.position;
                float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);

                if (viewableAngle < -maxViewableAngle || viewableAngle > maxViewableAngle)
                {
                    continue;
                }

                RaycastHit hit;
                if (!Physics.Linecast(playerManager.GetLockOnTransform().position, character.GetLockOnTransform().position, out hit))
                {
                    continue;
                }
                
                Debug.DrawLine(playerManager.GetLockOnTransform().position, character.GetLockOnTransform().position);
                
                if (hit.transform.gameObject.layer == environmentLayer)
                {
                    continue;
                }

                float distanceFromTarget = lockTargetDirection.magnitude;
                
                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = character.GetLockOnTransform();
                }

                if (!inputHandler.lockOnFlag || character.transform.root == currentLockOnTarget.transform.root)
                {
                    continue;
                }
                
                Vector3 relativeEnemyPosition = currentLockOnTarget.InverseTransformDirection(characterPosition);
                Vector3 currentLockOnTargetPosition = currentLockOnTarget.transform.position;
                float distanceFromLeftTarget = currentLockOnTargetPosition.x - characterPosition.x;
                float distanceFromRightTarget = currentLockOnTargetPosition.x + characterPosition.x;

                // Debug.Log("left: " + distanceFromLeftTarget + ", right: " + distanceFromRightTarget + ", pos: " + character.GetLockOnTransform().position);

                if (relativeEnemyPosition.x > 0.00 && distanceFromLeftTarget < leftTargetShortestDistance)
                {
                    // Debug.Log("left target " + character.GetLockOnTransform().position);
                    // Debug.Log(character.GetLockOnTransform().position);
                    leftTargetShortestDistance = distanceFromTarget;
                    leftLockOnTarget = character.GetLockOnTransform();
                }

                if (relativeEnemyPosition.x < 0.00 && distanceFromRightTarget < rightTargetShortestDistance)
                {
                    // Debug.Log("right target " + character.GetLockOnTransform().position);
                    // Debug.Log(character.GetLockOnTransform().position);
                    rightTargetShortestDistance = distanceFromRightTarget;
                    rightLockOnTarget = character.GetLockOnTransform();
                }
            }

            if (nearestLockOnTarget != null)
            {
                currentLockOnTarget = nearestLockOnTarget;
            }
        }

        public void ClearLockOnTarget()
        {
            currentLockOnTarget = null;
        }

        public bool IsLockOnActive()
        {
            return currentLockOnTarget != null;
        }

        public void SwitchToLeftLockOn()
        {
            if (leftLockOnTarget != null)
            {
                currentLockOnTarget = leftLockOnTarget;
            }
        }

        public void SwitchToRightLockOn()
        {
            if (rightLockOnTarget != null)
            {
                currentLockOnTarget = rightLockOnTarget;
            }
        }

        public void SetCameraHeight(float delta)
        {
            Vector3 velocity = Vector3.zero;
            float newPositionY = currentLockOnTarget != null ? lockedPivotPosition : unlockedPivotPosition;
            Vector3 newPosition = new Vector3(0, newPositionY);

            cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition,
                newPosition, ref velocity, delta);
        }
    }
}