using UnityEngine;

namespace Global
{
    public class AudioSingleton : MonoBehaviour
    {
        public static AudioSingleton Instance { get; private set; }
        public AudioSource source;
        
        private void Awake()
        {
            // Destroy this object if we already have a singleton configured
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
	
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
}
