using UnityEngine;

namespace DS
{
    public class PlayerStats : CharacterStats
    {
        private HealthBar healthBar;
        private StaminaBar staminaBar;
        private FocusPointBar focusPointBar;

        private AnimatorHandler animatorHandler;
        private PlayerManager playerManager;
        
        [SerializeField]
        private float staminaRegenerationAmount = 1;
        private float staminaRegenerationTimer = 0;

        private void Awake()
        {
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            focusPointBar = FindObjectOfType<FocusPointBar>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            playerManager = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            SetMaxHealth();
            SetMaxStamina();
            SetMaxFocusPoints();
        }

        private void SetMaxHealth()
        {
            maxHealth = healthLevel * 10;
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }
        
        private void SetMaxStamina()
        {
            maxStamina = staminaLevel * 10;
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
        }

        private void SetMaxFocusPoints()
        {
            maxFocusPoints = focusPointsLevel * 10;
            currentFocusPoints = maxFocusPoints;
            focusPointBar.SetMaxFocusPoints(maxFocusPoints);
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

        public void DeductFocusPoints(int focusPoints)
        {
            currentFocusPoints = Mathf.Max(0, currentFocusPoints - focusPoints);
            
            focusPointBar.SetCurrentFocusPoints(currentFocusPoints);
        }
    }
}