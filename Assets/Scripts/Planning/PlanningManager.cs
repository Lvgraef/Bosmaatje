using System;
using System.Collections.Generic;
using System.Linq;
using ApiClient;
using Dto;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace Planning
{
    public class PlanningManager : MonoBehaviour
    {
        public GameObject planPrefab;
        public GameObject customPlanPrefab;
        public GameObject createPrefab;
        public Transform parent;

        public async void Initialize([ItemCanBeNull] GetTreatmentResponseDto[] treatments)
        {
            var appointments = (from treatment in treatments
                where treatment?.date != null && !(treatment.date < DateTime.Now)
                select new Appointment { Title = treatment.treatmentName, Date = treatment.date.Value, Custom = false}).ToList();

            var customPlans = await AppointmentApiClient.GetAppointments();
            if (customPlans != null)
            {
                appointments.AddRange(from customPlan in customPlans
                    where customPlan?.date != null && !(customPlan.date < DateTime.Now)
                    select new Appointment { Title = customPlan.name, Date = customPlan.date, Custom = true});
            }

            appointments.Sort((first, second) => first.Date.CompareTo(second.Date));

            foreach (var appointment in appointments)
            {
                var plan = Instantiate(appointment.Custom ? customPlanPrefab : planPrefab, parent).GetComponent<Plan>();
                plan.title.text = appointment.Title;
                plan.date.text = appointment.Date.ToString("dd/MM/yyyy");
                plan.days.text = "Nog " + (appointment.Date - DateTime.Now + TimeSpan.FromDays(1)).Days +
                                 " Dag(en)";
            }
        }

        public void Open()
        {
            
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }

    public class Appointment
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public bool Custom { get; set; }
    }
}