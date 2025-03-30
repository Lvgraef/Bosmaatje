using System;
using System.Threading.Tasks;
using Dto;
using Global;
using JetBrains.Annotations;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

namespace ApiClient
{
    public static class TreatmentPlanApiClient
    {
        [ItemCanBeNull]
        public static async Task<GetTreatmentResponseDto[]> GetTreatments(TextMeshProUGUI statusText, [CanBeNull] string treatmentPlanName)
        {
            var url = $"{ApiUtil.BaseUrl}/treatments?treatmentPlanName={treatmentPlanName}";
            var response = await ApiUtil.PerformApiCall(url, "GET", token: UserSingleton.Instance.AccessToken);
            
            statusText.color = Color.red;
            
            switch (response)
            {
                case "HTTP/1.1 401 Unauthorized":
                    statusText.text = "Niet geautoriseerd";
                    return null;
                case "Cannot connect to destination host":
                    statusText.text = "Kan niet met server verbinden";
                    return null;
                case "HTTP/1.1 400 Bad Request":
                    statusText.text = "Bad request";
                    return null;
                case "HTTP/1.1 500 Internal Server Error":
                    statusText.text = "Er ging iets fout :(";
                    return null;
                default:
                    statusText.color = Color.green;
                    statusText.text = "Behandelingen opgehaald!";
                    return JsonConvert.DeserializeObject<GetTreatmentResponseDto[]>(response);
            }
        }

        public static async Task<bool> PutTreatment(Guid treatmentId, PutTreatmentRequestDto dto)
        {
            var url = $"{ApiUtil.BaseUrl}/treatments?treatmentId={treatmentId}";
            var response = await ApiUtil.PerformApiCall(url, "PUT", JsonConvert.SerializeObject(dto), UserSingleton.Instance.AccessToken);

            return response switch
            {
                "HTTP/1.1 401 Unauthorized" => false,
                "Cannot connect to destination host" => false,
                "HTTP/1.1 400 Bad Request" => false,
                "HTTP/1.1 500 Internal Server Error" => false,
                _ => true
            };
        }
    }
}