using UnityEngine;

namespace DS
{
    public class EnemyStats : MonoBehaviour
    {
        [SerializeField]
        private int healthLevel = 10;
        private int maxHealth;
        private int currentHealth;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            maxHealth = healthLevel * 10;
            currentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animator.Play("Death");
            }
            else
            {
                animator.Play("Damage");
            }
        }
    }
}