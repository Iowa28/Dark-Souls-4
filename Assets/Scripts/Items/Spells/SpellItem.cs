using UnityEngine;

namespace DS
{
    public class SpellItem : Item
    {
        [SerializeField]
        protected GameObject spellWarmUpFx;
        [SerializeField]
        protected GameObject spellCastFx;
        [SerializeField]
        protected string spellAnimation;

        [SerializeField]
        protected SpellType spellType;

        [SerializeField]
        [TextArea]
        protected string spellDescription;

        public virtual void AttemptToCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
        {
            Debug.Log("You attempt to cast a spell!");
        }

        public virtual void SuccessfullyCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
        {
            Debug.Log("You successfully cast a spell!");
        }

        public bool IsFaithSpell() => spellType.Equals(SpellType.FaithSpell);
        
        public bool IsMagicSpell() => spellType.Equals(SpellType.MagicSpell);
        
        public bool IsPyromaniacSpell() => spellType.Equals(SpellType.PyromaniacSpell);
        
    }
}