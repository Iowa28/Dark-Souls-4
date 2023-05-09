using UnityEngine;

namespace DS 
{
    public class PlayerAttacker : MonoBehaviour
    {
        private AnimatorHandler animatorHandler;
        private InputHandler inputHandler;
        private WeaponSlotManager weaponSlotManager;

        private string lastAttack;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            inputHandler = GetComponent<InputHandler>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (inputHandler.comboFlag)
            {
                animatorHandler.SetBool("canDoCombo", false);
                
                if (lastAttack == weapon.GetLightAttack1())
                {
                    animatorHandler.PlayTargetAnimation(weapon.GetLightAttack2(), true);
                    lastAttack = weapon.GetLightAttack2();
                }
                else if (lastAttack == weapon.GetLightAttack2())
                {
                    animatorHandler.PlayTargetAnimation(weapon.GetLightAttack3(), true);
                    lastAttack = weapon.GetLightAttack3();
                }
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            weaponSlotManager.attackingWeapon = weapon;
            animatorHandler.PlayTargetAnimation(weapon.GetLightAttack1(), true);
            lastAttack = weapon.GetLightAttack1();
        }
        
        public void HandleHeavyAttack(WeaponItem weapon)
        {
            weaponSlotManager.attackingWeapon = weapon;
            animatorHandler.PlayTargetAnimation(weapon.GetHeavyAttack1(), true);
            lastAttack = weapon.GetHeavyAttack1();
        }
    }
}