using UnityEngine;

namespace DS
{
    public class AnimatorHandler : MonoBehaviour
    {
        private PlayerManager playerManager;
        public Animator animator;
        // private InputHandler inputHandler;
        private PlayerLocomotion playerLocomotion;
        private int verticalHash;
        private int horizontalHash;
        private int isInteractingHash;
        public bool canRotate { get; private set; }

        [SerializeField]
        private float dampTime = .1f;
        [SerializeField]
        private float fadeDuration = .2f;

        public void Initialize()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            animator = GetComponent<Animator>();
            // inputHandler = GetComponentInParent<InputHandler>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            verticalHash = Animator.StringToHash("Vertical");
            horizontalHash = Animator.StringToHash("Horizontal");
            isInteractingHash = Animator.StringToHash("isInteracting");
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
            // else
            // {
            //     v = 0;
            // }

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
            // else
            // {
            //     h = 0;
            // }

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
            animator.SetBool(isInteractingHash, isInteracting);
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
    }
}