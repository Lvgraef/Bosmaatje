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
            //todo
        }
    }
}