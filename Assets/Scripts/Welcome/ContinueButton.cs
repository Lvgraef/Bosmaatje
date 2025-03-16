using UnityEngine;
using UnityEngine.SceneManagement;

namespace Welcome
{
    public class ContinueButton : MonoBehaviour
    {
        public async void Continue()
        {
            await SceneManager.LoadSceneAsync("Scenes/Configuration");
        }
    }
}