using System;
using UnityEngine;

namespace DS
{
    public class PlayerManager : MonoBehaviour
    {
        private InputHandler inputHandler;
        private Animator animator;

        private void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            inputHandler.isInteracting = animator.GetBool("isInteracting");
            inputHandler.rollFlag = false;
            // inputHandler.sprintFlag = false;
        }
    }
}