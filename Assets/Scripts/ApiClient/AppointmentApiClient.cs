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
    public static class AppointmentApiClient
    {
        [ItemCanBeNull]
        public static async Task<GetAppointmentsResponseDto[]> GetAppointments()
        {
            var url = $"{ApiUtil.BaseUrl}/appointments";
            var response = await ApiUtil.PerformApiCall(url, "GET", token: UserSingleton.Instance.AccessToken);

            return response switch
            {
                "HTTP/1.1 401 Unauthorized" or "Cannot connect to destination host" or "HTTP/1.1 400 Bad Request"
                    or "HTTP/1.1 500 Internal Server Error" => null,
                _ => JsonConvert.DeserializeObject<GetAppointmentsResponseDto[]>(response)
            };
        }

        public static async Task<bool> DeleteAppointment(Guid appointmentId, TextMeshProUGUI statusText)
        {
            var url = $"{ApiUtil.BaseUrl}/appointments?appointmentId={appointmentId}";
            var response = await ApiUtil.PerformApiCall(url, "DELETE", token: UserSingleton.Instance.AccessToken);

            switch (response)
            {
                case "HTTP/1.1 401 Unauthorized":
                    statusText.text = "Unauthorized";
                    return false;
                case "Cannot connect to destination host":
                    statusText.text = "Cannot connect to server";
                    return false;
                case "HTTP/1.1 400 Bad Request":
                    statusText.text = "Bad request";
                    return false;
                case "HTTP/1.1 500 Internal Server Error":
                    statusText.text = "Something went wrong :(";
                    return false;
                default:
                    statusText.color = Color.green;
                    statusText.text = "Got treatments!";
                    return true;
            }
        }
        
        public static async Task<bool> PostAppointment(PostAppointmentRequestDto dto,
            TextMeshProUGUI statusText)
        {
            var url = $"{ApiUtil.BaseUrl}/appointments";
            var response = await ApiUtil.PerformApiCall(url, "PUT", JsonConvert.SerializeObject(dto),
                UserSingleton.Instance.AccessToken);

            statusText.color = Color.red;
            
            switch (response)
            {
                case "HTTP/1.1 401 Unauthorized":
                    statusText.text = "Unauthorized";
                    return false;
                case "Cannot connect to destination host":
                    statusText.text = "Cannot connect to server";
                    return false;
                case "HTTP/1.1 400 Bad Request":
                    statusText.text = "Bad request";
                    return false;
                case "HTTP/1.1 500 Internal Server Error":
                    statusText.text = "Something went wrong :(";
                    return false;
                default:
                    statusText.color = Color.green;
                    statusText.text = "Saved appointment!";
                    return true;
            }
        }
    }
}