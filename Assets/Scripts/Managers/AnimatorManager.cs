using UnityEngine;

namespace DS
{
    public class AnimatorManager : MonoBehaviour
    {
        protected Animator animator;
        
        [SerializeField]
        protected float dampTime = .1f;
        [SerializeField]
        private float fadeDuration = .2f;
        
        
        public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
        {
            if (string.IsNullOrEmpty(targetAnimation))
                return;
            
            animator.applyRootMotion = isInteracting;
            SetBool("isInteracting", isInteracting);
            animator.CrossFade(targetAnimation, fadeDuration);
        }
        
        public void SetBool(string parameterName, bool value)
        {
            animator.SetBool(parameterName, value);
        }
        
        public bool GetBool(string parameterName)
        {
            return animator.GetBool(parameterName);
        }

        public void SetFloat(string parameterName, float value, float delta)
        {
            animator.SetFloat(parameterName, value, dampTime, delta);
        }
        
        public void SetFloat(int parameterHash, float value, float delta)
        {
            animator.SetFloat(parameterHash, value, dampTime, delta);
        }
    }
}