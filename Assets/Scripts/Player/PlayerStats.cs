using UnityEngine;

namespace DS
{
    public class PlayerStats : CharacterStats
    {
        private HealthBar healthBar;
        private StaminaBar staminaBar;

        private AnimatorHandler animatorHandler;
        private PlayerManager playerManager;
        
        [SerializeField]
        private float staminaRegenerationAmount = 1;
        private float staminaRegenerationTimer = 0;

        private void Awake()
        {
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            playerManager = GetComponent<PlayerManager>();
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
            if (playerManager.isInvulnerable)
                return;
            
            if (isDead)
                return;
            
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
                animatorHandler.PlayTargetAnimation("Death", true);
            }
            else
            {
                animatorHandler.PlayTargetAnimation("Damage", true);
            }
            
            healthBar.SetCurrentHealth(currentHealth);
        }

        public void DecreaseStamina(float value)
        {
            currentStamina -= value;

            if (currentStamina < 0)
            {
                currentStamina = 0;
            }
            
            staminaBar.SetCurrentStamina(currentStamina);
        }

        public void RegenerateStamina()
        {
            if (playerManager.isInteracting)
            {
                staminaRegenerationTimer = 0;
                return;
            }

            staminaRegenerationTimer += Time.deltaTime;

            if (currentStamina < maxStamina && staminaRegenerationTimer > 1f)
            {
                currentStamina += staminaRegenerationAmount * Time.deltaTime;
                staminaBar.SetCurrentStamina(currentStamina);
            }
        }

        public void HealPlayer(int amount)
        {
            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            
            healthBar.SetCurrentHealth(currentHealth);
        }
    }
}