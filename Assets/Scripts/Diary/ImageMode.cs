using System;
using System.Collections.Generic;
using System.IO;
using Global;
using SFB;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//using UnityEngine;


namespace Diary
{
    public class ImageMode : DiaryMode
    {

        public void Init(DiaryWriterManager diaryWriter)
        {
            _diaryWriter = diaryWriter;
        }
        
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
            ReloadImages(_diaryWriter.GetDiaryDate());
        }

        #region Image upload

        private void OnClick(DateTime date)
        {
            var paths = StandaloneFileBrowser.OpenFilePanel("Upload images", "", "png", true);
            foreach (var path in paths)
            {
                OutputRoutine(new Uri(path).AbsoluteUri, date);
            }
        }

        private void OutputRoutine(string url, DateTime date)
        {
            var loader = new WWW(url);
            byte[] png = loader.texture.EncodeToPNG();
            var user = Application.persistentDataPath + "/images/" + UserSingleton.Instance.Name;
            var directory = user + "/" + date.ToString("dd-MM-yyyy");
            string path = directory + "/" + Guid.NewGuid() + ".png";
            
            if (!Directory.Exists(Application.persistentDataPath + "/images/")) {
                Directory.CreateDirectory(Application.persistentDataPath + "/images/");
            }

            if (!Directory.Exists(user))
            {
                Directory.CreateDirectory(user);
            }
            
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            Debug.Log(path);
            File.WriteAllBytes(path, png);
        }

        #endregion

        private void ReloadImages(DateTime date)
        {
            foreach (var componentsInChild in _diaryWriter.images.GetComponentsInChildren<RawImage>())
            {
                Destroy(componentsInChild.gameObject);
            }
            string path = Application.persistentDataPath + "/images/" + UserSingleton.Instance.Name + "/" + date.ToString("dd-MM-yyyy");
            if (!Directory.Exists(path)) return;
            var info = new DirectoryInfo(path);
            var files = info.GetFiles();
            foreach (var fileInfo in files)
            {
                var filePath = fileInfo.FullName;
                var bytes = File.ReadAllBytes(filePath);
                var tex = new Texture2D(2, 2);
                tex.LoadImage(bytes); //..this will auto-resize the texture dimensions.
                var gameObject = new GameObject();
                var image = gameObject.AddComponent<RawImage>();
                image.texture = tex;
                image.transform.SetParent(_diaryWriter.images.transform);
                //todo set correct size
                image.rectTransform.sizeDelta = new Vector2(800, 800);
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
            foreach (var componentsInChild in _diaryWriter.images.GetComponentsInChildren<RawImage>())
            {
                Destroy(componentsInChild.gameObject);
            }
        }

        public override void HandleClose()
        {
            foreach (var componentsInChild in _diaryWriter.images.GetComponentsInChildren<RawImage>())
            {
                Destroy(componentsInChild.gameObject);
            }
            _diaryWriter.GetConfirmPopupClose().Invoke();
        }

        public override void HandleButtomMiddleSwitchMode() //
        {
            _diaryWriter.SwitchMode(new EditMode(_diaryWriter));
        }
    }
}