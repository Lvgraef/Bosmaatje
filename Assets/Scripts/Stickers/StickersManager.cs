using System;
using Mono.Cecil;
using Treatment;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Stickers
{
    public class StickersManager : MonoBehaviour
    {
        public TreatmentManager Treatment { get; set; }

        public RectTransform content;
        public GameObject stickerPrefab;
    
        private static bool isInitialized = false;

        private void Start()
        { 
            for (int i = 1; i <= CountStickerAssets(); i++)
            {
                GameObject instStickerObj = Instantiate(stickerPrefab, content);
                Button instSticker = instStickerObj.GetComponent<Button>();

                var finalI = i;
                instSticker.onClick.AddListener(() => StickerOnButtonClick(instSticker, finalI));


                instSticker.image.sprite = Resources.Load<Sprite>("Stickers/sticker-" + i);
            }
        }

        public void Close()
        {
            Destroy(gameObject);
        }
        
        public async void StickerOnButtonClick(Button button, int stickerid) // button is overbodig?
        {
            await Treatment.PutSticker(stickerid);
            Destroy(gameObject);
        }


        private static int CountStickerAssets()
        {
            var stickers = Resources.LoadAll<Sprite>("Stickers");
            int assetCount = stickers.Length;
            Debug.Log("assetCount: " + assetCount);
            return assetCount;
        }

    }
}

