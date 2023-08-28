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

        [Header("Spell Cost")]
        [SerializeField]
        protected int focusPointCost;

        [Header("Spell Type")]
        [SerializeField]
        protected SpellType spellType;

        [Header("Spell Description")]
        [SerializeField]
        [TextArea]
        protected string spellDescription;

        public virtual void AttemptToCastSpell(PlayerAnimatorManager playerAnimatorManager, WeaponSlotManager weaponSlotManager)
        {
            Debug.Log("Attempting to cast spell...");
        }

        public virtual void SuccessfullyCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStats playerStats)
        {
            playerStats.DeductFocusPoints(focusPointCost);
            Debug.Log("Spell cast successful");
        }

        #region Getters

        public bool IsFaithSpell() => spellType.Equals(SpellType.FaithSpell);
        
        public bool IsMagicSpell() => spellType.Equals(SpellType.MagicSpell);
        
        public bool IsPyromaniacSpell() => spellType.Equals(SpellType.PyromaniacSpell);

        public int GetFocusPointCost() => focusPointCost;

        #endregion
    }
}