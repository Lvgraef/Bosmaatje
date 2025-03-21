using System.Linq;
using System.Text.RegularExpressions;
using ApiClient;
using Dto;
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
            if (!HandleRegistrationInputValidation()) return;
       
            var success = await RegistrationApiClient.Register(new PostRegisterRequestDto
            {
                email = emailInputField.text.ToLower(),
                password = passwordInputField.text
            }, errorText);

            if (success)
            {
                await LoginApiClient.Login(errorText, new PostLoginRequestDto
                {
                    email = emailInputField.text.ToLower(),
                    password = passwordInputField.text
                });
                await SceneManager.LoadSceneAsync("Scenes/Welcome");
            }
        }
        
        public async void OnLoginButton()
        {
            await SceneManager.LoadSceneAsync("Scenes/Login");
        }


        private bool HandleRegistrationInputValidation()
        {
            if (string.IsNullOrEmpty(emailInputField.text) || string.IsNullOrEmpty(passwordInputField.text) || string.IsNullOrEmpty(confirmPasswordInputField.text))
            {
                errorText.text = "Vul alle velden in.";
                return false;
            }
            if (!Regex.IsMatch(emailInputField.text.ToLower(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                errorText.text = "Ongeldig e-mailadres.";
                return false;
            }
            if (passwordInputField.text.Length < 10)
            {
                errorText.text = "Wachtwoord moet minimaal 10 karakters bevatten.";
                return false;
            }
            if (!Regex.IsMatch(passwordInputField.text, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).+$"))
            {
                errorText.text = "wachtwoord moet een symbool bevatten";
                return false;
            }
            if (passwordInputField.text != confirmPasswordInputField.text)
            {
                errorText.text = "Wachtwoorden komen niet overeen.";
                return false;
            }

            return true;
        }
    }
    
}
