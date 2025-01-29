using UnityEngine;
using ELY.Core;
using NaughtyAttributes;
using System;
using System.Collections;

namespace ELY.PlayerCore
{
    public class PlayerMovement : MonoBehaviour
    {

        #region EVENTS
        //[Header("Events")]
        public event EventHandler OnJump;
        public event EventHandler OnLand;
        #endregion

        #region DEBUG & PROPERTIES
        [Header("Properties")]
        [ProgressBar("Stamina", 100f, EColor.Blue)]
        [SerializeField] float stamina;
        float speed;
        [Space(15f)]
        #endregion

        #region REFERENCES
        [Header("References")]
        [SerializeField] Rigidbody2D rb;
        [SerializeField] Transform groundCheck;
        [SerializeField] Transform wallCheck;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] LayerMask wallLayer;
        #endregion

        #region SETTINGS
        [Header("Settings")]
        [SerializeField] float walkingSpeed = 4f;
        [SerializeField] float runningSpeed = 7f;
        [Header("Jump")]
        [SerializeField] float jumpForce = 400f;
        [SerializeField] float groundCheckRadius = 0.3f;
        [Header("Wall Jump")]
        [SerializeField] float wallJumpForce = 550f;
        [SerializeField] float wallJumpTime = 1f; // Disables Movement For This Time
        [SerializeField] float wallCheckRadius = 0.4f;
        [Header("Stamina")]
        [SerializeField] float maxStamina = 100f;
        [Tooltip("drain stamina in a second")]
        [SerializeField] float staminaDrain = 7f;
        [Tooltip("gain stamina in a second")]
        [SerializeField] float staminaGain = 4f;
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
        
        private bool movementEnabled = true;
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
            else
            {
                TryWallJump();
            }
        }

        private void Update()
        {
            if (!movementEnabled) return;

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
        
        private void TryWallJump()
        {
            if (Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer))
            {
                StartCoroutine(PauseMovement());
                rb.linearVelocity = Vector2.zero;
                isRunning = false;
                rb.AddForce(-transform.right * wallJumpForce, ForceMode2D.Impulse);
                rb.AddForceY(wallJumpForce, ForceMode2D.Impulse);
            }
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
        
        IEnumerator PauseMovement()
        {
            movementEnabled = false;
            yield return new WaitForSeconds(wallJumpTime);
            movementEnabled = true;
        }

        void UpdateStamina()
        {
            if (isRunning && isMoving)
            {
                stamina -= staminaDrain * Time.deltaTime;
            }
            else if (stamina < maxStamina)
            {
                stamina += staminaGain * Time.deltaTime;
            }
        }
        
        public void FlipX(bool lookRight)
        {
            if (lookRight)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, 0f, transform.rotation.z);
            }
            else
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, 180f, transform.rotation.z);
            }
        }
        
        #endregion


    }
}
