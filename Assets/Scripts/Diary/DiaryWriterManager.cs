using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Diary;
using ApiClient;
using Dto;
using Util;
using Global;

public class DiaryWriterManager : MonoBehaviour
{
    // UI references
    public GameObject writeDiaryPopup;
    public GameObject pickDiaryPopup;
    public TMP_Text textDate;
    public TMP_InputField textContentField;
    public Button ClearText;

    public Button buttonSave;
    public Button buttonSwitchToEditMode;

    // Diary data
    private static DateTime diaryDate;
    private static bool isPreviewByDefault = true;
    private static bool isExistend;
    private static string diaryContent;

    // Mode for handling diary operations
    private DiaryMode currentMode;

    // Public Methods
    public void OpenDiary(DateTime date, bool previewByDefault, bool existend)
    {
        diaryDate = date;
        isPreviewByDefault = previewByDefault;
        isExistend = existend;

        currentMode = isPreviewByDefault ? new PreviewMode(this) : new EditMode(this);
        Debug.Log("we zijn alles aan het inladen");
        SetupDiary();
    }

    public void SaveOnButtonClick()
    {
        currentMode.HandleSave();
    }

    public void GoBackOnButtonClick()
    {
        currentMode.HandleGoBack();
    }

    public void CloseOnButtonClick()
    {
        currentMode.HandleClose();
    }

    public void ClearTextButtonClick()
    {
        if (textContentField.text == string.Empty)
        {
            return;
        }

        Action nothing = () => { };
        Confirmations.CreateConfirmationPopup(EmptyContentField, nothing, "Weet je zeker dat je de tekst wilt legen?", writeDiaryPopup.GetComponent<CanvasGroup>(), "Legen", "Sparen");
    }

    public void SwitchToEditModeOnButtonClick()
    {
        currentMode = new EditMode(this);
        currentMode.Setup();
    }

    // Private Methods
    private void SetupDiary()
    {
        textDate.text = diaryDate.ToString("dd/MM/yyyy");

        if (isExistend)
        {
            LoadExistingDiaryContent();
        }
        else
        {
            EmptyContentField();
        }
       

        Debug.Log(currentMode.ToString());
        currentMode.Setup();
    }

    private void LoadExistingDiaryContent()
    {
        //string content = DiarySingleton.Instance.GetDiaryData().Find(item => DateTime.ParseExact(item.date, "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) == diaryDate).content;
        string content = DiarySingleton.Instance.GetDiaryData().Find(item => item.date == diaryDate).content;

        diaryContent = content;
        textContentField.text = content;
    }

    private void ConfirmPopupGoBack()
    {
        writeDiaryPopup.SetActive(false);
        pickDiaryPopup.GetComponent<CanvasGroup>().interactable = true;
        pickDiaryPopup.SetActive(true);
        EmptyDiaryWriterManagerVariables();
    }

    private void ConfirmPopupClose()
    {
        writeDiaryPopup.SetActive(false);
        EmptyDiaryWriterManagerVariables();
    }

    private void EmptyDiaryWriterManagerVariables()
    {
        diaryDate = default;
        isPreviewByDefault = true;
        isExistend = false;
        diaryContent = string.Empty;
        textContentField.text = diaryContent;
    }

    private void EmptyContentField()
    {
        textContentField.text = string.Empty;
    }

    // Action Methods (For use by other classes)
    public Action GetConfirmPopupGoBack() => ConfirmPopupGoBack;
    public Action GetConfirmPopupClose() => ConfirmPopupClose;
    public Action GetSaveAllFirstBeforeClose() => SaveAllFirstBeforeClose;
    public Action GetSaveAllFirstBeforeGoBack() => SaveAllFirstBeforeGoBack;

    // Handling methods
    private void SaveAllFirstBeforeClose()
    {
        SaveOnButtonClick();
        ConfirmPopupClose();
    }

    private void SaveAllFirstBeforeGoBack()
    {
        SaveOnButtonClick();
        ConfirmPopupGoBack();
    }

    // Getters and Setters
    public bool GetIsExistend() => isExistend;
    public void SetIsExistend(bool existend) => isExistend = existend;

    public DateTime GetDiaryDate() => diaryDate;
    public string GetDiaryContent() => diaryContent;
    public string SetDiaryContent(string content) => diaryContent = content;

    public TMP_InputField GetContentFieldText() => textContentField;
}
