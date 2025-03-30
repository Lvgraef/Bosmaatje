using ApiClient;
using Dto;
using TMPro;
using UI.Dates;
using UnityEngine;

namespace Planning
{
    public class PlanCreation : MonoBehaviour
    {
        public GameObject planningPrefab;
        public TextMeshProUGUI statusText;
        public TMP_InputField name;
        public DatePicker date;

        public async void Save()
        {
            if (string.IsNullOrEmpty(name.text) || !date.SelectedDate.HasValue)
            {
                statusText.color = Color.red;
                statusText.text = "Vul alle velden in";
                return;
            }
            
            if (await AppointmentApiClient.PostAppointment(new PostAppointmentRequestDto
                {
                    name = name.text,
                    date = date.SelectedDate
                }, statusText))
            {
                var planning = Instantiate(planningPrefab, transform.parent);
                planning.GetComponent<PlanningManager>().Initialize();
                Destroy(gameObject);
            }
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}