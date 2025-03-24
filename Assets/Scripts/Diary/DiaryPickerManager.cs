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

public class DiaryPickerManager : MonoBehaviour
{
    public TMP_Text Weektext;

    public Button NextButton;
    public Button PreviousButton;

    public GameObject DiaryWriter;

    public Sprite spriteDiaryPencilEdit;
    public Sprite spriteDiaryWrite;
    public Sprite spriteDiaryEyeView;
    public Sprite spriteDiaryEmpty;

    public GameObject Bar1;
    public GameObject Bar2;
    public GameObject Bar3;
    public GameObject Bar4;
    public GameObject Bar5;
    public GameObject Bar6;
    public GameObject Bar7;

    private static DateTime[] _filledDates;
    private static int _sumOfDates;

    private GameObject[] _bars;
    private int _weekNum = 0;



    async void Start()
    {
        _bars = new GameObject[7] { Bar1, Bar2, Bar3, Bar4, Bar5, Bar6, Bar7 };

        //var result = await DiaryApiClient.DaysInDiary();
        //if (result == null || result.date.Length == 0)
        //{
        //    return;
        //}



        _filledDates = new DateTime[]
        {
            // December 2024
            new DateTime(2024, 12, 5),
            new DateTime(2024, 12, 12),
            new DateTime(2024, 12, 19),
            new DateTime(2024, 12, 25),

            // Januari 2025
            new DateTime(2025, 1, 3),
            new DateTime(2025, 1, 9),
            new DateTime(2025, 1, 15),
            new DateTime(2025, 1, 22),
            new DateTime(2025, 1, 28),

            // Februari 2025
            new DateTime(2025, 2, 2),
            new DateTime(2025, 2, 10),
            new DateTime(2025, 2, 18),
            new DateTime(2025, 2, 25),

            // Maart 2025 (tot gisteren)
            new DateTime(2025, 3, 5),
            new DateTime(2025, 3, 12),
            new DateTime(2025, 3, 18),
            new DateTime(2025, 3, 23),
            new DateTime(2025, 3, 24)
};

        //_filledDates = result.date.OrderBy(date => date).ToArray();

        _filledDates = _filledDates.OrderBy(date => date).ToArray();

        DateTime firstDate = _filledDates[0];
        DateTime lastDate = _filledDates[^1];


        _sumOfDates = (lastDate - firstDate).Days + 1;

        DateTime firstSunday = firstDate.AddDays(-(int)firstDate.DayOfWeek);
        _weekNum = (DateTime.Now - firstSunday).Days / 7;

        Fill7DiaryDays(_weekNum);
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

        bool isFuture = date > DateTime.Now.AddDays(1);
        openButton.interactable = !isFuture;

        if (isExistend)
        {
            if (isPreviewByDefault)
            {
                openButtonImage.sprite = spriteDiaryEyeView;
                openButton.image.color = Color.green;
                openButton.onClick.RemoveAllListeners();
                openButton.onClick.AddListener(() =>
                {
                    OpenDiary(date, isPreviewByDefault, isExistend); // Open de diary en je kan het bekijken en is al bestaand
                });
            }
            else
            {
                openButtonImage.sprite = spriteDiaryPencilEdit;
                openButton.image.color = Color.green;
                openButton.onClick.RemoveAllListeners();
                openButton.onClick.AddListener(() =>
                {
                    OpenDiary(date, isPreviewByDefault, isExistend); // Open de diary en je kan het bewerken en is al bestaand
                });
            }
        }
        else
        {
            if(isPreviewByDefault)
            {
                openButtonImage.sprite = spriteDiaryEmpty;
                openButton.image.color = Color.green;
                openButton.onClick.AddListener(() =>
                {
                    OpenDiary(date, isPreviewByDefault, isExistend); // Open de diary en je kan het bekijken en is nog niet bestaand
                });
            }
            else
            {
                openButtonImage.sprite = spriteDiaryWrite;
                openButton.image.color = Color.green;
                openButton.onClick.AddListener(() =>
                {
                    OpenDiary(date, isPreviewByDefault, isExistend); // Open de diary en je kan het bewerken en is nog niet bestaand
                });
        }
        }

        // Als de datum in de toekomst ligt, kan je er niet op klikken
        DateTime firstDate = _filledDates[0];
        if (date < firstDate)
        {
            openButton.interactable = false;
        }
    }


    private void OpenDiary(DateTime date, bool isPreviewByDefault, bool isExistend)
    {
        Debug.Log("we openen de WriterDiary");
        this.gameObject.GetComponent<CanvasGroup>().interactable = false;
        this.gameObject.SetActive(false);
        DiaryWriter.SetActive(true);

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
}
