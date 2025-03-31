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
            List<DiaryReadDto> succes = await DiaryApiClient.DaysInDiary();

            if (succes != null && succes.Count > 0)
            {
                _diaryReads = succes;
            }
            else
            {
                string _content = "Welkom in je nieuwe dagboek! \n je kan hieronder opschrijven wat je \n -gedaan heb op de dag \n - nog wilt gaan doen op de dag \n -alle leuke en minder leuke ervaringen \n -vragen voor de doker \n \n en nog veel meer!";
                PostDiaryContentRequestDto requestDto = new PostDiaryContentRequestDto { content = _content, date = DateTime.Now.Date };
                await DiaryApiClient.PostDiaryContent(requestDto);

                _diaryReads = new List<DiaryReadDto> { new DiaryReadDto { date = DateTime.Now.Date, content = _content } };
            }
        }


        public List<DiaryReadDto> GetDiaryData() => _diaryReads;
        public void AddDiaryData(DiaryReadDto diaryReadDto) => _diaryReads.Add(diaryReadDto);
        public void UpdateDiaryData(DiaryReadDto diaryReadDto) => _diaryReads[_diaryReads.FindIndex(diary => diary.date == diaryReadDto.date)] = diaryReadDto;
    }
}
