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
        private LayerMask ignoreLayers;
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
        
        private void Awake()
        {
            singleton = this;
            cameraTransform = GameObject.FindWithTag("MainCamera").transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
            targetTransform = FindObjectOfType<PlayerManager>().transform;
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
            lookAngle += (mouseXInput * lookSpeed) / delta;
            pivotAngle -= (mouseYInput * pivotSpeed) / delta;
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
                // float distance = (cameraPivotTransform.position - hit.point).sqrMagnitude;
                targetPosition = -(distance - cameraCollisionOffset);
            }

            if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
            {
                targetPosition = -minimumCollisionOffset;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / .2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }
    }
}