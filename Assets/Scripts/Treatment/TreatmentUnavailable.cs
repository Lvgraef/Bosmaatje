using UnityEngine;

namespace Treatment
{
    public class TreatmentUnavailable : MonoBehaviour
    {
        public void Close()
        {
            Destroy(gameObject);
        }
    }
}