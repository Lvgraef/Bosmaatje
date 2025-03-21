using UnityEngine;
using UnityEngine.Video;

namespace Treatment
{
    public class VideoManager : MonoBehaviour
    {
        public VideoPlayer videoPlayer;
        
        public void Close()
        {
            Destroy(gameObject);
        }

        public void SetVideo(string path)
        {
            videoPlayer.clip = Resources.Load<VideoClip>(path);
        }
    }
}