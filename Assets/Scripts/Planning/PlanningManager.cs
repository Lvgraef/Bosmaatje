using System;
using System.Collections.Generic;
using Dto;
using TMPro;
using UnityEngine;

namespace Planning
{
    public class PlanningManager : MonoBehaviour
    {
        public List<TextMeshProUGUI> treatmentNames;
        public List<TextMeshProUGUI> treatmentDates;
        public List<TextMeshProUGUI> treatmentDays;
        
        public void Initialize(GetTreatmentRequestDto[] treatments)
        {
            for (var i = 0; i < treatments.Length; i++)
            {
                treatmentNames[i].text = treatments[i].treatmentName;
                treatmentDates[i].text = treatments[i].date?.ToString("dd/MM/yyyy") ?? "Geen datum";
                if (treatments[i].date != null)
                {
                    if (treatments[i].date < DateTime.Now)
                    {
                        treatmentDays[i].text = "Behandeling al geweest!";
                    }
                    else
                    {
                        treatmentDays[i].text = "Nog " + (treatments[i].date - DateTime.Now).Value.Days + " Dagen";
                    }
                }
                else
                {
                    treatmentDays[i].text = "Geen datum";
                }
            }
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}