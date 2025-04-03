using System.Xml.Serialization;
using UnityEngine;

namespace Global
{
    public class SettingsSingleton : MonoBehaviour
    {
        public static SettingsSingleton Instance { get; private set; }

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
            if (!PlayerPrefs.HasKey("SoundMuted"))
            {
                SetPrefSoundMuted(false);
            }
            if (!PlayerPrefs.HasKey("SoundVolume"))
            {
                SetPrefSoundVolume(.5F);
            }

            if (GetPrefSoundMuted())
            {
                AudioListener.volume = 0;
            }
            else
            {
                AudioListener.volume = GetPrefSoundVolume();
            }
        }

        public void SetPrefSoundMuted(bool isMuted)
        {
            SetBool("SoundMuted", isMuted);
        }
        public bool GetPrefSoundMuted()
        {
            return Getbool("SoundMuted");
        }

        public void SetPrefSoundVolume(float Value)
        {
            SetFloat("SoundVolume", Value);
        }
        public float GetPrefSoundVolume()
        {
            return GetFloat("SoundVolume");
        }

        private void SetBool(string KeyName, bool Value)
        {
            PlayerPrefs.SetInt(KeyName, Value ? 1 : 0);
        }

        private bool Getbool(string KeyName)
        {
            if (!PlayerPrefs.HasKey(KeyName))
            {
                return true;
            }

            return PlayerPrefs.GetInt(KeyName) == 1 ? true : false;
        }

        private void SetFloat(string KeyName, float Value)
        {
            PlayerPrefs.SetFloat(KeyName, Value);
        }
        private float GetFloat(string KeyName)
        {
            if (!PlayerPrefs.HasKey(KeyName))
            {
                return .5F;
            }
            return PlayerPrefs.GetFloat(KeyName);
        }
    }

}