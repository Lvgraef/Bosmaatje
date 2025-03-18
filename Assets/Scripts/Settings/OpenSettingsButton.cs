using UnityEngine;

namespace Settings
{
    public class OpenSettingsButton : MonoBehaviour
    {
        public GameObject settingsPanel;
        public GameObject canvas;
        
        public async void OpenSettings()
        {
            var settings = await InstantiateAsync(settingsPanel, canvas.transform);
            settings[0].GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        }
    }
}