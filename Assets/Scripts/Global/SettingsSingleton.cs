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
            if (!PlayerPrefs.HasKey("SoundOn"))
            {
                SetPrefSoundOn(true);
            }
            if (!PlayerPrefs.HasKey("SoundVolume"))
            {
                SetPrefSoundVolume(.5F);
            }

            if (GetPrefSoundOn())
            {
                AudioListener.volume = GetPrefSoundVolume();
            }
            else
            {
                AudioListener.volume = 0;
            }
        }

        public void SetPrefSoundOn(bool turnedOn)
        {
            SetBool("SoundOn", turnedOn);
        }
        public bool GetPrefSoundOn()
        {
            return Getbool("SoundOn");
        }

        public void SetPrefSoundVolume(float Value)
        {
            SetFloat("SoundVolume", Value);
        }
        public float GetPrefSoundVolume()
        {
            return GetFloat("SoundVolume");
        }


        private void SetString(string KeyName, string Value)
        {
            PlayerPrefs.SetString(KeyName, Value);
        }

        private string GetString(string KeyName)
        {
            return PlayerPrefs.GetString(KeyName);
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