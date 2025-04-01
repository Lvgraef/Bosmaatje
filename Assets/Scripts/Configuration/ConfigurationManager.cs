using System;
using ApiClient;
using Dto;
using TMPro;
using TreatmentPlan;
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
        public GameObject treatmentText;
        private GetTreatmentResponseDto[] _treatments;
        
        private async void Start()
        {
            _treatments = await TreatmentPlanApiClient.GetTreatments(statusText, null);
            int completion = 0;
            for (var i = _treatments!.Length - 1; i >= 0; i--)
            {
                if (_treatments[i]?.stickerId != null)
                {
                    completion = i;
                    break;
                }
            }
            if (completion >= 2)
            {
                treatmentPlanSelector.SetEnabled(true);
                treatmentText.SetActive(false);
            }
            
            var config = await ConfigurationApiClient.GetConfiguration();
            if (config == null)
            {
                childNameField.interactable = true;
                dateOfBirthBlocker.SetActive(false);
                childNameBlocker.SetActive(false);
            }
            else
            {
                childNameField.interactable = false;
                dateOfBirthBlocker.SetActive(true);
                childNameBlocker.SetActive(true);
                
                childNameField.text = config.childName;
                childBirthDateField.SelectedDate = config.childBirthDate;
                primaryDoctorNameField.text = config.primaryDoctorName;
                characterSelector.SetCharacter(config.characterId);
                treatmentPlanSelector.SelectButton(config.treatmentPlanName switch
                {
                    null => -1,
                    "Hospitalization" => 0,
                    _ => 1
                });
                if (_treatments[0].date != null)
                {
                    treatmentStartDateField.SelectedDate = _treatments[0].date.Value;
                }
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

            var config = await ConfigurationApiClient.GetConfiguration();
            if (config == null)
            {
                var dto = new PostConfigurationsRequestDto
                {
                    childBirthDate = childBirthDateField.SelectedDate.Date,
                    characterId = characterSelector.characters[characterSelector.selectedCharacter].name,
                    childName = childNameField.text,
                    primaryDoctorName = primaryDoctorNameField.text,
                    treatmentPlanName = treatmentPlanSelector.selectedButton switch
                    {
                        -1 => null,
                        0 => "Hospitalization",
                        _ => "NoHospitalization"
                    }
                };

                if (!await ConfigurationApiClient.Configure(dto, statusText)) return;
            }
            else
            {
                var dto = new PutConfigurationRequestDto
                {
                    characterId = characterSelector.characters[characterSelector.selectedCharacter].name,
                    primaryDoctorName = primaryDoctorNameField.text,
                    treatmentPlanName = treatmentPlanSelector.selectedButton switch
                    {
                        -1 => null,
                        0 => "Hospitalization",
                        _ => "NoHospitalization"
                    }
                };
                
                if (!await ConfigurationApiClient.UpdateConfigure(dto, statusText)) return;
            }
            
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