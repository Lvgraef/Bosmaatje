using Assets.Scripts.Dto;
using Global;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.Dto.StickersDto;

public class StickersSingleton : MonoBehaviour
{

    public static StickersSingleton Instance;

    public RectTransform content;
    public GameObject stickerPrefab;

    private StickersDto _stickersDto = new StickersDto();
    private int number = 0;

    private StickersManager StickersManager = new StickersManager();

    private void Awake()
    {
        // Destroy this object if we already have a singleton configured
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        StickerDto[] stickers = GetStickersDto().stickers;

        foreach (StickerDto sticker in stickers)
        {
            number++;
            GameObject instStickerObj = Instantiate(stickerPrefab, content);
            Button instSticker = instStickerObj.GetComponent<Button>();

            instSticker.onClick.AddListener(() => StickersManager.StickerOnButtonClick(instStickerObj, sticker.stickerId, number));

            if (sticker.isAlreadyUnpacked)
            {
                instSticker.image.sprite = Resources.Load<Sprite>("Stickers/sticker-" + sticker.stickerId);
            }
            else
            {
                if (sticker.Date < System.DateTime.Now)
                {
                    instSticker.image.sprite = Resources.Load<Sprite>("Stickers/sticker-" + sticker.stickerId);

                    instSticker.image.color = new Color(1f, 1f, 0f, 1f); // geel
                }
                else
                {
                    instSticker.image.sprite = Resources.Load<Sprite>("Stickers/sticker-" + sticker.stickerId);
                    instSticker.image.color = new Color(0.5f, 0.5f, 0.5f, 1f); // grijs
                }
            }
        }
    }


    public void SetStickersDto(StickersDto stickersDto)
    {
        _stickersDto = stickersDto;
    }
    public StickersDto GetStickersDto()
    {
        if (_stickersDto == null)
        {
            _stickersDto = new StickersDto();
        }
        return _stickersDto;
    }

}
