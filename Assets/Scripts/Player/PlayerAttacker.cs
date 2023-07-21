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
        private PlayerStats playerStats;

        private string lastAttack;

        private void Awake()
        {
            animatorHandler = GetComponent<AnimatorHandler>();
            inputHandler = GetComponentInParent<InputHandler>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            playerStats = GetComponentInParent<PlayerStats>();
        }

        public void HandleRBAction()
        {
            switch (playerInventory.GetRightWeapon().GetWeaponType())
            {
                case WeaponType.MeleeCaster:
                    PerformRBMeleeAction();
                    break;
                case WeaponType.FaithCaster:
                    PerformRBFaithAction(playerInventory.GetRightWeapon());
                    break;
                case WeaponType.SpellCaster:
                    PerformRBMagicAction(playerInventory.GetRightWeapon());
                    break;
                case WeaponType.PyromaniacCaster:
                    PerformRBPyromaniacAction(playerInventory.GetRightWeapon());
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
        
        private void PerformRBFaithAction(WeaponItem weaponItem)
        {
            if (playerManager.isInteracting)
                return;
            
            SpellItem currentSpell = playerInventory.GetCurrentSpell();

            if (currentSpell != null && currentSpell.IsFaithSpell() &&
                playerStats.currentFocusPoints >= currentSpell.GetFocusPointCost())
            {
                currentSpell.AttemptToCastSpell(animatorHandler, weaponSlotManager);
            }
            else
            {
                animatorHandler.PlayTargetAnimation("Shrug",true);
            }
        }
        
        private void PerformRBMagicAction(WeaponItem weaponItem)
        {

        }
        
        private void PerformRBPyromaniacAction(WeaponItem weaponItem)
        {

        }

        public void SuccessfullyCastSpell()
        {
            SpellItem currentSpell = playerInventory.GetCurrentSpell();

            if (currentSpell != null)
            {
                currentSpell.SuccessfullyCastSpell(animatorHandler, playerStats);
            }
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