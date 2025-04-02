using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace ApiClient
{
    public static class ApiUtil
    {
        //public const string BaseUrl = "https://bosmaatje.azurewebsites.net";
        public const string BaseUrl = "https://localhost:7280";
        
        public static async Task<string> PerformApiCall(string url, string method, string jsonData = null, string token = null)
        {
            using UnityWebRequest request = new UnityWebRequest(url, method);
            if (!string.IsNullOrEmpty(jsonData))
            {
                byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            }

            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            
            if (!string.IsNullOrEmpty(token))
            {
                request.SetRequestHeader("Authorization", "Bearer " + token);
            }

            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                return request.downloadHandler.text;
            }

            Debug.LogError("Fout bij API-aanroep: " + request.error);
            return request.error;
        }
    }
}