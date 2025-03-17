using UnityEngine;

public class CloseSettings : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void CloseOnButtonClick(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
