using UnityEngine;

namespace Global
{
    public class AudioPlayer : MonoBehaviour
    {
        public void Play(AudioClip clip)
        {
            AudioSingleton.Instance.source.PlayOneShot(clip);
        }
    }
}