using UnityEngine;
using ELY.PlayerCore;
using ELY.Core;

namespace ELY
{
    public class PlayerSounds : MonoBehaviour
    {
        [Header("Refereneces")]
        [SerializeField] PlayerMovement playerMovement;
        [SerializeField] float walkingStepDelay = 0.3f;
        [SerializeField] float runningStepDelay = 0.1f;

        private void Start()
        {
            if (playerMovement == null) playerMovement = GetComponent<PlayerMovement>();

            playerMovement.OnJump += PlayerMovement_OnJump;
            playerMovement.OnLand += PlayerMovement_OnLand;
        }

        private void PlayerMovement_OnLand(object sender, System.EventArgs e)
        {
            SoundManager.PlaySound(SoundManager.Sound.Landing);
        }

        private void PlayerMovement_OnJump(object sender, System.EventArgs e)
        {
            SoundManager.PlaySound(SoundManager.Sound.Jump);
        }

        private void Update()
        {
            if (playerMovement.isMoving && playerMovement.isGrounded)
            {
                if (playerMovement.isRunning)
                {
                    SoundManager.PlaySoundWithCooldown(SoundManager.Sound.Step_Rock, runningStepDelay);
                }
                else
                {
                    SoundManager.PlaySoundWithCooldown(SoundManager.Sound.Step_Rock, walkingStepDelay);
                }
            }
            
        }

    }
}
