using ApiClient;
using Dto;
using TMPro;
using UI.Dates;
using UnityEngine;

namespace Planning
{
    public class PlanCreation : MonoBehaviour
    {
        public TextMeshProUGUI statusText;
        public TMP_InputField name;
        public DatePicker date;

        public async void Save()
        {
            if (await AppointmentApiClient.PostAppointment(new PostAppointmentRequestDto
                {
                    name = name.text,
                    date = date.SelectedDate
                }, statusText))
            {
                Destroy(gameObject);
            }
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}