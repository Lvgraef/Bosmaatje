﻿using ApiClient;
using Dto;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Login
{
    public class LoginManager : MonoBehaviour
    {
        public TextMeshProUGUI statusText;
        public TMP_InputField emailField;
        public TMP_InputField passwordField;

        public async void Login()
        {
            if (emailField.text.ToLower() == "" || passwordField.text == "")
            {
                statusText.text = "Fill in both email and password.";
                return;
            }
            
            if (await LoginApiClient.Login(statusText, new PostLoginRequestDto
                {
                    email = emailField.text.ToLower(),
                    password = passwordField.text
                }))
            {
                await SceneManager.LoadSceneAsync("Scenes/TreatmentPlan");
            }
        }
        
        public async void Register()
        {
            await SceneManager.LoadSceneAsync("Scenes/Registration");
        }
    }
}