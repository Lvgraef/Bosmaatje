using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Util
{
    public class CharacterSelector : MonoBehaviour
    {
        public List<CharacterKeyValue> characters;
        public int selectedCharacter;

        public void Next()
        {
            selectedCharacter = (selectedCharacter + 1) % characters.Count;
            for (var i = 0; i < characters.Count; i++)
            {
                characters[i].character.SetActive(i == selectedCharacter);
            }
        }
        
        public void Previous()
        {
            selectedCharacter = (selectedCharacter - 1 + characters.Count) % characters.Count;
            for (var i = 0; i < characters.Count; i++)
            {
                characters[i].character.SetActive(i == selectedCharacter);
            }
        }

        public void SetCharacter(string name)
        {
            selectedCharacter = characters.FindIndex(character => character.name == name);
        }
    }
    
    [Serializable]
    public struct CharacterKeyValue
    {
        public string name;
        public GameObject character;
        public GameObject prefab;
    }
}