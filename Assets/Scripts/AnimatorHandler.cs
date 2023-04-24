using UnityEngine;

namespace DS
{
    public class AnimatorHandler : MonoBehaviour
    {
        private Animator animator;
        private int vertical;
        private int horizontal;
        public bool canRotate { get; private set; }

        private const float dampTime = .1f;

        public void Initialize()
        {
            animator = GetComponent<Animator>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
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
            
            animator.SetFloat(vertical, v, dampTime, Time.deltaTime);
            animator.SetFloat(horizontal, h, dampTime, Time.deltaTime);
        }

        public void CanRotate()
        {
            canRotate = true;
        }

        public void StopRotation()
        {
            canRotate = false;
        }
    }
}