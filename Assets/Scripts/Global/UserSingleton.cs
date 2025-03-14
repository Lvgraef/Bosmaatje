using System.Threading.Tasks;
using ApiClient;
using Dto;
using UnityEngine;

namespace Global
{
    public class UserSingleton : MonoBehaviour
    {
        public static UserSingleton Instance { get; private set; }
    
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        public string Name { get; set; }
        
        public void Updated()
        {
            Invoke(nameof(Refresh), ExpiresIn);
        }

        private async Task Refresh()
        {
            var refreshToken = await ApiUtil.PerformApiCall($"https://localhost:7244/account/refresh", "POST", $"{{\"refreshToken\":\"{RefreshToken}\"}}");
            var token = JsonUtility.FromJson<PostLoginResponseDto>(refreshToken);
            Debug.Log("Refreshed Token!");
            AccessToken = token.accessToken;
            RefreshToken = token.refreshToken;
            ExpiresIn = token.expiresIn;
            Updated();
        }

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