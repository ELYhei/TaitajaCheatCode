using UnityEngine;

namespace ELY.PlayerCore
{
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }

    }
}
