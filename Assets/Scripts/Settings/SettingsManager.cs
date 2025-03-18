using Global;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Settings
{
    public class SettingsManager : MonoBehaviour
    {
        private void Start()
        {
            SetValuesSettingsManager();
            sliderbutton.image.sprite = soundOnhigh;
        }

        private float _volumeOfSound = 1;
        private bool _isSoundOn = true;

        public Button sliderbutton;
        public Slider slider;

        public Sprite soundOff;
        public Sprite soundOnlow;
        public Sprite soundOnmedium;
        public Sprite soundOnhigh;

        public void OnSliderValueChanged(float volume)
        {
            AudioListener.volume = volume;

            CheckVolumeSetImage(volume);
            _volumeOfSound = volume;
            Debug.Log("Volume: " + volume);
        }

        public void OnSoundButtonClick()
        {
            if (sliderbutton.image.sprite == soundOff)
            {
                AudioListener.volume = _volumeOfSound;
                slider.value = 1;
                CheckVolumeSetImage(_volumeOfSound);
            }
            else
            {
                sliderbutton.image.sprite = soundOff;
                slider.value = 0;
                AudioListener.volume = 0;
                _isSoundOn = false;
            }
        }

        public void SaveOnButtonClick()
        {
            SettingsSingleton.Instance.SetPrefSoundOn(_isSoundOn);
            SettingsSingleton.Instance.SetPrefSoundVolume(_volumeOfSound);
            Destroy(gameObject);
        }

        public void ToConfigurationscreenOnButtonClick()
        {
            SaveOnButtonClick();
            SceneManager.LoadScene("Configuration");
        }

        private void CheckVolumeSetImage(float volume)
        {
            Debug.Log("CheckVolumeSetImage is called");
            if (volume == 0)
            {
                sliderbutton.image.sprite = soundOff;
                _isSoundOn = false;
            }
            else if (volume > 0 && volume <= 0.33)
            {
                sliderbutton.image.sprite = soundOnlow;
                _isSoundOn = true;
            }
            else if (volume > 0.33 && volume <= 0.66)
            {
                sliderbutton.image.sprite = soundOnmedium;
                _isSoundOn = true;
            }
            else if (volume > 0.66 && volume <= 1)
            {
                sliderbutton.image.sprite = soundOnhigh;
                _isSoundOn = true;
            }
        }

        private void SetValuesSettingsManager()
        {
            if (!PlayerPrefs.HasKey("SoundOn"))
            {
                SettingsSingleton.Instance.SetPrefSoundOn(true);
                _isSoundOn = true;
            }
            if (!PlayerPrefs.HasKey("SoundVolume"))
            {
                SettingsSingleton.Instance.SetPrefSoundVolume(.5F);
                _volumeOfSound = .5f;
            }
            _volumeOfSound = SettingsSingleton.Instance.GetPrefSoundVolume();
            _isSoundOn = SettingsSingleton.Instance.GetPrefSoundOn();

            Debug.Log("Volume: " + _volumeOfSound);
            Debug.Log("SoundOn: " + _isSoundOn);

            if (_isSoundOn)
            {
                slider.value = _volumeOfSound;
                CheckVolumeSetImage(_volumeOfSound);
            }
            else
            {
                sliderbutton.image.sprite = soundOff;
            }

        }
    }
}
