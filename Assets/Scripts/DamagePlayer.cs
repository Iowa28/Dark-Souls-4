using UnityEngine;

namespace DS
{
    public class DamagePlayer : MonoBehaviour
    {
        [SerializeField]
        private int damage = 25;
        
        private void OnTriggerEnter(Collider other)
        {
            CharacterStats playerStats = other.GetComponent<CharacterStats>();

            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
            }
        }
    }
}