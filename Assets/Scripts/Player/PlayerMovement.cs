using UnityEngine;
using ELY.Core;
using NaughtyAttributes;

namespace ELY.PlayerCore
{
    public class PlayerMovement : MonoBehaviour
    {

        [Header("Properties")]
        [ProgressBar("Stamina", 100f, EColor.Blue)]
        [SerializeField] float stamina;
        [SerializeField] float speed;

        [Space(15f)]

        [Header("References")]
        [SerializeField] Rigidbody2D rb;
        [SerializeField] Transform groundCheck;
        [SerializeField] LayerMask groundLayer;

        [Header("Settings")]
        [SerializeField] float walkingSpeed = 4f;
        [SerializeField] float runningSpeed = 7f;
        [SerializeField] float jumpForce = 400f;
        [SerializeField] float groundCheckRadius = 0.4f;
        [Header("Stamina")]
        [SerializeField] float maxStamina = 100f;
        [Tooltip("drain stamina in a second")]
        [SerializeField] float staminaDrain = 3f;
        [Tooltip("gain stamina in a second")]
        [SerializeField] float staminaGain = 2f;
        [Tooltip("If stamina is less than this can not sprint")]
        [SerializeField] float minimunStaminaToSprint = 10f;


        private bool hasStamina { get { return stamina > 0; } }
        public bool isRunning { get; private set; }
        public bool isMoving { get { return rb.linearVelocity.x > 0.1f; } }
        public bool isGrounded
        {
            get
            {
                return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            }
        }

        private void Start()
        {
            if (rb == null)
                rb = GetComponent<Rigidbody2D>();

            InputManager.Instance.OnJumpClicked += Instance_OnJumpClicked;
            stamina = maxStamina;
        }

        private void Instance_OnJumpClicked(object sender, System.EventArgs e)
        {
            Jump();
        }

        private void Update()
        {
            HandleHorizontalMovement();
            HandleSprinting();
            UpdateStamina();
        }

        private void HandleSprinting()
        {
            if (InputManager.Instance.sprintPressed && hasStamina)
            {
                // Dont start running if not grounded
                if (!isRunning && !isGrounded)
                {
                    speed = walkingSpeed;
                    return;
                }
                // Dont start running if stamina is less than minimum stamina needed
                if (!isRunning && stamina < minimunStaminaToSprint)
                {
                    speed = walkingSpeed;
                    return;
                }
                speed = runningSpeed;
                isRunning = true;
            }
            else
            {
                speed = walkingSpeed;
                isRunning = false;
            }
        }

        void UpdateStamina()
        {
            if (isRunning)
            {
                stamina -= staminaDrain * Time.deltaTime;
            }
            else if (stamina < maxStamina)
            {
                stamina += staminaGain * Time.deltaTime;
            }
        }

        private void HandleHorizontalMovement()
        {
            float horizontalInput = InputManager.Instance.movementInput.x;
            rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocityY);
        }

        private void Jump()
        {
            if (isGrounded)
            {
                rb.AddForceY(jumpForce, ForceMode2D.Impulse);
            }
        }
    }
}
