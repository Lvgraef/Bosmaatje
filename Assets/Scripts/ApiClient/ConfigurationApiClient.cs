using System.Threading.Tasks;
using Dto;
using Global;
using JetBrains.Annotations;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace ApiClient
{
    public static class ConfigurationApiClient
    {
        [ItemCanBeNull]
        public static async Task<GetConfigurationsResponseDto> GetConfiguration()
        {
            var url = $"{ApiUtil.BaseUrl}/configurations";
            var response = await ApiUtil.PerformApiCall(url, "GET", token: UserSingleton.Instance.AccessToken);
            if (response == "Cannot connect to destination host") return null;
            return response == "HTTP/1.1 404 Not Found" ? null : JsonConvert.DeserializeObject<GetConfigurationsResponseDto>(response);
        }
        
        public static async Task<bool> Configure(PostConfigurationsRequestDto postRegisterRequestDto, TextMeshProUGUI statusText)
        {
            var url = $"{ApiUtil.BaseUrl}/configurations";
            var json = JsonConvert.SerializeObject(postRegisterRequestDto);
            var response = await ApiUtil.PerformApiCall(url, "POST", json, UserSingleton.Instance.AccessToken);
            
            statusText.color = Color.red;
            switch (response)
            {
                case "Cannot connect to destination host":
                    statusText.text = "Kan niet met server verbinden";
                    return false;
                case "HTTP/1.1 409 Conflict":
                    statusText.text = "Conflict";
                    return false;
                default:
                    statusText.color = Color.green;
                    statusText.text = "Succes!";
                    return true;
            }
        }

        public static async Task<bool> UpdateConfigure(PutConfigurationRequestDto putConfigurationRequestDto,
            TextMeshProUGUI statusText)
        {
            var url = $"{ApiUtil.BaseUrl}/configurations";
            var json = JsonConvert.SerializeObject(putConfigurationRequestDto);
            var response = await ApiUtil.PerformApiCall(url, "PUT", json, UserSingleton.Instance.AccessToken);
            
            switch (response)
            {
                case "Cannot connect to destination host":
                    statusText.text = "Kan niet met server verbinden";
                    return false;
                case "HTTP/1.1 404 Not Found":
                    statusText.text = "Not Found";
                    return false;
                default:
                    statusText.text = "Succes!";
                    return true;
            }
        }

        public static async Task<bool> PutFirstTreatment(PutTreatmentRequestDto putTreatmentRequestDto, TextMeshProUGUI statusText, string treatmentPlanName)
        {
            var treatments = await TreatmentPlanApiClient.GetTreatments(statusText, treatmentPlanName);
            
            var url = $"{ApiUtil.BaseUrl}/treatments?treatmentId={treatments![0].treatmentId}&treatmentPlanName={treatmentPlanName}";
            
            var json = JsonConvert.SerializeObject(putTreatmentRequestDto);
            var response = await ApiUtil.PerformApiCall(url, "PUT", json, UserSingleton.Instance.AccessToken);
            
            //todo
            switch (response)
            {
                case "Cannot connect to destination host":
                    statusText.text = "Kan niet met server verbinden";
                    return false;
                default:
                    statusText.text = "Configuratie opgeslagen!";
                    return true;
            }
        }
    }
}