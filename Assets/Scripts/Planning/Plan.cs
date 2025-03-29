using TMPro;
using UnityEditor;
using UnityEngine;

namespace Planning
{
    public class Plan : MonoBehaviour
    {
        public GUID AppointmentId { get; set; }
        public TextMeshProUGUI title;
        public TextMeshProUGUI date;
        public TextMeshProUGUI days;
    }
}