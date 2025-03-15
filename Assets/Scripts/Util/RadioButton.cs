using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class RadioButton : MonoBehaviour
    {
        public List<GameObject> radioButtonSelects;
        public int selectedButton;
        
        public void SelectButton(int index)
        {
            selectedButton = index;
            for (var i = 0; i < radioButtonSelects.Count; i++)
            {
                radioButtonSelects[i].SetActive(i == index);
            }
        }
    }
}
