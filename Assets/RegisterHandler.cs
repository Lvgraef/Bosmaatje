using System.Linq;
using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;

public class RegisterHandler : MonoBehaviour
{
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField confirmPasswordInputField;
    public TMP_Text errorText;

    private bool userTriedRegistrating = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (userTriedRegistrating)
        {
            HandleRegistrationInputValidation();
        }
        
    }

    public void OnRegisterButton()
    {
        userTriedRegistrating = true;
        HandleRegistrationInputValidation();
        // validation registration


        // registration

        // with the API
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
        if (!passwordInputField.text.Any(c => char.IsSymbol(c)))
        {
            errorText.text = "wachtwoord moet een symbool bevatten";
            return;
        }
        if (!emailInputField.text.Contains("@"))
        {
            errorText.text = "Ongeldig e-mailadres.";
            return;
        }
    }


}
