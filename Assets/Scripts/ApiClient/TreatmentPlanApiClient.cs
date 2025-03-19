using System.Threading.Tasks;
using Dto;
using Global;
using JetBrains.Annotations;
using Newtonsoft.Json;
using TMPro;

namespace ApiClient
{
    public static class TreatmentPlanApiClient
    {
        [ItemCanBeNull]
        public static async Task<GetTreatmentRequestDto[]> GetTreatments(TextMeshProUGUI statusText, string treatmentPlanId)
        {
            var url = $"{ApiUtil.BaseUrl}/treatments?treatmentPlanId={treatmentPlanId}";
            var response = await ApiUtil.PerformApiCall(url, "GET", token: UserSingleton.Instance.AccessToken);
            
            switch (response)
            {
                case "HTTP/1.1 401 Unauthorized":
                    statusText.text = "Unauthorized";
                    return null;
                case "Cannot connect to destination host":
                    statusText.text = "Cannot connect to server";
                    return null;
                default:
                    statusText.text = "Registered successfully!";
                    return JsonConvert.DeserializeObject<GetTreatmentRequestDto[]>(response);
            }
        }
    }
}