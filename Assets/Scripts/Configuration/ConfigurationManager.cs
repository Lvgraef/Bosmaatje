﻿using System;
using ApiClient;
using Dto;
using TMPro;
using UI.Dates;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

namespace Configuration
{
    public class ConfigurationManager : MonoBehaviour
    {
        public TextMeshProUGUI statusText;
        public TMP_InputField childNameField;
        public DatePicker childBirthDateField;
        public TMP_InputField primaryDoctorNameField;
        public CharacterSelector characterSelector;
        public RadioButton treatmentPlanSelector;
        public DatePicker treatmentStartDateField;
        public GameObject dateOfBirthBlocker;
        public GameObject childNameBlocker;

        private async void Start()
        {
            var config = await ConfigurationApiClient.GetConfiguration();
            if (config == null)
            {
                childNameField.interactable = true;
                dateOfBirthBlocker.SetActive(false);
                childNameBlocker.SetActive(false);
                treatmentPlanSelector.SetEnabled(true);
            }
            else
            {
                childNameField.interactable = false;
                dateOfBirthBlocker.SetActive(true);
                childNameBlocker.SetActive(true);
                treatmentPlanSelector.SetEnabled(true);
            }
        }

        public async void Configure()
        {
            if (childNameField.text.Length >= 50)
            {
                statusText.text = "De naam van het kind moet minder dan 50 karakters lang zijn.";
                return;
            }
            if (primaryDoctorNameField.text.Length >= 50)
            {
                statusText.text = "De naam van de arts moet minder dan 50 karakters lang zijn.";
                return;
            }

            if (string.IsNullOrEmpty(childNameField.text))
            {
                statusText.text = "Vul de naam van het kind in.";
                return;
            }
            if (string.IsNullOrEmpty(primaryDoctorNameField.text))
            {
                statusText.text = "Vul de naam van de arts in.";
                return;
            }

            if (!childBirthDateField.SelectedDate.HasValue)
            {
                statusText.text = "Vul de geboortedatum van het kind in.";
                return;
            }
            
            var dto = new PostConfigurationsRequestDto
            {
                childBirthDate = childBirthDateField.SelectedDate.Date,
                characterId = characterSelector.characters[characterSelector.selectedCharacter].name,
                childName = childNameField.text,
                primaryDoctorName = primaryDoctorNameField.text,
                treatmentPlanName = treatmentPlanSelector.selectedButton == 0 ? "Hospitalization" : "NoHospitalization"
            };

            if (!await ConfigurationApiClient.Configure(dto, statusText)) return;

            if (treatmentStartDateField.SelectedDate.HasValue)
            {
                if (await ConfigurationApiClient.PutFirstTreatment(new PutTreatmentRequestDto
                    {
                        date = treatmentStartDateField.SelectedDate.Date
                    }, statusText, treatmentPlanSelector.selectedButton == 0 ? "Hospitalization" : "NoHospitalization"))
                {
                    await SceneManager.LoadSceneAsync("Scenes/Introduction");
                }
            }
            else
            {
                await SceneManager.LoadSceneAsync("Scenes/Introduction");
            }
        }
    }
}