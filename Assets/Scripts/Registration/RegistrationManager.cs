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
            if (!ValidateRegisterInput()) return;

            errorText.color = Color.yellow;
            errorText.text = "We proberen u in te loggen";

            var success = await RegistrationApiClient.Register(new PostRegisterRequestDto
            {
                email = emailInputField.text.ToLower(),
                password = passwordInputField.text
            }, errorText);

            if (success)
            {
                await SceneManager.LoadSceneAsync("Scenes/Welcome");
            }
        }
        
        public async void OnLoginButton()
        {
            await SceneManager.LoadSceneAsync("Scenes/Login");
        }

        public bool ValidateRegisterInput()
        {
            string email = emailInputField.text.Trim().ToLower();
            string password = passwordInputField.text;
            string confirmPassword = confirmPasswordInputField.text;
            errorText.color = Color.red;

            if (string.IsNullOrEmpty(email))
            {
                errorText.text = "Vul nog een email in";
                return false;
            }

            if (!IsValidEmail(email))
            {
                errorText.text = "Ongeldig e-mailadress, voer een geldige in volgens het formaat: voorbeeld@domein.com";
                return false;
            }

            if (string.IsNullOrEmpty(password))
            {
                errorText.text = "Vul nog een wachtwoord in, deze moet minimaal 8 tekens lang zijn en een hoofdletter, kleine letter, cijfer en speciaal teken bevatten.";
                return false;
            }

            if (password != confirmPassword)
            {
                errorText.text = "De wachtwoorden komen niet overeen"; // deze lijkt somehow niet we werken
            }

            if (password.Length < 8)
            {
                errorText.text = "Uw wachtwoord moet minimaal 8 tekens lang zijn";
                return false;
            }
            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                errorText.text = "Uw wachtwoord moet minimaal 1 hoofdletter bevatten";
                return false;
            }

            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                errorText.text = "Uw wachtwoord moet minimaal 1 kleine letter bevatten";
                return false;
            }

            if (!Regex.IsMatch(password, @"\d"))
            {
                errorText.text = "Uw wachtwoord moet minimaal 1 cijfer bevatten";
                return false;
            }

            if (!Regex.IsMatch(password, @"[\W_]"))
            {
                errorText.text = "Uw wachtwoord moet minimaal 1 speciaal teken bevatten";
                return false;
            }
            return true;
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
