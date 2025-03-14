using System.Linq;
using System.Text.RegularExpressions;
using ApiClient;
using Assets.Dto_s;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Registration
{
    public class RegistrationManager : MonoBehaviour
    {
        public TMP_InputField emailInputField;
        public TMP_InputField passwordInputField;
        public TMP_InputField confirmPasswordInputField;
        public TextMeshProUGUI errorText;
        
        public async void OnRegisterButton()
        {
            HandleRegistrationInputValidation();
       
            var success = await RegistrationApiClient.Register(new PostRegisterRequestDto
            {
                Email = emailInputField.text,
                Password = passwordInputField.text
            }, errorText);

            if (success)
            {
                //todo load next scene
                await SceneManager.LoadSceneAsync("Scenes/Login");
            }
        }
        
        public async void OnLoginButton()
        {
            await SceneManager.LoadSceneAsync("Scenes/Login");
        }


        private void HandleRegistrationInputValidation()
        {
            if (string.IsNullOrEmpty(emailInputField.text) || string.IsNullOrEmpty(passwordInputField.text) || string.IsNullOrEmpty(confirmPasswordInputField.text))
            {
                errorText.text = "Vul alle velden in.";
                return;
            }
            if (passwordInputField.text != confirmPasswordInputField.text)
            {
                errorText.text = "Wachtwoorden komen niet overeen.";
                return;
            }
            if (passwordInputField.text.Length < 8)
            {
                errorText.text = "Wachtwoord moet minimaal 8 karakters bevatten.";
                return;
            }
            if (!Regex.IsMatch(passwordInputField.text, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).+$"))
            {
                errorText.text = "wachtwoord moet een symbool bevatten";
                return;
            }
            if (!Regex.IsMatch(emailInputField.text.ToLower(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                errorText.text = "Ongeldig e-mailadres.";
            }
        }
    }
}
