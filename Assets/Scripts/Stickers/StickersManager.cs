using System;
using Assets.Scripts.Dto;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
//using static Assets.Scripts.Dto.StickersDto;

public class StickersManager : MonoBehaviour
{

    public GameObject panel;

    public RectTransform content;
    public GameObject stickerPrefab;

    //StickersSingleton StickersSing = StickersSingleton.Instance;

    private static bool isInitialized = false;

    private void Start()
    {
        if (isInitialized)
        {
            return;
        }

        isInitialized = true;


        for (int i = 1; i <= CountStickerAssets(); i++)
        {
            GameObject instStickerObj = Instantiate(stickerPrefab, content);
            Button instSticker = instStickerObj.GetComponent<Button>();

            instSticker.onClick.AddListener(() => StickerOnButtonClick(instSticker, i));


            instSticker.image.sprite = Resources.Load<Sprite>("Stickers/sticker-" + i);
        }
    }




    public void StickerOnButtonClick(Button button, int stickerid) // button is overbodig?
    {
        if (button != null || stickerid == 0)
        {
            Debug.LogWarning("StickerOnButtonClick: button, stickerid or gridnumber is null");
            return;
        }

        //stuur door wat het id is, en haal het op met "Resources.Load<Sprite>("Stickers/sticker-" + stickerid);"
    }


    private static int CountStickerAssets()
    {
        string[] guids = AssetDatabase.FindAssets("t:sprite", new[] { "Assets/Resources/Stickers" });
        int assetCount = guids.Length;
        Debug.Log("assetCount: " + assetCount);
        return assetCount;
    }

}

