using System;
using Assets.Scripts.Dto;
using Unity.VisualScripting;
using UnityEngine;
using static Assets.Scripts.Dto.StickersDto;

public class StickersManager : MonoBehaviour
{

    //private GameObject _button = null;
    //private int _stickerNumber = 0;
    StickersSingleton StickersSing = StickersSingleton.Instance;

    private static bool isInitialized = false;

    private void Start()
    {
        if (isInitialized)
        {
            return;
        }

        isInitialized = true;
    }


    public void StickerOnButtonClick(GameObject button, int stickerid, int gridnumber)
    {
        if (button != null || stickerid == 0)
        {
            return;
        }

        StickerDto sticker = StickersSing.GetStickersDto().stickers[gridnumber];

        if (sticker.isAlreadyUnpacked)
        {
            return;
            // animatie of een geluidje
        }
        else if (sticker.Date < DateTime.Now)
        {
            sticker.isAlreadyUnpacked = true;
            // animatie of een geluidje
        }
        else
        {
            // animatie of een geluidje, dat het nog niet unlocked is

        }

    }


}
