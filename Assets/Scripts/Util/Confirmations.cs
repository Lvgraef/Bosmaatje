using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Util
{
    public static class Confirmations
    {

        public static void CreateConfirmationPopup(Action MethodByDiscart, Action MethodBySaveFirst, string text, CanvasGroup FreezeWhileConfirm)
        {
            // Freeze the game while the confirmation is active
            FreezeWhileConfirm.interactable = false;

            // Create Canvas
            GameObject canvasGO = new GameObject("Canvas");
            Canvas canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay; // Ensure UI is visible
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
            // Create Image
            GameObject imageGO = new GameObject("Image");
            Image image = imageGO.AddComponent<Image>();
            // Set Parent
            imageGO.transform.SetParent(canvasGO.transform, false);
            // Set Image Size
            RectTransform rectTransform = imageGO.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(800, 400);
            rectTransform.anchoredPosition = Vector2.zero;

            // Create Text
            GameObject textGO = new GameObject("Text");
            Text textui = textGO.AddComponent<Text>();
            textui.text = text;
            textui.alignment = TextAnchor.MiddleCenter;
            textui.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            textui.color = Color.black;

            // Set Parent and Position for Text
            textGO.transform.SetParent(imageGO.transform, false);
            RectTransform textRect = textGO.GetComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(600, 200);
            textRect.anchoredPosition = Vector2.zero;

            // Create Buttons
            GameObject buttonSave = CreateButton("SaveButton", new Vector2(-150, -200), false);
            GameObject buttonDiscart = CreateButton("DiscartButton", new Vector2(150, -200), true);
            buttonSave.transform.SetParent(canvasGO.transform, false);
            buttonDiscart.transform.SetParent(canvasGO.transform, false);

            // Add Listeners
            Button saveButton = buttonSave.GetComponent<Button>();
            saveButton.onClick.AddListener(() =>
            {
                MethodBySaveFirst();
                GameObject.Destroy(canvasGO);
                FreezeWhileConfirm.interactable = true;
            });

            Button discartButton = buttonDiscart.GetComponent<Button>();

            discartButton.onClick.AddListener(() =>
            {
                MethodByDiscart();
                GameObject.Destroy(canvasGO);
                FreezeWhileConfirm.interactable = true;
            });

        }

        private static GameObject CreateButton(string name, Vector2 position, bool isDiscart)
        {
            GameObject buttonGO = new GameObject(name);
            Button button = buttonGO.AddComponent<Button>();
            Image buttonImage = buttonGO.AddComponent<Image>();
            buttonImage.color = isDiscart ? Color.red : Color.green;

            // Create Button Text
            GameObject textGO = new GameObject("Text");
            Text text = textGO.AddComponent<Text>();
            text.text = isDiscart ? "wegdoen" : "opslaan";
            text.alignment = TextAnchor.MiddleCenter;
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.color = Color.black;

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
