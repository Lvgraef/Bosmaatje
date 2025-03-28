using UnityEngine;
using Dto;
using ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using System.Xml.Linq;
using Unity.Burst.Intrinsics;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using UnityEngine.InputSystem;
using Global;
using UnityEngine.U2D;

public class DiaryPickerManager : MonoBehaviour
{
    public TMP_Text Weektext;

    public Button NextButton;
    public Button PreviousButton;

    public GameObject DiaryWriter;

    public Sprite spriteDiaryPencilEdit;
    //public Sprite spriteDiaryWrite;
    public Sprite spriteDiaryEyeView;
    //public Sprite spriteDiaryEmpty;

    public GameObject Bar1;
    public GameObject Bar2;
    public GameObject Bar3;
    public GameObject Bar4;
    public GameObject Bar5;
    public GameObject Bar6;
    public GameObject Bar7;

    private GameObject[] _bars = new GameObject[7];

    private static DateTime[] _filledDates;
    private static int _sumOfDates;
    private int _weekNum = 0;



    void Start()
    {

    }

    private void OnEnable()
    {
        InitializeDiaryPicker();
    }


    public void Fill7DiaryDays(int weekNum)
    {
        if (_sumOfDates == 0)
        {
            return;
        }

        Weektext.text = $"Week {_weekNum + 1}";

        DateTime firstDate = _filledDates[0];

        DateTime firstSunday = firstDate.AddDays(-(int)firstDate.DayOfWeek); // Vind de eerste zondag

        DateTime CurrentDate = firstSunday.AddDays(weekNum * 7);

        //DateTime CurrentDate = firstDate.AddDays(weekNum * 7);


        foreach (var bar in _bars)
        {
            bool exists = Array.Exists(_filledDates, date => date == CurrentDate);
            //bool isOldDate = CurrentDate >= DateTime.Now.AddDays(-2);
            bool isPreviewByDefault = CurrentDate < DateTime.Now.AddDays(-1) || CurrentDate > DateTime.Now.AddDays(1);
            Debug.Log($"Vullen week {_weekNum} met datum {CurrentDate}");


            FillBar(bar, CurrentDate, exists, isPreviewByDefault);
            CurrentDate = CurrentDate.AddDays(1);


        }
    }

    public void NextWeekOnButonClick()
    {
        _weekNum++;

        PreviousButton.interactable = true;
        Fill7DiaryDays(_weekNum);
    }

    public void PreviousWeekOnButtonClick()
    {
        _weekNum--;

        if (_weekNum == 0)
        {
            PreviousButton.interactable = false;
            Fill7DiaryDays(_weekNum);
            return;
        }
        else
        {
            Fill7DiaryDays(_weekNum);
        }
    }

    public void OpenOnButtonClick()
    {
        this.gameObject.SetActive(true);
        this.gameObject.GetComponent<CanvasGroup>().interactable = true;

        //if (_bars == null || _bars.Length == 0)
        //{
        //    _bars = new GameObject[] { Bar1, Bar2, Bar3, Bar4, Bar5, Bar6, Bar7 };
        //    Debug.Log(" _bars is nu handmatig geïnitialiseerd in OpenOnButtonClick().");
        //}

        InitializeDiaryPicker();
    }

    public void CloseOnButtonClick()
    {
        this.gameObject.SetActive(false);
    }

    private void FillBar(GameObject bar, DateTime date, bool isExistend, bool isPreviewByDefault)
    {
        Button openButton = bar.GetComponentInChildren<Button>();
        Image openButtonImage = openButton.transform.GetChild(0).GetComponent<Image>(); // Aannemende dat de Image de eerste child is
        TMP_Text dateText = bar.GetComponentInChildren<TMP_Text>();

        string DayOfWeekText = GetAbreviationFromDayOfTheWeek(date);
        dateText.text = $"{date.ToString("dd/MM/yyyy")} ({DayOfWeekText})";

        bool isAfterLastFuture = date > DateTime.Now.AddDays(1);
        bool isBeforeFirstDate = date < _filledDates[0];
        openButton.interactable = !(isAfterLastFuture || isBeforeFirstDate);

        openButtonImage.sprite = GetSprite(isExistend, isPreviewByDefault);

        openButton.image.color = Color.green;
        openButton.onClick.RemoveAllListeners();
        openButton.onClick.AddListener(() => OpenDiary(date, isPreviewByDefault, isExistend));
    }

