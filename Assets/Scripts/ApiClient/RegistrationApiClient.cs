using System.Threading.Tasks;
using Dto;
using TMPro;
using UnityEngine;

namespace ApiClient
{
    public static class RegistrationApiClient
    {
        public static async Task<bool> Register(PostRegisterRequestDto postRegisterRequestDto, TextMeshProUGUI statusText)
        {
            var url = $"{ApiUtil.BaseUrl}/account/register";
            var json = JsonUtility.ToJson(postRegisterRequestDto);
            var response = await ApiUtil.PerformApiCall(url, "POST", json);

            statusText.color = Color.red;
            switch (response)
            {
                case "HTTP/1.1 401 Unauthorized":
                    statusText.text = "Unauthorized";
                    return false;
                case "HTTP/1.1 400 Bad Request":
                    statusText.text = "User already exists";
                    return false;
                case "Cannot connect to destination host":
                    statusText.text = "Cannot connect to server";
                    return false;
                default:
                    statusText.color = Color.green;
                    statusText.text = "Registered successfully!";
                    return true;
            }
        }
    }
}