using System.Threading.Tasks;
using Dto;
using Global;
using TMPro;
using UnityEngine;

namespace ApiClient
{
    public class LoginApiClient : MonoBehaviour
    {
        public async Task<bool> Login(TextMeshProUGUI statusMessage, string email, string password)
        {
            statusMessage.text = "Loading...";

            if (email.ToLower() == "" || password == "")
            {
                statusMessage.text = "Fill in both email and password.";
                return false;
            }

            var dto = new PostLoginRequestDto { email = email.ToLower(), password = password };
        
            var response = await ApiUtil.PerformApiCall("https://avansict2226538.azurewebsites.net/account/login", "Post",
                JsonUtility.ToJson(dto));

            if (response == "HTTP/1.1 401 Unauthorized")
            {
                statusMessage.text = "Wrong email or password";
                Debug.Log("Login failed.");
                return false;
            }

            if (response == "HTTP/1.1 500 Internal Server Error")
            {
                statusMessage.text = "Something went wrong :( Please try again.";
                Debug.Log("Server Error");
                return false;
            }

            if (response == "Cannot connect to destination host")
            {
                statusMessage.text = "Cannot connect to server";
                return false;
            }

            var postLoginResponseDto = JsonUtility.FromJson<PostLoginResponseDto>(response);


            UserSingleton.Instance.AccessToken = postLoginResponseDto.accessToken;
            UserSingleton.Instance.Name = email.ToLower();
            UserSingleton.Instance.ExpiresIn = postLoginResponseDto.expiresIn;
            UserSingleton.Instance.RefreshToken = postLoginResponseDto.refreshToken;
            UserSingleton.Instance.Updated();
            return true;
        }
    }
}
