using UnityEngine;

namespace DS
{
    public class DamagePlayer : MonoBehaviour
    {
        [SerializeField]
        private int damage = 25;
        
        private void OnTriggerEnter(Collider other)
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
            }
        }
    }
}