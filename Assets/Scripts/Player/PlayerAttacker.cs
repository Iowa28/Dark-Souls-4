using UnityEngine;

namespace DS 
{
    public class PlayerAttacker : MonoBehaviour
    {
        private AnimatorHandler animatorHandler;
        private InputHandler inputHandler;
        private WeaponSlotManager weaponSlotManager;
        private PlayerManager playerManager;
        private PlayerInventory playerInventory;

        private string lastAttack;

        private void Awake()
        {
            animatorHandler = GetComponent<AnimatorHandler>();
            inputHandler = GetComponentInParent<InputHandler>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerInventory = GetComponentInParent<PlayerInventory>();
        }

        public void HandleRBAction()
        {
            switch (playerInventory.GetRightWeapon().GetWeaponType())
            {
                case WeaponType.MeleeCaster:
                    PerformRBMeleeAction();
                    break;
                case WeaponType.PyromaniacCaster:
                case WeaponType.FaithCaster:
                case WeaponType.SpellCaster:
                    PerformRBMagicAction(playerInventory.GetRightWeapon());
                    break;
            }
        }

        #region Attack Actions

        private void PerformRBMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleWeaponCombo(playerInventory.GetRightWeapon());
                inputHandler.comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                    return;
                if (playerManager.canDoCombo)
                    return;
                    
                animatorHandler.SetBool("isUsingRightHand", true);
                HandleLightAttack(playerInventory.GetRightWeapon());
            }
        }
        
        private void PerformRBMagicAction(WeaponItem weaponItem)
        {
            
        }

        #endregion

        private void HandleWeaponCombo(WeaponItem weapon)
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
                else if (lastAttack == weapon.GetTwoHandLightAttack1())
                {
                    animatorHandler.PlayTargetAnimation(weapon.GetTwoHandLightAttack2(), true);
                    lastAttack = weapon.GetTwoHandLightAttack2();
                }
            }
        }

        private void HandleLightAttack(WeaponItem weapon)
        {
            weaponSlotManager.attackingWeapon = weapon;
            
            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAnimation(weapon.GetTwoHandLightAttack1(), true);
                lastAttack = weapon.GetTwoHandLightAttack1();
            }
            else
            {
                animatorHandler.PlayTargetAnimation(weapon.GetLightAttack1(), true);
                lastAttack = weapon.GetLightAttack1();
            }
        }
        
        public void HandleHeavyAttack(WeaponItem weapon)
        {
            weaponSlotManager.attackingWeapon = weapon;
            
            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAnimation(weapon.GetTwoHandHeavyAttack1(), true);
                lastAttack = weapon.GetTwoHandHeavyAttack1();
            }
            else
            {
                animatorHandler.PlayTargetAnimation(weapon.GetHeavyAttack1(), true);
                lastAttack = weapon.GetHeavyAttack1();
            }
        }
    }
}