using UnityEngine;

namespace DS
{
    [CreateAssetMenu(menuName = "Items/Spells/Healing Spell")]
    public class HealingSpell : SpellItem
    {
        [SerializeField]
        private int healAmount;

        public override void AttemptToCastSpell(AnimatorHandler animatorHandler, WeaponSlotManager weaponSlotManager)
        {
            base.AttemptToCastSpell(animatorHandler, weaponSlotManager);
            GameObject rightHandWeapon = weaponSlotManager.rightHandSlot.currentWeaponModel;
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFx, rightHandWeapon.transform.GetChild(0));
            Destroy(instantiatedWarmUpSpellFX, 4f);
            animatorHandler.PlayTargetAnimation(spellAnimation, true);
        }

        public override void SuccessfullyCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
        {
            base.SuccessfullyCastSpell(animatorHandler, playerStats);
            GameObject instantiatedSpellFX = Instantiate(spellCastFx, animatorHandler.transform);
            Destroy(instantiatedSpellFX, 2f);
            playerStats.HealPlayer(healAmount);
        }
    }
}