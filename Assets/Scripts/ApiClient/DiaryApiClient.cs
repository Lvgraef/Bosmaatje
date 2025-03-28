using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dto;
using Global;
using UnityEngine;
using Newtonsoft.Json;

namespace ApiClient
{
    public static class DiaryApiClient
    {
        public static async Task<List<DiaryReadDto>> DaysInDiary()
        {
            var url = $"{ApiUtil.BaseUrl}/diaries";
            var response = await ApiUtil.PerformApiCall(url, "GET", token: UserSingleton.Instance.AccessToken);
            if (response == "Cannot connect to destination host") return null;
            return JsonConvert.DeserializeObject<List<DiaryReadDto>>(response);
        }

        //public static async Task<GetSpecificDiaryContentResponseDto> GetSpecificDiaryContent(GetSpecificDiaryContentRequestDto DiaryDto)
        //{
        //    var url = $"{ApiUtil.BaseUrl}/diary?date={DiaryDto.date}";
        //    var response = await ApiUtil.PerformApiCall(url, "GET", token: UserSingleton.Instance.AccessToken);
        //    if (response == "Cannot connect to destination host") return null;
        //    return JsonUtility.FromJson<GetSpecificDiaryContentResponseDto>(response);
        //} 

        public static async Task<bool> PutDiaryContent(PutDiaryContentRequestDto diaryDto, DateTime date)
        {
            var formattedDate = date.ToString("yyyy-MM-dd");
            var url = $"{ApiUtil.BaseUrl}/diaries?date={formattedDate}";
            var json = JsonConvert.SerializeObject(diaryDto);
            Debug.Log(json);
            var response = await ApiUtil.PerformApiCall(url, "PUT", token: UserSingleton.Instance.AccessToken, jsonData: json);

            Debug.Log(response);


            if (response == "Cannot connect to destination host") return false;
            return true;
        }

        public static async Task<bool> PostDiaryContent(PostDiaryContentRequestDto diaryDto)
        {
            var url = $"{ApiUtil.BaseUrl}/diaries";
            var json = JsonConvert.SerializeObject(diaryDto);
            Debug.Log(json);
            var response = await ApiUtil.PerformApiCall(url, "POST", token: UserSingleton.Instance.AccessToken, jsonData: json);
                

            Debug.Log(response);

            if (response == "Cannot connect to destination host") return false;
            return true;
        }
    }
}
