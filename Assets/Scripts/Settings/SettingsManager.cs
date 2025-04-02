using System.Linq;
using Global;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Settings
{
    public class SettingsManager : MonoBehaviour
    {
        private bool _isdebug = true;

        public GameObject statusText;

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
            statusText.SetActive(false);
            SetValuesSettingsManager();
            ChangeSliderValue(_volumeOfSound);
            ChangeSoundimage(_volumeOfSound);

        }

        public void OnSliderValueChanged(float volume)
        {
            AudioListener.volume = volume;
            _volumeOfSound = volume;
            _isAudioMuted = false;

            ChangeSoundimage(volume);
            if (_isdebug) Debug.Log(volume);
        }

        public void OnSoundButtonClick()
        {
            Color fillColor = Color.white;
            if (_isAudioMuted)
            {
                // Unmute
                AudioListener.volume = _volumeOfSound;
                _isAudioMuted = false;

                fillColor = new Color(13f / 255f, 43f / 255f, 32f / 255f); // Dark Green
            }
            else
            {
                // Mute
                AudioListener.volume = 0;
                _isAudioMuted = true;

                fillColor = new Color(210f / 255f, 196f / 255f, 157f / 255f); // Light Brown
            }
            ChangeFillColorSlider(fillColor);
            ChangeSoundimage(_volumeOfSound);
        }

        public void SaveOnButtonClick()
        {
            SettingsSingleton.Instance.SetPrefSoundMuted(_isAudioMuted);
            SettingsSingleton.Instance.SetPrefSoundVolume(_volumeOfSound);
            statusText.SetActive(true);
        }

        public void ToConfigurationscreenOnButtonClick()
        {
            SaveOnButtonClick();
            SceneManager.LoadScene("Configuration");
        }

        public void CloseSettingsOnButtonClick()
        {
            SetValuesSettingsManager();
            gameObject.SetActive(false);
            statusText.SetActive(false);
        }

        public void OpenSettingsOnButtonClick()
        {
            gameObject.SetActive(true);
        }

        private void ChangeSoundimage(float volume)
        {
            soundbutton.image.sprite = GetSoundImage(volume);
        }
        private void ChangeSliderValue(float volume)
        {
            slider.value = volume;
        }
        private Sprite GetSoundImage(float volume)
        {
            if (_isAudioMuted || volume <= 0)
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
            if (!PlayerPrefs.HasKey("SoundMuted"))
            {
                _isAudioMuted = false;
            }
            if (!PlayerPrefs.HasKey("SoundVolume"))
            {
                _volumeOfSound = .5f;
            }
            _volumeOfSound = SettingsSingleton.Instance.GetPrefSoundVolume();
            _isAudioMuted = SettingsSingleton.Instance.GetPrefSoundMuted();

            if (_isAudioMuted)
            {
                AudioListener.volume = 0;
                return;
            }

            AudioListener.volume = _volumeOfSound;
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