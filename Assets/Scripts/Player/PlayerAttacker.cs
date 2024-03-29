using UnityEngine;

namespace DS 
{
    public class PlayerAttacker : MonoBehaviour
    {
        private PlayerAnimatorManager playerAnimatorManager;
        private InputHandler inputHandler;
        private WeaponSlotManager weaponSlotManager;
        private PlayerManager playerManager;
        private PlayerInventory playerInventory;
        private PlayerStats playerStats;

        private string lastAttack;
        
        private LayerMask backStabLayer;

        private void Awake()
        {
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            inputHandler = GetComponentInParent<InputHandler>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            playerStats = GetComponentInParent<PlayerStats>();
        }

        private void Start()
        {
            backStabLayer = 1 << 12;
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
                    
                playerAnimatorManager.SetBool("isUsingRightHand", true);
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
                currentSpell.AttemptToCastSpell(playerAnimatorManager, weaponSlotManager);
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation("Shrug",true);
            }
        }
        
        private void PerformRBMagicAction(WeaponItem weaponItem)
        {

        }
        
        private void PerformRBPyromaniacAction(WeaponItem weaponItem)
        {

        }


        private void HandleWeaponCombo(WeaponItem weapon)
        {
            if (inputHandler.comboFlag)
            {
                playerAnimatorManager.SetBool("canDoCombo", false);
                if (lastAttack == weapon.GetLightAttack1())
                {
                    playerAnimatorManager.PlayTargetAnimation(weapon.GetLightAttack2(), true);
                    lastAttack = weapon.GetLightAttack2();
                }
                else if (lastAttack == weapon.GetTwoHandLightAttack1())
                {
                    playerAnimatorManager.PlayTargetAnimation(weapon.GetTwoHandLightAttack2(), true);
                    lastAttack = weapon.GetTwoHandLightAttack2();
                }
            }
        }

        private void HandleLightAttack(WeaponItem weapon)
        {
            weaponSlotManager.attackingWeapon = weapon;
            
            if (inputHandler.twoHandFlag)
            {
                playerAnimatorManager.PlayTargetAnimation(weapon.GetTwoHandLightAttack1(), true);
                lastAttack = weapon.GetTwoHandLightAttack1();
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation(weapon.GetLightAttack1(), true);
                lastAttack = weapon.GetLightAttack1();
            }
        }

        #endregion

        #region Input Actions

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            weaponSlotManager.attackingWeapon = weapon;
            
            if (inputHandler.twoHandFlag)
            {
                playerAnimatorManager.PlayTargetAnimation(weapon.GetTwoHandHeavyAttack1(), true);
                lastAttack = weapon.GetTwoHandHeavyAttack1();
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation(weapon.GetHeavyAttack(), true);
                lastAttack = weapon.GetHeavyAttack();
            }
        }
        
        public void SuccessfullyCastSpell()
        {
            SpellItem currentSpell = playerInventory.GetCurrentSpell();

            if (currentSpell != null)
            {
                currentSpell.SuccessfullyCastSpell(playerAnimatorManager, playerStats);
            }
        }
        
        public void AttemptBackStabOrRiposte()
        {
            if (Physics.Raycast(inputHandler.GetCriticalAttackRaycastStartPoint().position,
                    transform.TransformDirection(Vector3.forward), out RaycastHit hit, .5f, backStabLayer))
            {
                CharacterManager enemyCharacterManager =
                    hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

                if (enemyCharacterManager != null)
                {
                    playerManager.transform.position = enemyCharacterManager.GetBackStabCollider()
                        .GetBackStabberStandPoint().position;

                    Vector3 rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
                    targetRotation = Quaternion.Slerp(playerManager.transform.rotation, targetRotation,
                        500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventory.GetRightWeapon().GetCriticalDamageMultiplier() *
                                         rightWeapon.currentWeaponDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;
                    
                    playerAnimatorManager.PlayTargetAnimation("Back Stab", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Back Stabbed", true);
                    
                }
            }
        }

        #endregion
    }
}