using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SFB;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using File = UnityEngine.Windows.File;
using Image = UnityEngine.UI.Image;

//using UnityEngine;


namespace Diary
{
    public class ImageMode : DiaryMode
    {
        private List<Texture2D> _images = new();
        
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
            Debug.Log(path);
            File.WriteAllBytes(path, png);
        }

        #endregion

        private void ReloadImages(DateTime date)
        {
            string path = Application.persistentDataPath + "/images/" + date.ToString("dd-MM-yyyy");
            var info = new DirectoryInfo(path);
            var files = info.GetFiles();
            foreach (var fileInfo in files)
            {
                var filePath = fileInfo.DirectoryName;
                var bytes = File.ReadAllBytes(filePath);
                var tex = new Texture2D(2, 2);
                tex.LoadImage(bytes); //..this will auto-resize the texture dimensions.
                _images.Add(tex);
            }
            _diaryWriter.scroll.SetActive(true);
            foreach (var texture2D in _images)
            {
                var gameObject = new GameObject();
                var image = gameObject.AddComponent<RawImage>();
                image.texture = texture2D;
            }
        }

        public override void HandleSaveUpdater()
        {
            var date = _diaryWriter.GetDiaryDate();
            OnClick(date);
            ReloadImages(date);
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