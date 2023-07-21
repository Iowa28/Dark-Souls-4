using UnityEngine;

namespace DS
{
    public class CharacterStats : MonoBehaviour
    {
        [SerializeField]
        protected int healthLevel = 10;
        protected int maxHealth;
        public int currentHealth { get; set; }

        [SerializeField]
        protected int staminaLevel = 10;
        protected float maxStamina;
        protected float currentStamina;
        
        public bool isDead { get; protected set; }
    }
}