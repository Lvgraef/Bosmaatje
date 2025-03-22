using TMPro;
using UnityEngine;
using UnityEngine.Video;

namespace Treatment
{
    public class VideoManager : MonoBehaviour
    {
        public VideoPlayer videoPlayer;
        public TextMeshProUGUI title;

        public void Initialize(string path, string title)
        {
            videoPlayer.clip = Resources.Load<VideoClip>(path);
            this.title.text = title;
        }
        
        public void Close()
        {
            Destroy(gameObject);
        }
    }
}