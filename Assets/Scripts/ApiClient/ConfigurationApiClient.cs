using System;
using System.Threading.Tasks;
using Assets.Dto_s;
using Dto;
using Global;
using TMPro;
using UnityEngine;

namespace ApiClient
{
    public static class ConfigurationApiClient
    {
        public static async Task<bool> Configure(PostConfigureRequestDto postRegisterRequestDto, TextMeshProUGUI statusText)
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
            var url = $"{ApiUtil.BaseUrl}/configurations?treatment=0";
            
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