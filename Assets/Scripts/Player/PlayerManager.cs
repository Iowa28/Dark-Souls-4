using UnityEngine;

namespace DS
{
    public class PlayerManager : CharacterManager
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
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleJumping();
            cameraHandler.SetCameraHeight(delta);
            
            CheckForInteractableObjects();
        }
        
        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        }

        private void LateUpdate()
        {
            inputHandler.ResetInputFlags();
            
            float delta = Time.deltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }

            if (isInAir)
            {
                playerLocomotion.inAirTimer += delta;
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

                        if (inputHandler.selectInput)
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
                
                if (itemUIGameObject != null && inputHandler.selectInput)
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