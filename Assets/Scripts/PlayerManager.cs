using UnityEngine;

namespace DS
{
    public class PlayerManager : MonoBehaviour
    {
        private CameraHandler cameraHandler;
        private InputHandler inputHandler;
        private PlayerLocomotion playerLocomotion;
        private AnimatorHandler animatorHandler;
        
        private InteractableUI interactableUI;
        [SerializeField]
        private GameObject interactionUIGameObject;
        [SerializeField]
        private GameObject itemUIGameObject;

        public bool isInteracting { get; private set; }
        public bool isSprinting { get; set; }
        public bool isInAir { get; set; }
        public bool isGrounded { get; set; }
        public bool canDoCombo { get; private set; }
        
        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
        }

        private void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            interactableUI = FindObjectOfType<InteractableUI>();
        }

        private void Update()
        {
            isInteracting = animatorHandler.GetBool("isInteracting");
            canDoCombo = animatorHandler.GetBool("canDoCombo");
            animatorHandler.SetBool("isInAir", isInAir);

            float delta = Time.deltaTime;
            inputHandler.TickInput(delta);
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
            playerLocomotion.HandleJumping();
            
            CheckForInteractableObjects();
        }
        
        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }
        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;
            inputHandler.rbInput = false;
            inputHandler.rtInput = false;
            inputHandler.eInput = false;
            inputHandler.dPadRight = false;
            inputHandler.dPadLeft = false;
            inputHandler.dPadUp = false;
            inputHandler.dPadDown = false;
            inputHandler.jumpInput = false;

            if (isInAir)
            {
                playerLocomotion.inAirTimer += Time.deltaTime;
            }
        }

        private void CheckForInteractableObjects()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, .5f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
            {
                if (hit.collider.CompareTag("Interactable"))
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if (interactableObject != null)
                    {
                        string interactableText = interactableObject.GetInteractableText();
                        interactableUI.SetInteractableText(interactableText);
                        interactionUIGameObject.SetActive(true);

                        if (inputHandler.eInput)
                        {
                            interactableObject.Interact(this);
                        }
                    }
                }
            }
            else
            {
                if (interactionUIGameObject != null)
                {
                    interactionUIGameObject.SetActive(false);
                }
                
                if (itemUIGameObject != null && inputHandler.eInput)
                {
                    itemUIGameObject.SetActive(false);
                }
            }
        }

        #region Getters

        public GameObject GetItemUIGameObject()
        {
            return itemUIGameObject;
        }

        #endregion
    }
}