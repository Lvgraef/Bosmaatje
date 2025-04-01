using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Util
{
    public class RadioButton : MonoBehaviour
    {
        public List<GameObject> radioButtonSelects;
        public int selectedButton = -1;
        
        public void SelectButton(int index)
        {
            selectedButton = index;
            for (var i = 0; i < radioButtonSelects.Count; i++)
            {
                radioButtonSelects[i].SetActive(i == index);
            }
        }

        public void SetEnabled(bool enable)
        {
            foreach (var componentsInChild in GetComponentsInChildren<Button>())
            {
                componentsInChild.interactable = enable;
            }
        }
    }
}
