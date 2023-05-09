using UnityEngine;

namespace DS
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField]
        private float radius = .6f;
        [SerializeField ]
        private string interactableText;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        public virtual void Interact(PlayerManager playerManager)
        {
            Debug.Log("You interacted");
        }

        #region Getters

        public string GetInteractableText()
        {
            return interactableText;
        }

        #endregion
    }
}