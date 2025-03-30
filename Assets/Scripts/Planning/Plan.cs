using System;
using ApiClient;
using TMPro;
using UnityEngine;

namespace Planning
{
    public class Plan : MonoBehaviour
    {
        public PlanningManager PlanningManager { get; set; }
        public GameObject planningPrefab;
        public Guid AppointmentId { get; set; }
        public TextMeshProUGUI title;
        public TextMeshProUGUI date;
        public TextMeshProUGUI days;

        public async void Delete()
        {
            await AppointmentApiClient.DeleteAppointment(AppointmentId);
            PlanningManager.Initialize();
        }
    }
}