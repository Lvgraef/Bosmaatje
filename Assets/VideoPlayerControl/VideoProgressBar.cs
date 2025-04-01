using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerManager : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Image progressBarImage;
    [SerializeField] private Sprite pauzeSprite;
    [SerializeField] private Sprite playSprite;
    [SerializeField] private Button playPauseButton;

    private bool hasReachedEnd = false;
    private bool isCurrentlySeeking = false;
    private bool wasPlayingBeforeDrag = false;

    private void Awake()
    {
        if (progressBarImage == null)
            progressBarImage = GetComponent<Image>();
    }

    private void Update()
    {
        UpdateProgressBar();
        CheckForVideoEnd();

        if (isCurrentlySeeking && Input.GetMouseButtonUp(0))
        {
            isCurrentlySeeking = false;
            if (wasPlayingBeforeDrag)
            {
                StartPlayback();
                UpdatePlayButton(pauzeSprite);
            }
        }
    }

    private void UpdateProgressBar()
    {
        if (videoPlayer.length > 0)
        {
            progressBarImage.fillAmount = (float)videoPlayer.time / (float)videoPlayer.length;
        }
    }

    private void CheckForVideoEnd()
    {
        if (videoPlayer.time >= videoPlayer.length - 0.1f && !hasReachedEnd)
        {
            PausePlayback();
            ResetPlayback();
            UpdatePlayButton(playSprite);
            hasReachedEnd = true;
        }
    }

    private void ResetPlayback()
    {
        videoPlayer.time = 0;
        audioSource.time = 0;
    }

    private void PausePlayback()
    {
        videoPlayer.Pause();
        audioSource.Pause();
    }

    private void StartPlayback()
    {
        videoPlayer.Play();
        audioSource.time = (float)videoPlayer.time;
        audioSource.Play();
    }

    private void UpdatePlayButton(Sprite sprite)
    {
        playPauseButton.image.sprite = sprite;
    }

    public void PlayPauzeOnButtonClick()
    {
        if (videoPlayer.isPlaying)
        {
            Debug.Log("Pause");
            PausePlayback();
            UpdatePlayButton(playSprite);
        }
        else
        {
            Debug.Log("Play");
            StartPlayback();
            UpdatePlayButton(pauzeSprite);
            hasReachedEnd = false;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        // Only process drag if we're in seeking mode
        if (isCurrentlySeeking)
        {
            PausePlayback();
            float seekPercentage = HandleSeekInput(eventData);
            SeekToTime(seekPercentage);
            hasReachedEnd = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsClickOnProgressBar(eventData))
        {
            isCurrentlySeeking = true;
            wasPlayingBeforeDrag = videoPlayer.isPlaying;
            PausePlayback();

            float seekPercentage = HandleSeekInput(eventData);
            SeekToTime(seekPercentage);
            hasReachedEnd = false;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isCurrentlySeeking)
        {
            isCurrentlySeeking = false;

            if (wasPlayingBeforeDrag)
            {
                StartPlayback();
                UpdatePlayButton(pauzeSprite);
            }
        }
    }

    private float HandleSeekInput(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            progressBarImage.rectTransform, eventData.position, null, out localPoint))
        {
            return Mathf.InverseLerp(progressBarImage.rectTransform.rect.xMin, progressBarImage.rectTransform.rect.xMax, localPoint.x);
        }
        return 0;
    }

    private bool IsClickOnProgressBar(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            progressBarImage.rectTransform, eventData.position, null, out localPoint))
        {
            return progressBarImage.rectTransform.rect.Contains(localPoint);
        }
        return false;
    }

    private void SeekToTime(float seekPercentage)
    {
        float targetTime = (float)(videoPlayer.length * seekPercentage);
        videoPlayer.time = targetTime;

        if (audioSource.clip != null)
            audioSource.time = targetTime;
    }
}