using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Global;
using UnityEngine.SceneManagement;
using System.Data.Common;




public class SettingsManager : MonoBehaviour
{
    void Start()
    {
        SetValuesSettingsManager();



    }

    private float volumeOfSound = .5f;
    private bool isSoundOn = true;

    public Button sliderbutton;
    public Slider slider;

    public Sprite soundOff;
    public Sprite soundOnlow;
    public Sprite soundOnmedium;
    public Sprite soundOnhigh;

    public void OnSliderValueChanged(float volume)
    {
        //float volume = slider.values;

        AudioListener.volume = volume;

        CheckVolumeSetImage(volume);
        volumeOfSound = volume;
        Debug.Log("Volume: " + volume);
    }

    public void OnSoundButtonClick()
    {
        if (sliderbutton.image.sprite == soundOff)
        {
            AudioListener.volume = volumeOfSound;
            slider.value = volumeOfSound;
            CheckVolumeSetImage(volumeOfSound);
        }
        else
        {
            sliderbutton.image.sprite = soundOff;
            AudioListener.volume = 0;
            isSoundOn = false;
        }
    }

    public void SaveOnButtonClick()
    {
        SetAndGetUserPreferences.instance.SetPrefSoundOn(isSoundOn);
        SetAndGetUserPreferences.instance.SetPrefSoundVolume(volumeOfSound);
    }

    public void ToConfigurationscreenOnButtonClick()
    {
        SaveOnButtonClick();// ik twijfel of we nu wel of niet de save moeten aanroepen
        this.gameObject.SetActive(false);
        SceneManager.LoadScene("Configuration");
    }

    public void OnCloseButtonClick()
    {
        this.gameObject.SetActive(false);
    }

    public void OnOpenButtonClick()
    {
        SetValuesSettingsManager();
        this.gameObject.SetActive(true);
    }

    private void CheckVolumeSetImage(float volume)
    {
        Debug.Log("CheckVolumeSetImage is called");
        if (volume == 0)
        {
            sliderbutton.image.sprite = soundOff;
            isSoundOn = false;
        }
        else if (volume > 0 && volume <= 0.33)
        {
            sliderbutton.image.sprite = soundOnlow;
            isSoundOn = true;
        }
        else if (volume > 0.33 && volume <= 0.66)
        {
            sliderbutton.image.sprite = soundOnmedium;
            isSoundOn = true;
        }
        else if (volume > 0.66 && volume <= 1)
        {
            sliderbutton.image.sprite = soundOnhigh;
            isSoundOn = true;
        }
    }

    private void SetValuesSettingsManager()
    {
        if (!PlayerPrefs.HasKey("SoundOn"))
        {
            SetAndGetUserPreferences.instance.SetPrefSoundOn(true);
            isSoundOn = true;
        }
        if (!PlayerPrefs.HasKey("SoundVolume"))
        {
            SetAndGetUserPreferences.instance.SetPrefSoundVolume(.5F);
            volumeOfSound = .5f;
        }
        volumeOfSound = SetAndGetUserPreferences.instance.GetPrefSoundVolume();
        isSoundOn = SetAndGetUserPreferences.instance.GetPrefSoundOn();

        Debug.Log("Volume: " + volumeOfSound);
        Debug.Log("SoundOn: " + isSoundOn);

        if (isSoundOn)
        {
            slider.value = volumeOfSound;
            CheckVolumeSetImage(volumeOfSound);
        }
        else
        {
            sliderbutton.image.sprite = soundOff;
        }

    }
}
