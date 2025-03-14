using ApiClient;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Login
{
    public class LoginManager : MonoBehaviour
    {
        public LoginApiClient apiClient;
        public TextMeshProUGUI statusText;
        public TMP_InputField emailField;
        public TMP_InputField passwordField;

        public async void Login()
        {
            if (await apiClient.Login(statusText, emailField.text, passwordField.text))
            {
                //todo load next scene
                await SceneManager.LoadSceneAsync("Scenes/Login");
            }
        }
        
        public async void Register()
        {
            //todo
            await SceneManager.LoadSceneAsync("Scenes/Register");
        }
    }
}