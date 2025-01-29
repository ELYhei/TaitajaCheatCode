using UnityEngine;
using ELY.Camera;

namespace ELY.PlayerCore
{
    public class PlayerCameraController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Cam cam;
        [SerializeField] PlayerMovement playerMovement;

        [Header("Camera Shake Settings")]
        [SerializeField] Cam.ShakeType landShakeType;
        [SerializeField] float landShakeDuration = 0.1f;
        [SerializeField] Cam.ShakeType wallJumpShakeType;
        [SerializeField] float wallJumpShakeDuration = 0.1f;

        private void Start()
        {
            if (playerMovement == null) playerMovement = GetComponent<PlayerMovement>();
            if (cam == null) cam = UnityEngine.Camera.main.GetComponent<Cam>();

            playerMovement.OnLand += PlayerMovement_OnLand;
            playerMovement.OnWallJump += PlayerMovement_OnWallJump;
        }

        private void PlayerMovement_OnWallJump(object sender, System.EventArgs e)
        {
            cam.Shake(wallJumpShakeType, wallJumpShakeDuration);
        }

        private void PlayerMovement_OnLand(object sender, System.EventArgs e)
        {
            cam.Shake(landShakeType, landShakeDuration);
        }
    }
}
