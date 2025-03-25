using System.Collections.Generic;
using ApiClient;
using Dto;
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
        [FormerlySerializedAs("calendarPrefab")] public GameObject planningPrefab;
        public Transform canvas;
        public List<Image> pos;
        public GetConfigurationsRequestDto Configuration { get; set; }
        public GetTreatmentRequestDto[] Treatments { get; set; }

        private async void Start()
        {
            Configuration = await ConfigurationApiClient.GetConfiguration();
            Treatments = await TreatmentPlanApiClient.GetTreatments(statusText, Configuration?.treatmentPlanName);
            Progress();
        }

        public void OpenPlanning()
        {
            var planning = Instantiate(planningPrefab, canvas);
            planning.GetComponent<PlanningManager>().Initialize(Treatments);
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
                if (Treatments[i].isCompleted)
                {
                    completion = i;
                    break;
                }
            }

            pos[completion].sprite = sprite;
            pos[completion].gameObject.SetActive(true);
        }

        public void OpenTreatment(int index)
        {
            var treatment = Instantiate(treatmentPrefab.gameObject, canvas);
            treatment.GetComponent<TreatmentManager>().Initialize(Treatments[index].treatmentId, Treatments[index].treatmentName, Treatments[index].description, Treatments[index].imagePath, Treatments[index].videoPath, Treatments[index].date, Treatments[index].stickerId, Configuration.primaryDoctorName);
        }
    }
}