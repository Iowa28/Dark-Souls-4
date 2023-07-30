using UnityEngine;

namespace DS
{
    public class EnemyStats : CharacterStats
    {
        private EnemyAnimationManager animationManager;

        private void Awake()
        {
            animationManager = GetComponentInChildren<EnemyAnimationManager>();
        }

        private void Start()
        {
            maxHealth = healthLevel * 10;
            currentHealth = maxHealth;
        }

        public override void TakeDamage(int damage)
        {
            if (isDead)
                return;
            
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
                animationManager.PlayTargetAnimation("Death", true);
            }
            else
            {
                animationManager.PlayTargetAnimation("Damage", true);
            }
        }
    }
}