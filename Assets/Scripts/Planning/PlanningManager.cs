using System;
using System.Collections.Generic;
using Dto;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace Planning
{
    public class PlanningManager : MonoBehaviour
    {
        public List<TextMeshProUGUI> treatmentNames;
        public List<TextMeshProUGUI> treatmentDates;
        public List<TextMeshProUGUI> treatmentDays;
        
        public void Initialize([ItemCanBeNull] GetTreatmentRequestDto[] treatments)
        {
            for (var i = 0; i < treatments.Length; i++)
            {
                if (treatments [i] == null)
                {
                    treatmentNames[i].text = "Geen behandeling";
                    treatmentDates[i].text = "Geen datum";
                    treatmentDays[i].text = "Geen datum";
                    continue;
                }
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
                        treatmentDays[i].text = "Nog " + (treatments[i].date - DateTime.Now + TimeSpan.FromDays(1)).Value.Days + " Dag(en)";
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