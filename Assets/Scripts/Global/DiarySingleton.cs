using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiClient;
using Dto;
using UnityEngine;

namespace Global
{
    public class DiarySingleton: MonoBehaviour
    {
        public static DiarySingleton Instance { get; private set; }

        private static List<DiaryReadDto> _diaryReads;

        private static DateTime[] _diaryDates;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);
        }

        private async void Start()
        {
            GetDiaryDataRequestDto succes = await DiaryApiClient.DaysInDiary();

            if (succes != null)
            {
                _diaryReads = succes.Diaries;
            }
            else if (_diaryReads == null)
            {
                PostDiaryContentRequestDto requestDto = new PostDiaryContentRequestDto { content = "Welcome in je nieuwe dagboek!", date = DateTime.Now.Date };
                await DiaryApiClient.PostDiaryContent(requestDto);
            }
            if (_diaryReads == null)
            {
                _diaryReads = new List<DiaryReadDto>();
            }
        }

        public List<DiaryReadDto> GetDiaryData() => _diaryReads;
        public void AddDiaryData(DiaryReadDto diaryReadDto) => _diaryReads.Add(diaryReadDto);
        public void UpdateDiaryData(DiaryReadDto diaryReadDto) => _diaryReads[_diaryReads.FindIndex(x => x.date == diaryReadDto.date)] = diaryReadDto;
    }
}
