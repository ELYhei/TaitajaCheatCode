using UnityEngine;

namespace ELY
{
    public class FollowTarget : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform targetTransform;

        [Header("Settings")]
        [SerializeField] Vector2 offset;

        private void LateUpdate()
        {
            transform.position = new Vector3(targetTransform.position.x + offset.x, targetTransform.position.y + offset.y, transform.position.z);
        }
    }
}
