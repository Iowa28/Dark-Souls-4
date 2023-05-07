using UnityEngine;

namespace DS
{
    public class AnimatorHandler : MonoBehaviour
    {
        private PlayerManager playerManager;
        private Animator animator;
        private PlayerLocomotion playerLocomotion;
        private int verticalHash;
        private int horizontalHash;
        public bool canRotate { get; private set; }

        [SerializeField]
        private float dampTime = .1f;
        [SerializeField]
        private float fadeDuration = .2f;

        public void Initialize()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            animator = GetComponent<Animator>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            verticalHash = Animator.StringToHash("Vertical");
            horizontalHash = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            const float limitDelta = .55f;

            #region Vertical

            float v = 0;

            if (verticalMovement > 0 && verticalMovement < limitDelta)
            {
                v = .5f;
            }
            else if (verticalMovement > limitDelta)
            {
                v = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -limitDelta)
            {
                v = -.5f;
            }
            else if (verticalMovement < -limitDelta)
            {
                v = -1;
            }

            #endregion
            
            #region Horizontal

            float h = 0;
            
            if (horizontalMovement > 0 && horizontalMovement < limitDelta)
            {
                h = .5f;
            }
            else if (horizontalMovement > limitDelta)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -limitDelta)
            {
                h = -.5f;
            }
            else if (horizontalMovement < -limitDelta)
            {
                h = -1;
            }

            #endregion

            if (isSprinting)
            {
                v = 2;
                h = horizontalMovement;
            }
            
            animator.SetFloat(verticalHash, v, dampTime, Time.deltaTime);
            animator.SetFloat(horizontalHash, h, dampTime, Time.deltaTime);
        }

        public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
        {
            animator.applyRootMotion = isInteracting;
            SetBool("isInteracting", isInteracting);
            animator.CrossFade(targetAnimation, fadeDuration);
        }

        public void CanRotate()
        {
            canRotate = true;
        }

        public void StopRotation()
        {
            canRotate = false;
        }

        private void OnAnimatorMove()
        {
            if (!playerManager.isInteracting)
                return;

            float delta = Time.deltaTime;
            playerLocomotion.rigidbody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerLocomotion.rigidbody.velocity = velocity;
        }

        public bool GetBool(string parameterName)
        {
            return animator.GetBool(parameterName);
        }

        public void SetBool(string parameterName, bool value)
        {
            animator.SetBool(parameterName, value);
        }

        public void EnableCombo()
        {
            SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            SetBool("canDoCombo", false);
        }
    }
}