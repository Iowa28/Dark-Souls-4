using UnityEngine;

namespace DS
{
    [CreateAssetMenu(menuName = "Items/Spells/Healing Spell")]
    public class HealingSpell : SpellItem
    {
        [SerializeField]
        private int healAmount;

        public override void AttemptToCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
        {
            // GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFx, animatorHandler.transform);
            // Destroy(instantiatedWarmUpSpellFX, 2f);
            animatorHandler.PlayTargetAnimation(spellAnimation, true);
            Debug.Log("Attempting to cast spell...");
        }

        public override void SuccessfullyCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
        {
            GameObject instantiatedSpellFX = Instantiate(spellCastFx, animatorHandler.transform);
            Destroy(instantiatedSpellFX, 2f);
            playerStats.HealPlayer(healAmount);
            Debug.Log("Spell cast successful");
        }
    }
}