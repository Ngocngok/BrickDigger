using UnityEngine;
using UnityEditor;
using BrickDigger;
using UnityEngine.UI;
using TMPro;

public static class FixUI
{
    [MenuItem("BrickDigger/Fix UI Text")]
    public static void FixUIText()
    {
        // Find all TextMeshProUGUI components
        TextMeshProUGUI[] texts = Object.FindObjectsOfType<TextMeshProUGUI>();
        
        foreach (var text in texts)
        {
            // Ensure text is visible
            text.color = Color.white;
            text.fontSize = 24;
            text.fontStyle = FontStyles.Bold;
            text.alignment = TextAlignmentOptions.Center;
            
            // Set default text values
            if (text.gameObject.name == "AxesText")
                text.text = "Axes: 25";
            else if (text.gameObject.name == "CoinsText")
                text.text = "Coins: 0 (0)";
            else if (text.gameObject.name == "PieceProgressText")
                text.text = "Piece: 0/4";
            else if (text.gameObject.name == "Text" && text.transform.parent.name == "JumpButton")
                text.text = "JUMP";
            else if (text.gameObject.name == "Text" && text.transform.parent.name == "DigButton")
                text.text = "DIG";
                
            EditorUtility.SetDirty(text);
        }
        
        // Fix button images
        Button[] buttons = Object.FindObjectsOfType<Button>();
        foreach (var button in buttons)
        {
            Image image = button.GetComponent<Image>();
            if (image != null)
            {
                if (button.name == "JumpButton" || button.name == "DigButton")
                {
                    image.color = new Color(0.2f, 0.5f, 0.8f, 0.8f); // Blue buttons
                }
            }
            EditorUtility.SetDirty(button);
        }
        
        // Fix joystick visibility
        MobileJoystick joystick = Object.FindObjectOfType<MobileJoystick>();
        if (joystick != null)
        {
            Image joystickBg = joystick.GetComponent<Image>();
            if (joystickBg != null)
            {
                joystickBg.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
                EditorUtility.SetDirty(joystickBg);
            }
            
            Transform handle = joystick.transform.Find("Handle");
            if (handle != null)
            {
                Image handleImg = handle.GetComponent<Image>();
                if (handleImg != null)
                {
                    handleImg.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
                    EditorUtility.SetDirty(handleImg);
                }
            }
        }
        
        Debug.Log("UI Text Fixed!");
    }
    
    [MenuItem("BrickDigger/Setup UI Layout")]
    public static void SetupUILayout()
    {
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null) return;
        
        // Find HUD Panel
        Transform hudPanel = canvas.transform.Find("HUDPanel");
        if (hudPanel == null) return;
        
        // Position text elements at top
        Transform axesText = hudPanel.Find("AxesText");
        if (axesText != null)
        {
            RectTransform rect = axesText.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
            rect.pivot = new Vector2(0, 1);
            rect.anchoredPosition = new Vector2(20, -20);
            rect.sizeDelta = new Vector2(300, 40);
        }
        
        Transform coinsText = hudPanel.Find("CoinsText");
        if (coinsText != null)
        {
            RectTransform rect = coinsText.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
            rect.pivot = new Vector2(0, 1);
            rect.anchoredPosition = new Vector2(20, -60);
            rect.sizeDelta = new Vector2(300, 40);
        }
        
        Transform pieceText = hudPanel.Find("PieceProgressText");
        if (pieceText != null)
        {
            RectTransform rect = pieceText.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 1);
            rect.anchorMax = new Vector2(0.5f, 1);
            rect.pivot = new Vector2(0.5f, 1);
            rect.anchoredPosition = new Vector2(0, -20);
            rect.sizeDelta = new Vector2(300, 40);
        }
        
        // Position mobile controls at bottom
        Transform joystick = hudPanel.Find("Joystick");
        if (joystick != null)
        {
            RectTransform rect = joystick.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 0);
            rect.anchorMax = new Vector2(0, 0);
            rect.pivot = new Vector2(0, 0);
            rect.anchoredPosition = new Vector2(150, 150);
            rect.sizeDelta = new Vector2(250, 250);
        }
        
        Transform jumpButton = hudPanel.Find("JumpButton");
        if (jumpButton != null)
        {
            RectTransform rect = jumpButton.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(1, 0);
            rect.anchorMax = new Vector2(1, 0);
            rect.pivot = new Vector2(1, 0);
            rect.anchoredPosition = new Vector2(-200, 150);
            rect.sizeDelta = new Vector2(150, 150);
        }
        
        Transform digButton = hudPanel.Find("DigButton");
        if (digButton != null)
        {
            RectTransform rect = digButton.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(1, 0);
            rect.anchorMax = new Vector2(1, 0);
            rect.pivot = new Vector2(1, 0);
            rect.anchoredPosition = new Vector2(-30, 150);
            rect.sizeDelta = new Vector2(150, 150);
        }
        
        Debug.Log("UI Layout Setup Complete!");
    }
}