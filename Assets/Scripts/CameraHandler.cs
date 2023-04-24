using UnityEngine;

namespace DS
{
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField]
        private Transform targetTransform;
        private Transform cameraTransform;
        [SerializeField]
        private Transform cameraPivotTransform;
        private Vector3 cameraTransformPosition;
        private LayerMask ignoreLayers;

        public static CameraHandler singleton;

        [SerializeField]
        private float lookSpeed = .1f;
        [SerializeField]
        private float followSpeed = .1f;
        [SerializeField]
        private float pivotSpeed = .03f;

        private float defaultPosition;
        private float lookAngle;
        private float pivotAngle;
        [SerializeField]
        private float minimumPivot = -35;
        [SerializeField]
        private float maximumPivot = 35;

        private void Awake()
        {
            singleton = this;
            cameraTransform = GameObject.FindWithTag("MainCamera").transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        }

        public void FollowTarget(float delta)
        {
            Vector3 targetPosition = Vector3.Lerp(transform.position, targetTransform.position, delta / followSpeed);
            transform.position = targetPosition;
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
    }
}