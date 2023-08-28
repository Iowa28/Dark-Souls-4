using UnityEngine;

namespace DS
{
    public class DamageCollider : MonoBehaviour
    {
        private Collider damageCollider;

        public int currentWeaponDamage  { get; set; }

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerStats playerStats = collision.GetComponent<PlayerStats>();
                
                if (playerStats != null)
                {
                    playerStats.TakeDamage(currentWeaponDamage);
                }
            }
            
            if (collision.CompareTag("Enemy"))
            {
                EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
                
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(currentWeaponDamage);
                }
            }
        }
    }
}