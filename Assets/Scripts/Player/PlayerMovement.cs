using UnityEngine;
using ELY.Core;
using NaughtyAttributes;
using System;

namespace ELY.PlayerCore
{
    public class PlayerMovement : MonoBehaviour
    {

        #region EVENTS
        //[Header("Events")]
        public event EventHandler OnJump;
        public event EventHandler OnLand;
        #endregion

        #region DEBUG
        [Header("Properties")]
        [ProgressBar("Stamina", 100f, EColor.Blue)]
        [SerializeField] float stamina;
        [SerializeField] float speed;
        [Space(15f)]
        #endregion

        #region REFERENCES
        [Header("References")]
        [SerializeField] Rigidbody2D rb;
        [SerializeField] Transform groundCheck;
        [SerializeField] LayerMask groundLayer;
        #endregion

        #region SETTINGS
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
        #endregion

        #region PROPERTIES
        public Vector2 velocity { get { return rb.linearVelocity; } }
        private bool hasStamina { get { return stamina > 0; } }
        public bool isRunning { get; private set; }
        public bool isMoving { get { return Mathf.Abs(rb.linearVelocity.x) > 0.1f; } }
        bool IsGrounded = false;
        public bool isGrounded
        {
            get
            {
                if (Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer))
                {
                    if (!IsGrounded)
                    {
                        OnLand?.Invoke(this, EventArgs.Empty);
                    }
                    IsGrounded = true;
                }
                else
                {
                    IsGrounded = false;
                }
                
                return IsGrounded;
            }
        }
        #endregion
        
        private void Start()
        {
            if (rb == null)
                rb = GetComponent<Rigidbody2D>();

            InputManager.Instance.OnJumpClicked += Instance_OnJumpClicked;
            stamina = maxStamina;
        }

        private void Instance_OnJumpClicked(object sender, System.EventArgs e)
        {
            if (isGrounded)
            {
                Jump();
            }
        }

        private void Update()
        {
            HandleHorizontalMovement();
            HandleSprinting();
            UpdateStamina();
        }

        #region MOVEMENT LOGIC
        private void HandleHorizontalMovement()
        {
            float horizontalInput = InputManager.Instance.movementInput.normalized.x;
            rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocityY);
        }
        
        private void HandleSprinting()
        {
            if (!InputManager.Instance.sprintPressed || !hasStamina)
            {
                StopRunning();
                return;
            }

            // Prevent sprinting if not grounded or stamina is too low
            if (!isRunning && (!isGrounded || stamina < minimunStaminaToSprint))
            {
                StopRunning();
                return;
            }

            StartRunning();
        }
        
        private void Jump()
        {
            OnJump?.Invoke(this, EventArgs.Empty);
            rb.AddForceY(jumpForce, ForceMode2D.Impulse);
        }
        
        private void StartRunning()
        {
            speed = runningSpeed;
            isRunning = true;
        }

        private void StopRunning()
        {
            speed = walkingSpeed;
            isRunning = false;
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
        #endregion
    
    }
}
