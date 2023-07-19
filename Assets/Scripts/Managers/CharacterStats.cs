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
        protected int maxStamina;
        protected int currentStamina;
        
        protected bool isDead;
    }
}