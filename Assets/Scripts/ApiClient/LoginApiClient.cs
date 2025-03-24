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
            statusMessage.text = "Loading...";
        
            var response = await ApiUtil.PerformApiCall($"{ApiUtil.BaseUrl}/account/login", "Post",
                JsonUtility.ToJson(request));

            switch (response)
            {
                case "HTTP/1.1 401 Unauthorized":
                    statusMessage.text = "Wrong email or password";
                    Debug.Log("Login failed.");
                    return false;
                case "HTTP/1.1 500 Internal Server Error":
                    statusMessage.text = "Something went wrong :( Please try again.";
                    Debug.Log("Server Error");
                    return false;
                case "Cannot connect to destination host":
                    statusMessage.text = "Cannot connect to server";
                    return false;
            }
            
            statusMessage.text = "Logged in!";

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
