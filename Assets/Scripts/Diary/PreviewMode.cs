using System;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace Diary
{
    public class PreviewMode : DiaryMode
    {
        public PreviewMode(DiaryWriterManager diaryWriter) : base(diaryWriter) { }

        public override void Setup()
        {
            _diaryWriter.GetContentFieldText().gameObject.SetActive(true);
            _diaryWriter.GetContentFieldText().interactable = false;
            
            _diaryWriter.buttonSave.gameObject.SetActive(false);
            _diaryWriter.ClearText.gameObject.SetActive(false);
            _diaryWriter.BackgroundImages.gameObject.SetActive(false);

            _diaryWriter.buttonSave.GetComponentInChildren<TMP_Text>().text = "During PreviewMode am I invissible";
            _diaryWriter.buttonButtomMiddleSwitchMode.GetComponentInChildren<TMP_Text>().text = "afbeeldingen";
            _diaryWriter.buttonTopBarSwitchMode.image.sprite = _diaryWriter.EditTextSprite;

            Debug.LogWarning("previewmode should not have been openend");// deze mode is niet meer gewenst

        }

        public override void HandleSaveUpdater()
        {
            Debug.LogWarning("Preview mode, cannot save, save button should have been invissible");// save knop zou niet zichtbaar moeten zijn
        }

        public override void HandleGoBack()
        {
            _diaryWriter.GetConfirmPopupGoBack().Invoke();
            Debug.LogWarning("previewmode should not have been openend");// deze mode is niet meer gewenst
        }

        public override void HandleClose()
        {
            _diaryWriter.GetConfirmPopupClose().Invoke();
            Debug.LogWarning("previewmode should not have been openend");// deze mode is niet meer gewenst
        }

        public override void HandleTopBarSwitchMode()
        {
            _diaryWriter.SwitchMode(new EditMode(_diaryWriter));
            Debug.LogWarning("previewmode should not have been openend");// deze mode is niet meer gewenst
        }

        public override void HandleButtomMiddleSwitchMode()
        {
            _diaryWriter.SwitchMode(new ImageMode(_diaryWriter));
            Debug.LogWarning("previewmode should not have been openend");// deze mode is niet meer gewenst
        }
    }

}
