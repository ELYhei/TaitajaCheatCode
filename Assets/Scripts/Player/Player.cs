using UnityEngine;

namespace ELY.PlayerCore
{
    public class Player : MonoBehaviour
    {
        private Vector3 spawnPosition = new Vector3(0, 0, 0);
        private const string playerPrefabName = "Player"; // Player GameObject In Resources Folder, Assets/Resources/Player.prefab
        #region Instance Creation On First Call
        private static readonly object _lock = new object();
        private static Player _instance;
        public static Player Instance 
        {
            get
            {
                if (Instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            Player prefab = Resources.Load<Player>(playerPrefabName);

                            if (prefab == null)
                            {
                                Debug.LogError("InputManager prefab not found in Resources!");
                            }
                            else
                            {
                                _instance = Instantiate(prefab);
                                DontDestroyOnLoad(_instance.gameObject);
                            }
                            _instance.transform.position = _instance.spawnPosition;
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
    }
}
