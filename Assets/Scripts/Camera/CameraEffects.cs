using System.Collections;
using UnityEngine;

namespace ELY.Camera
{
    public class CameraEffects : MonoBehaviour
    {
        
        public void Shake(float duration, float magnitude)
        {
            StartCoroutine(ShakeCoroutine(duration, magnitude));
        }
        IEnumerator ShakeCoroutine(float duration, float magnitude)
        {
            Quaternion originalRotation = transform.rotation;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float z = Random.Range(-1f, 1f) * magnitude; // Only rotating on the Z-axis

                transform.rotation = Quaternion.Euler(0, 0, z);

                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.rotation = originalRotation; // Reset rotation
        }
    }
}
