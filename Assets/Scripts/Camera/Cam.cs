using Unity.VisualScripting;
using UnityEngine;

namespace ELY.Camera
{
    public class Cam : MonoBehaviour
    {
        [Header("References")]
        CameraMovement cameraMovement;
        CameraEffects cameraEffects;

        [Header("Settings")]
        [SerializeField] Transform target;
        [SerializeField] Vector2 offset;

        public enum ShakeType
        {
            Small,
            Medium,
            High,
            Super,
        }
        float magnitude;

        private void Awake()
        {
            cameraMovement = transform.AddComponent<CameraMovement>();
            cameraEffects = transform.AddComponent<CameraEffects>();            
        }


        private void LateUpdate()
        {
            UpdateMovement();
        }

        void UpdateMovement()
        {
            cameraMovement.MoveTo(target, offset);
        }

        public void Shake(ShakeType shakeType, float duration)
        {
            SetShakeSettings(shakeType);
            cameraEffects.Shake(duration, magnitude);
        }

        private void SetShakeSettings(ShakeType shakeType)
        {
            switch (shakeType)
            {
                case ShakeType.Small:
                    magnitude = 0.5f;
                    break;
                case ShakeType.Medium:
                    magnitude = 1f;
                    break;
                case ShakeType.High:
                    magnitude = 4f;
                    break;
                case ShakeType.Super:
                    magnitude = 8f;
                    break;
                default:
                    magnitude = 0.5f;
                    break;
            }
        }

    }
}
