using System.Threading.Tasks;
using Dto;
using Global;
using TMPro;
using UnityEngine;

namespace ApiClient
{
    public static class LoginApiClient
    {
        public static async Task<bool> Login(TextMeshProUGUI statusMessage, PostLoginRequestDto request)
        {
            statusMessage.color = Color.yellow;
            statusMessage.text = "Loading...";
        
            var response = await ApiUtil.PerformApiCall($"{ApiUtil.BaseUrl}/account/login", "Post",
                JsonUtility.ToJson(request));

            statusMessage.color = Color.red;
            
            switch (response)
            {
                case "HTTP/1.1 401 Unauthorized":
                    statusMessage.text = "Verkeerd email of wachtwoord";
                    Debug.Log("Login failed.");
                    return false;
                case "HTTP/1.1 500 Internal Server Error":
                    statusMessage.text = "Er ging iets fout :( Probeer later opnieuw.";
                    Debug.Log("Server Error");
                    return false;
                case "Cannot connect to destination host":
                    statusMessage.text = "Kan niet met server verbinden";
                    return false;
            }
            statusMessage.color = Color.green;

            statusMessage.text = "Ingelogd!";

            var postLoginResponseDto = JsonUtility.FromJson<PostLoginResponseDto>(response);


            UserSingleton.Instance.AccessToken = postLoginResponseDto.accessToken;
            UserSingleton.Instance.Name = request.email.ToLower();
            UserSingleton.Instance.ExpiresIn = postLoginResponseDto.expiresIn;
            UserSingleton.Instance.RefreshToken = postLoginResponseDto.refreshToken;
            UserSingleton.Instance.Updated();
            return true;
        }
    }
}
