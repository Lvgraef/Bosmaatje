using TMPro;
using UnityEngine;

namespace Planning
{
    public class Reminder : MonoBehaviour
    {
        public GameObject planningPrefab;
        public TextMeshProUGUI name;
        public TextMeshProUGUI date;
        
        public void Close()
        {
            Destroy(gameObject);
        }

        public void View()
        {
            var planning = Instantiate(planningPrefab, transform.parent);
            planning.GetComponent<PlanningManager>().Initialize();
        }
    }
}