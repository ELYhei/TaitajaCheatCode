using UnityEngine;
using ELY.Core;
using ELY.PlayerCore;

namespace ELY
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [Header("References")]
        Animator animator;
        PlayerMovement movementController;
        SpriteRenderer spriteRenderer;

        [Header("Animator Params")]
        const string xVelo = "xVelocity";
        const string yVelo = "yVelocity";
        const string jumpTrigger = "Jump";
        const string groundedBool = "IsGrounded";

        private void Start()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            movementController = Utils.FindComponentInParents<PlayerMovement>(transform);
            movementController.OnJump += MovementController_OnJump;
        }

        private void MovementController_OnJump(object sender, System.EventArgs e)
        {
            animator.SetTrigger(jumpTrigger);
        }

        private void Update()
        {
            if (movementController.velocity.x < -0.1) movementController.FlipX(false);
            else if (movementController.velocity.x > 0.1) movementController.FlipX(true);
            animator.SetBool(groundedBool, movementController.isGrounded);
            animator.SetFloat(xVelo, Mathf.Abs(movementController.velocity.x));
            animator.SetFloat(yVelo, movementController.velocity.y);
        }
    }
}
