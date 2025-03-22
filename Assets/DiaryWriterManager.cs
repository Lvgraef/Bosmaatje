using System;
using System.Xml.Serialization;
using ApiClient;
using Dto;
using TMPro;

using UnityEngine;
using UnityEngine.UI;
using Util;


public class DiaryWriterManager : MonoBehaviour
{
    public GameObject writeDiaryPopup;
    public GameObject pickDiaryPopup;
    public TMP_Text textDate;
    public TMP_InputField TextContent;
    public GameObject ConfirmPopup;

    public Button buttonSave;

    private static DateTime _diaryDate;
    private static bool _isEditable;
    private static bool _isExistend;
    private static string _diaryContent;

    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OpenDiary(DateTime date, bool isEditable, bool isExistend)
    {
        _diaryDate = date;
        _isEditable = isEditable;
        _isExistend = isExistend;

        setupDiary();
    }

    public async void SaveOnButtonClick()
    {
        _diaryContent = TextContent.text;

        if (_isExistend)
        {
            PutSpecificDiaryContentRequestDto putContentDto = new PutSpecificDiaryContentRequestDto { date = _diaryDate, content = _diaryContent };
            bool response = await DiaryApiClient.PutSpecificDiaryContent(putContentDto);
        }
        else
        {
            PostSpecificDiaryContentRequestDto postContentDto = new PostSpecificDiaryContentRequestDto { date = _diaryDate, content = _diaryContent };
            bool response = await DiaryApiClient.PostSpecificDiaryContent(postContentDto);
        }
    }

    public void GoBackOnButtonClick()
    {
        if (_diaryContent != TextContent.text)
        {
            Confirmations.CreateConfirmationPopup(DiscardAll, SaveAllFist, "nog niet alles is opgeslagen, wil je het nog opslaan?", writeDiaryPopup.GetComponent<CanvasGroup>());


            ConfirmPopup.gameObject.SetActive(true);
        }
        else
        {
            writeDiaryPopup.SetActive(false);
            pickDiaryPopup.GetComponent<CanvasGroup>().interactable = true;
        }


        

    }

    private void SaveAllFist()
    {
        SaveOnButtonClick();
        writeDiaryPopup.SetActive(false);
        pickDiaryPopup.GetComponent<CanvasGroup>().interactable = true;
        pickDiaryPopup.SetActive(true);
    }

    private void DiscardAll()
    {
        writeDiaryPopup.SetActive(false);
        pickDiaryPopup.GetComponent<CanvasGroup>().interactable = true;
        pickDiaryPopup.SetActive(true);
    }

    private async void setupDiary()
    {
        textDate.text = _diaryDate.ToString("dd/MM/yyyy");
        Debug.Log("de date is: " + _diaryDate);
        if (_isExistend)
        {
            GetSpecificDiaryContentRequestDto getContentDto = new GetSpecificDiaryContentRequestDto { date = _diaryDate };

            // GetSpecificDiaryContentResponseDto respons = await DiaryApiClient.GetSpecificDiaryContent(getContentDto);

            GetSpecificDiaryContentResponseDto respons = new GetSpecificDiaryContentResponseDto { content = "En dit is de algeschreven testContent" };
            _diaryContent = respons.content;

            TextContent.text = _diaryContent;
        }

        buttonSave.interactable = _isEditable;


        writeDiaryPopup.SetActive(true);
    }
}
