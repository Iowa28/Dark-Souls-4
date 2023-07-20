using UnityEngine;

namespace DS
{
    public class SpellItem : MonoBehaviour
    {
        [SerializeField]
        private GameObject spellWarmUpFx;
        [SerializeField]
        private GameObject spellCastFx;
        [SerializeField]
        private string spellAnimation;

        [SerializeField]
        private SpellType SpellType;

        [SerializeField]
        [TextArea]
        private string spellDescription;

        public virtual void AttemptToCastSpell()
        {
            Debug.Log("You attempt to cast a spell!");
        }

        public virtual void SuccessfullyCastSpell()
        {
            Debug.Log("You successfully cast a spell!");
        }
    }
}