using UnityEngine;
using UnityEngine.SceneManagement;

namespace Treatment
{
    public class TreatmentConfigure : MonoBehaviour
    {
        public void Configure()
        {
            SceneManager.LoadScene("Configuration");
        }
        
        public void Close()
        {
            Destroy(gameObject);
        }
    }
}