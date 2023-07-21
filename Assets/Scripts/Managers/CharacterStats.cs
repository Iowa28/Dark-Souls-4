using UnityEngine;

namespace DS
{
    public class CharacterStats : MonoBehaviour
    {
        [SerializeField]
        protected int healthLevel = 10;
        protected int maxHealth;
        protected int currentHealth;

        [SerializeField]
        protected int staminaLevel = 10;
        protected float maxStamina;
        protected float currentStamina;

        [SerializeField]
        protected int focusPointsLevel = 10;
        protected float maxFocusPoints;
        public float currentFocusPoints { get; protected set; }
        
        public bool isDead { get; protected set; }
    }
}