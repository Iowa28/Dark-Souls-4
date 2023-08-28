using UnityEngine;

namespace DS
{
    public class EnemyStats : CharacterStats
    {
        private EnemyAnimatorManager animatorManager;

        private void Awake()
        {
            animatorManager = GetComponentInChildren<EnemyAnimatorManager>();
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
                animatorManager.PlayTargetAnimation("Death", true);
            }
            else
            {
                animatorManager.PlayTargetAnimation("Damage", true);
            }
        }

        public void TakeDamageWithoutAnimation(int damage)
        {
            currentHealth -= damage;
            
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }
    }
}