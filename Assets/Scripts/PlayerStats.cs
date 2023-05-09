using UnityEngine;

namespace DS
{
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField]
        private int healthLevel = 10;
        private int maxHealth;
        private int currentHealth;

        [SerializeField]
        private int staminaLevel = 10;
        private int maxStamina;
        private int currentStamina;
        
        private HealthBar healthBar;
        private StaminaBar staminaBar;

        private AnimatorHandler animatorHandler;

        private void Awake()
        {
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        private void Start()
        {
            SetMaxHealth();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            
            SetMaxStamina();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
        }

        private void SetMaxHealth()
        {
            maxHealth = healthLevel * 10;
        }
        
        private void SetMaxStamina()
        {
            maxStamina = staminaLevel * 10;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animatorHandler.PlayTargetAnimation("Death", true);
            }
            else
            {
                animatorHandler.PlayTargetAnimation("Damage", true);
            }
            
            healthBar.SetCurrentHealth(currentHealth);
        }

        public void DecreaseStamina(int value)
        {
            currentStamina -= value;

            if (currentStamina < 0)
            {
                currentStamina = 0;
            }
            
            staminaBar.SetCurrentStamina(currentStamina);
        }
    }
}