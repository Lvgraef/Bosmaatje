using System.Threading.Tasks;
using Dto;
using Global;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace ApiClient
{
    public static class ConfigurationApiClient
    {
        [ItemCanBeNull]
        public static async Task<GetConfigurationsRequestDto> GetConfiguration()
        {
            var url = $"{ApiUtil.BaseUrl}/configurations";
            var response = await ApiUtil.PerformApiCall(url, "GET", token: "dasdf");
            if (response == "Cannot connect to destination host") return null;
            return response == "HTTP/1.1 404 Not Found" ? null : JsonUtility.FromJson<GetConfigurationsRequestDto>(response);
        }
        
        public static async Task<bool> Configure(PostConfigurationsRequestDto postRegisterRequestDto, TextMeshProUGUI statusText)
        {
            var url = $"{ApiUtil.BaseUrl}/configurations";
            var json = JsonUtility.ToJson(postRegisterRequestDto);
            var response = await ApiUtil.PerformApiCall(url, "POST", json, UserSingleton.Instance.AccessToken);

            //todo
            switch (response)
            {
                case "Cannot connect to destination host":
                    statusText.text = "Cannot connect to server";
                    return false;
                default:
                    statusText.text = "!";
                    return true;
            }
        }

        public static async Task<bool> PutFirstTreatment(PutTreatmentRequestDto putTreatmentRequestDto, TextMeshProUGUI statusText)
        {
            var url = $"{ApiUtil.BaseUrl}/treatments?treatment=0";
            
            var json = JsonUtility.ToJson(putTreatmentRequestDto);
            var response = await ApiUtil.PerformApiCall(url, "PUT", json, UserSingleton.Instance.AccessToken);
            
            //todo
            switch (response)
            {
                case "Cannot connect to destination host":
                    statusText.text = "Cannot connect to server";
                    return false;
                default:
                    statusText.text = "!";
                    return true;
            }
        }
    }
}