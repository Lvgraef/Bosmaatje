using UnityEngine;
using UnityEngine.SceneManagement;

namespace Introduction
{
    public class NextButton : MonoBehaviour
    {
        public async void Next()
        {
            await SceneManager.LoadSceneAsync("Scenes/TreatmentPlan");
        }
    }
}
