using UnityEngine;

namespace ELY.Camera
{
    public class CameraMovement : MonoBehaviour
    {

        public void MoveTo(Transform target, Vector2 offset)
        {
            transform.position = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);
        }

    }
}
