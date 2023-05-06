using System;
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
        public HealthBar healthBar;

        private AnimatorHandler animatorHandler;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        private void Start()
        {
            maxHealth = healthLevel * 10;
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
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
    }
}