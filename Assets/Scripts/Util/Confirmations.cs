using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Util
{
    public static class Confirmations
    {
        public static void CreateConfirmationPopup(Action MethodByDiscart, Action MethodBySaveFirst, string text, CanvasGroup FreezeWhileConfirm)
        {
            Debug.Log("CreateConfirmationPopup");
            // Gebruik de default tekst als deze niet wordt opgegeven
            string defaultDenytext = "wegdoen";
            string defaultConfirmText = "opslaan";
 
            // Call de originele methode met de default tekst
            CreateConfirmationPopup(MethodByDiscart, MethodBySaveFirst, text, FreezeWhileConfirm, defaultDenytext, defaultConfirmText);
        }
 
        public static void CreateConfirmationPopup(Action MethodByDeny, Action MethodByConfirm, string popupMessage, CanvasGroup FreezeWhileConfirm, string denyMessage, string confirmMessage)
        {
            Debug.Log("CreateConfirmationPopup");
            // Freeze the game while the confirmation is active
            FreezeWhileConfirm.interactable = false;
 
            // Create Canvas
            GameObject canvasGO = new GameObject("Canvas");

            Canvas canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay; // Ensure UI is visible
            canvas.sortingOrder = 5;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();

            // set canvas order to 5:
            canvas.sortingOrder = 5;

            // Create Image
            GameObject imageGO = new GameObject("Image");
            Image image = imageGO.AddComponent<Image>();
            image.color = new Color(0.4f, 0.29f, 0.23f) ;
            // Set Parent
            imageGO.transform.SetParent(canvasGO.transform, false);
            // Set Image Size
            RectTransform rectTransform = imageGO.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(800, 400);
            rectTransform.anchoredPosition = Vector2.zero;
 
            // Create Text
            GameObject textGO = new GameObject("TextGo");
            TextMeshProUGUI textui = textGO.AddComponent<TextMeshProUGUI>();
            textui.alignment = TextAlignmentOptions.Center;
            textui.text = popupMessage;
            textui.color = new Color(210 / 255f, 196/255f, 157/255f);
 
            // Set Parent and Position for Text
            textGO.transform.SetParent(imageGO.transform, false);
            RectTransform textRect = textGO.GetComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(600, 200);
            textRect.anchoredPosition = Vector2.zero;
 
            // Create Buttons
            GameObject buttonSave = CreateButton("SaveButton", new Vector2(-150, -200), false, denyMessage, confirmMessage);
            GameObject buttonDiscart = CreateButton("DiscartButton", new Vector2(150, -200), true, denyMessage, confirmMessage);
            buttonSave.transform.SetParent(canvasGO.transform, false);
            buttonDiscart.transform.SetParent(canvasGO.transform, false);
 
            // Add Listeners
            Button saveButton = buttonSave.GetComponent<Button>();
            saveButton.onClick.AddListener(() =>
            {
                MethodByConfirm();
                GameObject.Destroy(canvasGO);
                FreezeWhileConfirm.interactable = true;
            });
 
            Button discartButton = buttonDiscart.GetComponent<Button>();
 
            discartButton.onClick.AddListener(() =>
            {
                MethodByDeny();
                GameObject.Destroy(canvasGO);
                FreezeWhileConfirm.interactable = true;
            });
 
        }
 
        private static GameObject CreateButton(string name, Vector2 position, bool isDiscart, string denyMessage, string confirmMessage)
        {
            GameObject buttonGO = new GameObject(name);
            Button button = buttonGO.AddComponent<Button>();
            Image buttonImage = buttonGO.AddComponent<Image>();
            buttonImage.color = isDiscart ? Color.red : Color.green;
 
            // Create Button Text
            GameObject textGO = new GameObject("TextGO");
            TextMeshProUGUI tmp = textGO.AddComponent<TextMeshProUGUI>();
 
            //textGO.transform.position = new Vector3(0, 0, 0);
 
            tmp.text = isDiscart ? denyMessage : confirmMessage;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.color = Color.black;
 
            // Set Parent and Position
            textGO.transform.SetParent(buttonGO.transform, false);
            RectTransform textRect = textGO.GetComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(160, 50);
            textRect.anchoredPosition = Vector2.zero;
 
            // Set Button Size and Position
            RectTransform buttonRect = buttonGO.GetComponent<RectTransform>();
            buttonRect.sizeDelta = new Vector2(200, 60);
            buttonRect.anchoredPosition = position;
 
            return buttonGO;
        }
    }
}