using UnityEngine;

namespace DS
{
    public class PlayerAnimatorManager : AnimatorManager
    {
        private PlayerManager playerManager;
        private PlayerLocomotion playerLocomotion;
        private PlayerStats playerStats;
        private int verticalHash;
        private int horizontalHash;
        public bool canRotate { get; private set; }

        public void Initialize()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            animator = GetComponent<Animator>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            playerStats = GetComponentInParent<PlayerStats>();
            verticalHash = Animator.StringToHash("Vertical");
            horizontalHash = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting, float delta)
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
            
            SetFloat(verticalHash, v, delta);
            SetFloat(horizontalHash, h, delta);
        }

        public void CanRotate()
        {
            canRotate = true;
        }

        public void StopRotation()
        {
            canRotate = false;
        }

        public override void TakeCriticalDamage()
        {
            playerStats.TakeDamageWithoutAnimation(playerManager.pendingCriticalDamage);
            playerManager.pendingCriticalDamage = 0;
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
        
        #region Animation Events

        public void EnableCombo()
        {
            SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            SetBool("canDoCombo", false);
        }

        public void DrainStamina(float value)
        {
            playerStats.DecreaseStamina(value);
        }

        public void EnableIsInvulnerable()
        {
            SetBool("isInvulnerable", true);
        }
        
        public void DisableIsInvulnerable()
        {
            SetBool("isInvulnerable", false);
        }
        
        #endregion
    }
}