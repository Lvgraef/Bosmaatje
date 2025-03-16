using ApiClient;
using Dto;
using TMPro;
using UI.Dates;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            
            var dto = new PostConfigureRequestDto
            {
                childBirthDate = childBirthDateField.SelectedDate.Date,
                characterId = characterSelector.characters[characterSelector.selectedCharacter].name,
                childName = childNameField.text,
                primaryDoctorName = primaryDoctorNameField.text,
                treatmentPlanName = treatmentPlanSelector.selectedButton == 0 ? "Hospitalization" : "NoHospitalization"
            };

            if (!await ConfigurationApiClient.Configure(dto, statusText)) return;
            
            if (await ConfigurationApiClient.PutFirstTreatment(new PutTreatmentRequestDto
                {
                    date = treatmentStartDateField.SelectedDate.Date
                }, statusText))
            {
                await SceneManager.LoadSceneAsync("Scenes/introduction");
            }
        }
    }
}