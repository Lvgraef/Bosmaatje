using System;
using TMPro;

//using UnityEngine;



namespace Diary
{
    public class ImageMode : DiaryMode
    {
        public ImageMode(DiaryWriterManager diaryWriter) : base(diaryWriter) { }

        public override void Setup()
        {
            _diaryWriter.GetContentFieldText().gameObject.SetActive(false);
            _diaryWriter.buttonSave.gameObject.SetActive(true);
            _diaryWriter.ClearText.gameObject.SetActive(false);
            _diaryWriter.BackgroundImages.gameObject.SetActive(true);

            _diaryWriter.buttonSave.GetComponentInChildren<TMP_Text>().text = "Upload";
            _diaryWriter.buttonButtomMiddleSwitchMode.GetComponentInChildren<TMP_Text>().text = "dagboek";
        }

        public override void HandleSaveUpdater()
        {
            // hier doen we de image upload
            throw new NotImplementedException();
        }

        public override void HandleGoBack()
        {
            _diaryWriter.GetConfirmPopupGoBack().Invoke();// je zou hier optimaal willen hebebn dat je naar een ander mode gaat als je daar net was, dat is nu nog niet het geval.
        }

        public override void HandleClose()
        {
            _diaryWriter.GetConfirmPopupClose().Invoke();
        }

        public override void HandleTopBarSwitchMode()
        {
            _diaryWriter.SwitchMode(new EditMode(_diaryWriter));
        }

        public override void HandleButtomMiddleSwitchMode()//
        {
            _diaryWriter.SwitchMode(new EditMode(_diaryWriter));
        }
    }

}
