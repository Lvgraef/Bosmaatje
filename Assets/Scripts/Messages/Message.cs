﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Messages
{
    public class Message : MonoBehaviour
    {
        public TextMeshProUGUI typeText;
        public TextMeshProUGUI messageText;
        
        public void OpenRandom(Transform canvas)
        {
            var messageObj = Instantiate(gameObject, canvas);
            var message = MessageData.Messages[Random.Range(0, MessageData.Messages.Count)];
            var messageScript = messageObj.GetComponent<Message>();
            messageScript.typeText.text = message.Type;
            messageScript.messageText.text = message.Message;
        }

        public void Close()
        {
            Destroy(gameObject);
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