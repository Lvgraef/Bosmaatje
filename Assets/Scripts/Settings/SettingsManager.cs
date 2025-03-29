using System.Linq;
using Global;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Settings
{
    public class SettingsManager : MonoBehaviour
    {
        private float _volumeOfSound = 1;
        private bool _isAudioMuted = false;

        public Button soundbutton;
        public Slider slider;

        public Sprite soundOff;
        public Sprite soundOnlow;
        public Sprite soundOnmedium;
        public Sprite soundOnhigh;

        private void OnEnable()
        {
            SetValuesSettingsManager();
            ChangeSoundimage(_volumeOfSound);
            ChangeSliderValue(_volumeOfSound);
        }

        public void OnSliderValueChanged(float volume)
        {
            AudioListener.volume = volume;
            _volumeOfSound = volume;

            ChangeSoundimage(volume);
        }

        public void OnSoundButtonClick()
        {
            if (_isAudioMuted)
            {
                AudioListener.volume = 0;

                _isAudioMuted = false;

                Color fillColor = new Color(13, 43, 32);// dark green
                ChangeFillColorSlider(fillColor);
            }
            else
            {
                AudioListener.volume = _volumeOfSound;
                _isAudioMuted = true;

                Color fillColor = new Color(210, 196, 157);// light brown
                ChangeFillColorSlider(fillColor);
            }
            ChangeSoundimage(_volumeOfSound, _isAudioMuted);

        }

        public void SaveOnButtonClick()
        {
            SettingsSingleton.Instance.SetPrefSoundOn(_isAudioMuted);
            SettingsSingleton.Instance.SetPrefSoundVolume(_volumeOfSound);
        }

        public void ToConfigurationscreenOnButtonClick()
        {
            SaveOnButtonClick();
            SceneManager.LoadScene("Configuration");
        }

        public void CloseSettingsOnButtonClick()
        {
            gameObject.SetActive(false);
        }

        public void OpenSettingsOnButtonClick()
        {
            gameObject.SetActive(true);
        }

        private void ChangeSoundimage(float volume, bool makeSoundOff = false)
        {
            soundbutton.image.sprite = GetSoundImage(volume, !makeSoundOff);
        }
        private void ChangeSliderValue(float volume)
        {
            slider.value = volume;
        }
        private Sprite GetSoundImage(float volume, bool isSoundOn = true)
        {
            if (!isSoundOn || volume <= 0)
            {
                return soundOff;
            }

            volume = Mathf.Clamp01(volume);

            if (volume <= 0.33f)
            {
                return soundOnlow;
            }
            else if (volume <= 0.66f)
            {
                return soundOnmedium;
            }
            else
            {
                return soundOnhigh;
            }
        }



        private void SetValuesSettingsManager()
        {
            if (!PlayerPrefs.HasKey("SoundOn"))
            {
                _isAudioMuted = true;
            }
            if (!PlayerPrefs.HasKey("SoundVolume"))
            {
                _volumeOfSound = .5f;
            }
            _volumeOfSound = SettingsSingleton.Instance.GetPrefSoundVolume();
            _isAudioMuted = SettingsSingleton.Instance.GetPrefSoundOn();
        }

        private void ChangeFillColorSlider(Color fillColor)
        {
            var fill = (slider as UnityEngine.UI.Slider).GetComponentsInChildren<UnityEngine.UI.Image>().FirstOrDefault(t => t.name == "Fill");
            if (fill != null)
            {
                fill.color = fillColor;
            }
        }
    }
}
