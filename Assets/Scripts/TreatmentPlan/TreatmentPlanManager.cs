using System;
using System.Collections.Generic;
using System.Linq;
using ApiClient;
using Dto;
using JetBrains.Annotations;
using Messages;
using Planning;
using TMPro;
using Treatment;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TreatmentPlan
{
    public class TreatmentPlanManager : MonoBehaviour
    {
        public TextMeshProUGUI statusText;
        public TreatmentManager treatmentPrefab;

        public GameObject planningPrefab;

        public Transform canvas;
        public List<Image> pos;
        public List<TextMeshProUGUI> treatmentNames;
        public List<TextMeshProUGUI> timeLeft;
        public GetConfigurationsResponseDto Configuration { get; set; }
        public GameObject mushroomA;
        public GameObject mushroomB;
        public GameObject reminderPrefab;

        [ItemCanBeNull]
        public GetTreatmentResponseDto[] Treatments { get; set; } = { null, null, null, null, null, null };

        public GameObject treatmentUnavailable;
        public Message message;

        public async void Start()
        {
            Configuration = await ConfigurationApiClient.GetConfiguration();
            var treatments = await TreatmentPlanApiClient.GetTreatments(statusText, Configuration?.treatmentPlanName);
            for (var i = 0; i < 6; i++)
            {
                Treatments![i] = treatments?.SingleOrDefault(treatment => treatment.order == i);
            }

            for (var i = 0; i < 6; i++)
            {
                if (Treatments?[3] == null)
                {
                    if (i == 4)
                    {
                        treatmentNames[i].text = "Onbekend";
                        continue;
                    }

                    treatmentNames[i].text = "Onbekend";
                }
                
                if (i == 4)
                {
                    treatmentNames[i].text = Treatments?[3]?.treatmentName;
                    break;
                }

                treatmentNames[i].text = Treatments?[i]?.treatmentName;
            }
            
            treatmentNames[5].text = Treatments?[4]?.treatmentName;
            treatmentNames[6].text = Treatments?[5]?.treatmentName;


            if (Configuration?.treatmentPlanName == "Hospitalization")
            {
                mushroomA.SetActive(true);
                mushroomB.SetActive(false);
            }
            else
            {
                mushroomA.SetActive(false);
                mushroomB.SetActive(true);
            }

            Progress();
            Reminders();
        }

        public void OpenPlanning()
        {
            var planning = Instantiate(planningPrefab, canvas);
            planning.GetComponent<PlanningManager>().Initialize();
        }

        private async void Reminders()
        {
            var appointments = (from treatment in Treatments
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
                if (appointment.Date - DateTime.Now > TimeSpan.FromDays(1)) continue;
                var reminderObject = Instantiate(reminderPrefab, canvas);
                var reminder = reminderObject.GetComponent<Reminder>();
                reminder.name.text = appointment.Title;
                reminder.date.text = appointment.Date.ToString("dd/MM/yyyy");
            }
        }
        
        private void Progress()
        {
            foreach (var image in pos)
            {
                image.gameObject.SetActive(false);
            }

            var sprite = Resources.Load<Sprite>(Configuration?.characterId);
            int completion = 0;
            for (var i = Treatments!.Length - 1; i >= 0; i--)
            {
                if (Treatments[i]?.stickerId != null)
                {
                    completion = i + 1;
                    break;
                }
            }

            if (completion < 5)
            {
                if (Treatments[completion + 1]?.date != null)
                {
                    timeLeft[completion].transform.parent.gameObject.SetActive(true);
                    timeLeft[completion].text = $"Nog {(Treatments[completion + 1]?.date - DateTime.Now + TimeSpan.FromDays(1))?.Days} dagen";
                }
                else
                {
                    timeLeft[completion].transform.parent.gameObject.SetActive(false);
                }
            }


            pos[completion].sprite = sprite;
            pos[completion].gameObject.SetActive(true);
        }

        public void OpenTreatment(int index)
        {
            if (Treatments[index] == null)
            {
                var unavailable = Instantiate(treatmentUnavailable, canvas);
                unavailable.GetComponent<RectTransform>().localPosition = Vector3.zero;
                return;
            }

            var treatment = Instantiate(treatmentPrefab.gameObject, canvas);
            treatment.GetComponent<TreatmentManager>().Initialize(Treatments[index].order, this,
                Treatments[index].treatmentId, Treatments[index].treatmentName, Treatments[index].description,
                Treatments[index].imagePath, Treatments[index].videoPath, Treatments[index].date,
                Treatments[index].stickerId, Configuration.primaryDoctorName);
            message.OpenRandom(canvas);
        }
    }
}