using ApiClient;
using Dto;
using TMPro;
using Treatment;
using UnityEngine;

namespace TreatmentPlan
{
    public class TreatmentPlanManager : MonoBehaviour
    {
        public TextMeshProUGUI statusText;
        public TreatmentManager treatmentPrefab;
        public Transform canvas;
        public GetConfigurationsRequestDto Configuration { get; set; }
        public GetTreatmentRequestDto[] Treatments { get; set; }

        private async void Start()
        {
            Configuration = await ConfigurationApiClient.GetConfiguration();
            Treatments = await TreatmentPlanApiClient.GetTreatments(statusText, Configuration?.treatmentPlanName);
        }

        public void OpenTreatment(int index)
        {
            var treatment = Instantiate(treatmentPrefab.gameObject, canvas);
            treatment.GetComponent<TreatmentManager>().Initialize(Treatments[index].treatmentId, Treatments[index].treatmentName, Treatments[index].description, Treatments[index].imagePath, Treatments[index].videoPath, Treatments[index].date, Treatments[index].stickerId, Configuration.primaryDoctorName);
        }
    }
}