    private Sprite GetSprite(bool isExistend, bool isPreviewByDefault)
        => (isExistend && isPreviewByDefault) ? spriteDiaryEyeView : spriteDiaryPencilEdit;


    private void InitializeDiaryPicker()
    {
        List<DiaryReadDto> result = DiarySingleton.Instance.GetDiaryData();
        if (result == null || result.Count == 0)
        {
            return;
        }

        _bars[0] = Bar1;
        _bars[1] = Bar2;
        _bars[2] = Bar3;
        _bars[3] = Bar4;
        _bars[4] = Bar5;
        _bars[5] = Bar6;
        _bars[6] = Bar7;

        _filledDates = result.Select(diary => diary.date).ToArray();

        _filledDates = _filledDates.OrderBy(date => date).ToArray();
        //

        // fot testing only
        //_filledDates = new DateTime[]
        //    {
        //        new DateTime(2025, 01, 03),
        //        new DateTime(2025, 01, 07),
        //        new DateTime(2025, 01, 10),
        //        new DateTime(2025, 01, 15),
        //        new DateTime(2025, 01, 18),   
        //        new DateTime(2025, 01, 22),
        //        new DateTime(2025, 01, 25),
        //        new DateTime(2025, 01, 29),
        //        new DateTime(2025, 02, 02),
        //        new DateTime(2025, 02, 06),
        //        new DateTime(2025, 02, 09),
        //        new DateTime(2025, 02, 13),
        //        new DateTime(2025, 02, 16),
        //        new DateTime(2025, 02, 19),
        //        new DateTime(2025, 02, 22),
        //        new DateTime(2025, 02, 25),
        //        new DateTime(2025, 03, 01),
        //        new DateTime(2025, 03, 05),
        //        new DateTime(2025, 03, 08),
        //        new DateTime(2025, 03, 11),
        //        new DateTime(2025, 03, 14),
        //        new DateTime(2025, 03, 18),
        //        new DateTime(2025, 03, 21),
        //        new DateTime(2025, 03, 24),
        //    };


        DateTime firstDate = _filledDates[0];
        DateTime lastDate = _filledDates[^1];


        _sumOfDates = (lastDate - firstDate).Days + 1;

        DateTime firstSunday = firstDate.AddDays(-(int)firstDate.DayOfWeek);
        _weekNum = (DateTime.Now - firstSunday).Days / 7;


        //Debug.Log($"Aantal data items: {result?.Count ?? 0}");
        Debug.Log($"_filledDates lengte: {_filledDates?.Length ?? 0}");


        Fill7DiaryDays(_weekNum);
        Debug.Log($"WeekNum bij openen: {_weekNum}");

    }


    private void OpenDiary(DateTime date, bool isPreviewByDefault, bool isExistend)
    {
        Debug.Log("we openen de WriterDiary");
        this.gameObject.GetComponent<CanvasGroup>().interactable = false;
        this.gameObject.SetActive(false);
        DiaryWriter.SetActive(true);

        isPreviewByDefault = false;// omdat we deze functionaliteit niet meer willen hebben.

        DiaryWriter.GetComponent<DiaryWriterManager>().OpenDiary(date, isPreviewByDefault, isExistend);
    }

    private string GetAbreviationFromDayOfTheWeek(DateTime date)
    {
        int dayOfWeek = (int)date.DayOfWeek;

        switch (dayOfWeek)
        {
            case 0:
                return "Zo";
            case 1:
                return "Ma";
            case 2:
                return "Di";
            case 3:
                return "Wo";
            case 4:
                return "Do";
            case 5:
                return "Vr";
            case 6:
                return "Za";
            default:
                return "";
        }
    }

    //public void GetInitializeDiaryPicker() => InitializeDiaryPicker();
}
