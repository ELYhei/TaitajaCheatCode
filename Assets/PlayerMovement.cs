using UnityEngine;
using ELY.Core;

namespace ELY.PlayerCore
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("References")]
        Rigidbody2D rb;
        [SerializeField] Transform groundCheck;
        [SerializeField] LayerMask groundLayer;

        [Header("Settings")]
        [SerializeField] float walkingSpeed = 3f;
        [SerializeField] float runningSpeed = 5f;
        [SerializeField] float jumpForce = 4f;
        [SerializeField] float groundCheckRadius = 0.4f;

        [Header("Properties")]
        float speed;

        public bool isGrounded
        {
            get
            {
                return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            }
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            InputManager.Instance.OnJumpClicked += Instance_OnJumpClicked;
        }

        private void Instance_OnJumpClicked(object sender, System.EventArgs e)
        {
            Jump();
        }

        private void Update()
        {
            speed = walkingSpeed;
            HandleHorizontalMovement();
        }

        private void HandleHorizontalMovement()
        {
            Vector2 moveDirection = new Vector2(InputManager.Instance.movementInput.x, 0);
            rb.AddForce(moveDirection * speed);
        }

        private void Jump()
        {
            if (isGrounded)
            {
                Debug.Log("Jump Force");
                rb.AddForceY(jumpForce, ForceMode2D.Impulse);
            }
        }
    }
}
