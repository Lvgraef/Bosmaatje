using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dto;
using Global;
using UnityEngine;

namespace ApiClient
{
    public static class DiaryApiClient
    {
        public static async Task<GetDiaryDatesRequestDto> DaysInDiary()
        {
            var url = $"{ApiUtil.BaseUrl}/diary";
            var response = await ApiUtil.PerformApiCall(url, "GET", token: UserSingleton.Instance.AccessToken);
            if (response == "Cannot connect to destination host") return null;
            return JsonUtility.FromJson<GetDiaryDatesRequestDto>(response);
        }

        public static async Task<GetSpecificDiaryContentResponseDto> GetSpecificDiaryContent(GetSpecificDiaryContentRequestDto DiaryDto)
        {
            var url = $"{ApiUtil.BaseUrl}/diary?date={DiaryDto.date}";
            var response = await ApiUtil.PerformApiCall(url, "GET", token: UserSingleton.Instance.AccessToken);
            if (response == "Cannot connect to destination host") return null;
            return JsonUtility.FromJson<GetSpecificDiaryContentResponseDto>(response);
        } 

        public static async Task<bool> PutSpecificDiaryContent(PutSpecificDiaryContentRequestDto diaryDto)
        {
            var url = $"{ApiUtil.BaseUrl}/diary?date={diaryDto.date}";
            string json = JsonUtility.ToJson(diaryDto);
            var response = await ApiUtil.PerformApiCall(url, "GET", token: UserSingleton.Instance.AccessToken, jsonData: json);

            if (response == "No content") return true;

            if (response == "Cannot connect to destination host") return false;
            return false;
        }

        public static async Task<bool> PostSpecificDiaryContent(PostSpecificDiaryContentRequestDto diaryDto)
        {
            var url = $"{ApiUtil.BaseUrl}/diary?date={diaryDto.date}";
            var response = await ApiUtil.PerformApiCall(url, "GET", token: UserSingleton.Instance.AccessToken);
            var json = JsonUtility.ToJson(diaryDto);
            if (response == "No content") return true;

            if (response == "Cannot connect to destination host") return false;
            return false;
        }
    }
}
