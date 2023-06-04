using UnityEngine;

namespace DS
{
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField]
        private Transform targetTransform;
        [SerializeField]
        private Transform cameraPivotTransform;
        private Transform cameraTransform;
        private Vector3 cameraTransformPosition;
        public LayerMask ignoreLayers { get; private set; }
        private Vector3 cameraFollowVelocity = Vector3.zero;

        public static CameraHandler singleton { get; private set; }

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

        [Header("Lock On")]
        [SerializeField]
        private float maxLockOnDistance = 26f;
        [SerializeField]
        private float maxViewableAngle = 50f;
        private Transform currentLockOnTarget;
        // private List<CharacterManager> availableTargets = new List<CharacterManager>();

        private InputHandler inputHandler;
        
        private void Awake()
        {
            singleton = this;
            cameraTransform = GameObject.FindWithTag("MainCamera").transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
            targetTransform = FindObjectOfType<PlayerManager>().transform;
            inputHandler = FindObjectOfType<InputHandler>();
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
            Transform nearestLockOnTarget = null;

            Collider[] colliders = Physics.OverlapSphere(targetTransform.position, maxLockOnDistance);

            foreach (Collider characterCollider in colliders)
            {
                CharacterManager character = characterCollider.GetComponent<CharacterManager>();

                if (character != null)
                {
                    Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                    // float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                    float distanceFromTarget = lockTargetDirection.magnitude;
                    float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);

                    if (character.transform.root != targetTransform.transform.root 
                        && viewableAngle > -maxViewableAngle 
                        && viewableAngle < maxViewableAngle
                        && distanceFromTarget < shortestDistance)
                    {
                        shortestDistance = distanceFromTarget;
                        nearestLockOnTarget = character.GetLockOnTransform();
                    }
                }
            }

            if (nearestLockOnTarget != null)
            {
                currentLockOnTarget = nearestLockOnTarget;
            }
        }

        public bool IsLockOnActive()
        {
            return currentLockOnTarget != null;
        }

        public void ClearLockOnTarget()
        {
            currentLockOnTarget = null;
        }
    }
}