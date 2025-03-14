using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Dto_s;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.ApiCLient
{
    public class ApiClient
    {

        private string baseUrl = "https://ourApi";

        public async Task<string> Register(PostRegisterRequestDto postRegisterRequestDto)
        {
            string url = $"{baseUrl}/auth/register";// ik denk zoiets als de url van de api
            string json = JsonUtility.ToJson(postRegisterRequestDto);
            var response = await PerformApiCall(url, "POST", json);
            return response;
        }




        private async Task<string> PerformApiCall(string url, string method, string jsonData = null, string token = null)
        {
            using (UnityWebRequest request = new UnityWebRequest(url, method))
            {
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
                else
                {
                    Debug.LogError("Error during API call: " + request.error);
                    return null;
                }
            }
        }
    }
}
