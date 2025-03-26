using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Messages
{
    public class Message : MonoBehaviour
    {
        public TextMeshProUGUI typeText;
        public TextMeshProUGUI messageText;
        
        public void OpenRandom()
        {
            var message = MessageData.Messages[Random.Range(0, MessageData.Messages.Count)];
            typeText.text = message.Type;
            messageText.text = message.Message;
        }
    }
    
    public class MessageData
    {
        public static readonly List<MessageData> Messages = new()
        {
            new MessageData("Tip", "Je mag je knuffel meenemen naar het ziekenhuis!"),
            new MessageData("Advies", "Wanneer je veel zorgen ervaart, praat hier dan over met iemand die je vertrouwt."),
            new MessageData("Herstel", "Wanneer je suiker laag is moet je een snoepje eten, dit kan je natuurlijk ook gezellig delen met klasgenootjes!"),
        };
        
        public string Type { get; }
        public string Message { get; }

        private MessageData(string type, string message)
        {
            Type = type;
            Message = message;
        }
    }
}