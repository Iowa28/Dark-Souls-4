using UnityEngine;
using UnityEngine.UI;

namespace DS
{
    public class WeaponPickUp : Interactable
    {
        [SerializeField]
        private WeaponItem weapon;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            PickUpItem(playerManager);
        }

        private void PickUpItem(PlayerManager playerManager)
        {
            // PlayerInventory playerInventory = playerManager.GetComponent<PlayerInventory>();
            // PlayerLocomotion playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            // AnimatorHandler animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();
            //
            // playerLocomotion.rigidbody.velocity = Vector3.zero;
            // animatorHandler.PlayTargetAnimation("PickUpItem", true);
            // playerInventory.weaponsInventory.Add(weapon);
            // playerManager.GetItemUIGameObject().GetComponentInChildren<Text>().text = weapon.GetItemName();
            // playerManager.GetItemUIGameObject().GetComponentInChildren<RawImage>().texture = weapon.GetItemIcon().texture;
            // playerManager.GetItemUIGameObject().SetActive(true);
            // Destroy(gameObject);
        }
    }
}