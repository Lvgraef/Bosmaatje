using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class CharacterSelector : MonoBehaviour
    {
        public List<GameObject> characters;
        public int selectedCharacter;

        public void Next()
        {
            selectedCharacter = (selectedCharacter + 1) % characters.Count;
            for (var i = 0; i < characters.Count; i++)
            {
                characters[i].SetActive(i == selectedCharacter);
            }
        }
        
        public void Previous()
        {
            selectedCharacter = (selectedCharacter - 1 + characters.Count) % characters.Count;
            for (var i = 0; i < characters.Count; i++)
            {
                characters[i].SetActive(i == selectedCharacter);
            }
        }
    }
}