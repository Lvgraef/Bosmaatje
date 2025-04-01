using System;
using System.Collections;
using System.Collections.Generic;
using SFB;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

//using UnityEngine;


namespace Diary
{
    public class ImageMode : DiaryMode
    {
        public ImageMode(DiaryWriterManager diaryWriter) : base(diaryWriter)
        {
        }

        public override void Setup()
        {
            _diaryWriter.GetContentFieldText().gameObject.SetActive(false);
            _diaryWriter.buttonSave.gameObject.SetActive(true);
            _diaryWriter.ClearText.gameObject.SetActive(false);
            _diaryWriter.BackgroundImages.gameObject.SetActive(true);

            _diaryWriter.buttonSave.GetComponentInChildren<TMP_Text>().text = "Upload";
            _diaryWriter.buttonButtomMiddleSwitchMode.GetComponentInChildren<TMP_Text>().text = "dagboek";
        }

        #region Image upload

        private void OnClick(DateTime date)
        {
            var paths = StandaloneFileBrowser.OpenFilePanel("Upload images", "", "png", true);
            foreach (var path in paths)
            {
                _diaryWriter.StartCoroutine(OutputRoutine(new Uri(path).AbsoluteUri, date));
            }
        }

        private IEnumerator OutputRoutine(string url, DateTime date)
        {
            var loader = new WWW(url);
            yield return loader;
            byte[] png = loader.texture.EncodeToPNG();
            string path = Application.persistentDataPath + "/images/" + date.ToString("dd-MM-yyyy") + Guid.NewGuid() + ".png";
            File.WriteAllBytes(path, png);
        }

        #endregion

        private void ReloadImages()
        {
            throw new NotImplementedException();
        }

        public override void HandleSaveUpdater()
        {
            OnClick(_diaryWriter.GetDiaryDate());
        }

        public override void HandleGoBack()
        {
            _diaryWriter.GetConfirmPopupGoBack()
                .Invoke(); // je zou hier optimaal willen hebebn dat je naar een ander mode gaat als je daar net was, dat is nu nog niet het geval.
        }

        public override void HandleClose()
        {
            _diaryWriter.GetConfirmPopupClose().Invoke();
        }

        public override void HandleButtomMiddleSwitchMode() //
        {
            _diaryWriter.SwitchMode(new EditMode(_diaryWriter));
        }
    }
}