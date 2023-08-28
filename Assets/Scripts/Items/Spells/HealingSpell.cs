using UnityEngine;

namespace DS
{
    [CreateAssetMenu(menuName = "Items/Spells/Healing Spell")]
    public class HealingSpell : SpellItem
    {
        [SerializeField]
        private int healAmount;

        public override void AttemptToCastSpell(PlayerAnimatorManager playerAnimatorManager, WeaponSlotManager weaponSlotManager)
        {
            base.AttemptToCastSpell(playerAnimatorManager, weaponSlotManager);
            GameObject rightHandWeapon = weaponSlotManager.rightHandSlot.currentWeaponModel;
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFx, rightHandWeapon.transform.GetChild(0));
            Destroy(instantiatedWarmUpSpellFX, 4f);
            playerAnimatorManager.PlayTargetAnimation(spellAnimation, true);
        }

        public override void SuccessfullyCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStats playerStats)
        {
            base.SuccessfullyCastSpell(playerAnimatorManager, playerStats);
            GameObject instantiatedSpellFX = Instantiate(spellCastFx, playerAnimatorManager.transform);
            Destroy(instantiatedSpellFX, 2f);
            playerStats.HealPlayer(healAmount);
        }
    }
}