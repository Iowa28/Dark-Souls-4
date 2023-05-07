using UnityEngine;

namespace DS 
{
    public class PlayerAttacker : MonoBehaviour
    {
        private AnimatorHandler animatorHandler;
        private InputHandler inputHandler;

        private string lastAttack;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            inputHandler = GetComponent<InputHandler>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (inputHandler.comboFlag)
            {
                animatorHandler.SetBool("canDoCombo", false);
                
                if (lastAttack == weapon.GetLightAttack1())
                {
                    animatorHandler.PlayTargetAnimation(weapon.GetLightAttack2(), true);
                }
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            animatorHandler.PlayTargetAnimation(weapon.GetLightAttack1(), true);
            lastAttack = weapon.GetLightAttack1();
        }
        
        public void HandleHeavyAttack(WeaponItem weapon)
        {
            animatorHandler.PlayTargetAnimation(weapon.GetHeavyAttack1(), true);
            lastAttack = weapon.GetHeavyAttack1();
        }
    }
}