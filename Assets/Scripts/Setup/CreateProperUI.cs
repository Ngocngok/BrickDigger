using UnityEngine;
using UnityEditor;
using BrickDigger;
using UnityEngine.UI;
using TMPro;

public static class CreateProperUI
{
    [MenuItem("BrickDigger/Create Proper UI")]
    public static void CreateUI()
    {
        // Find or create canvas
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("UICanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }
        
        // Clear existing HUD if any
        Transform oldHud = canvas.transform.Find("HUDPanel");
        if (oldHud != null)
            Object.DestroyImmediate(oldHud.gameObject);
        
        // Create HUD Panel
        GameObject hudPanel = CreatePanel("HUDPanel", canvas.transform);
        
        // Create simple text displays using Text component (not TMP)
        CreateSimpleText("AxesText", hudPanel.transform, "Axes: 25", 
            new Vector2(0, 1), new Vector2(20, -20));
        CreateSimpleText("CoinsText", hudPanel.transform, "Coins: 0 (0)", 
            new Vector2(0, 1), new Vector2(20, -60));
        CreateSimpleText("PieceProgressText", hudPanel.transform, "Piece: 0/4", 
            new Vector2(0.5f, 1), new Vector2(0, -20));
        
        // Create Mobile Controls
        CreateJoystick(hudPanel.transform);
        CreateControlButton("JumpButton", hudPanel.transform, "JUMP", 
            new Vector2(-200, 150), Color.blue);
        CreateControlButton("DigButton", hudPanel.transform, "DIG", 
            new Vector2(-30, 150), new Color(0.8f, 0.2f, 0.2f));
        
        // Create Win Panel
        CreateWinLosePanel("WinPanel", canvas.transform, "LEVEL COMPLETE!", true);
        CreateWinLosePanel("LosePanel", canvas.transform, "OUT OF AXES!", false);
        
        Debug.Log("Proper UI Created!");
    }
    
    private static GameObject CreatePanel(string name, Transform parent)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent, false);
        RectTransform rect = panel.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        return panel;
    }
    
    private static GameObject CreateSimpleText(string name, Transform parent, string text, 
        Vector2 anchor, Vector2 position)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent, false);
        
        RectTransform rect = textObj.AddComponent<RectTransform>();
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.pivot = new Vector2(0, 1);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(300, 40);
        
        // Use regular Text component instead of TMP
        Text textComp = textObj.AddComponent<Text>();
        textComp.text = text;
        textComp.font = Font.CreateDynamicFontFromOSFont("Arial", 24);
        textComp.fontSize = 24;
        textComp.color = Color.white;
        textComp.alignment = TextAnchor.MiddleLeft;
        
        // Add outline for better visibility
        Outline outline = textObj.AddComponent<Outline>();
        outline.effectColor = Color.black;
        outline.effectDistance = new Vector2(2, -2);
        
        return textObj;
    }
    
    private static GameObject CreateJoystick(Transform parent)
    {
        GameObject joystickObj = new GameObject("Joystick");
        joystickObj.transform.SetParent(parent, false);
        
        RectTransform rect = joystickObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);
        rect.pivot = new Vector2(0, 0);
        rect.anchoredPosition = new Vector2(150, 150);
        rect.sizeDelta = new Vector2(250, 250);
        
        Image bgImage = joystickObj.AddComponent<Image>();
        bgImage.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
        
        // Create handle
        GameObject handle = new GameObject("Handle");
        handle.transform.SetParent(joystickObj.transform, false);
        RectTransform handleRect = handle.AddComponent<RectTransform>();
        handleRect.anchorMin = new Vector2(0.5f, 0.5f);
        handleRect.anchorMax = new Vector2(0.5f, 0.5f);
        handleRect.pivot = new Vector2(0.5f, 0.5f);
        handleRect.anchoredPosition = Vector2.zero;
        handleRect.sizeDelta = new Vector2(80, 80);
        
        Image handleImage = handle.AddComponent<Image>();
        handleImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        
        joystickObj.AddComponent<MobileJoystick>();
        
        return joystickObj;
    }
    
    private static GameObject CreateControlButton(string name, Transform parent, 
        string text, Vector2 position, Color color)
    {
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(parent, false);
        
        RectTransform rect = buttonObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 0);
        rect.anchorMax = new Vector2(1, 0);
        rect.pivot = new Vector2(1, 0);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(150, 150);
        
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = new Color(color.r, color.g, color.b, 0.7f);
        
        Button button = buttonObj.AddComponent<Button>();
        
        // Add text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);
        
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        Text textComp = textObj.AddComponent<Text>();
        textComp.text = text;
        textComp.font = Font.CreateDynamicFontFromOSFont("Arial", 28);
        textComp.fontSize = 28;
        textComp.fontStyle = FontStyle.Bold;
        textComp.color = Color.white;
        textComp.alignment = TextAnchor.MiddleCenter;
        
        Outline outline = textObj.AddComponent<Outline>();
        outline.effectColor = Color.black;
        
        return buttonObj;
    }
    
    private static GameObject CreateWinLosePanel(string name, Transform parent, 
        string title, bool isWin)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent, false);
        
        RectTransform rect = panel.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        
        Image bg = panel.AddComponent<Image>();
        bg.color = new Color(0, 0, 0, 0.8f);
        
        // Content panel
        GameObject content = new GameObject("Content");
        content.transform.SetParent(panel.transform, false);
        
        RectTransform contentRect = content.AddComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0.5f, 0.5f);
        contentRect.anchorMax = new Vector2(0.5f, 0.5f);
        contentRect.pivot = new Vector2(0.5f, 0.5f);
        contentRect.anchoredPosition = Vector2.zero;
        contentRect.sizeDelta = new Vector2(500, 400);
        
        Image contentBg = content.AddComponent<Image>();
        contentBg.color = new Color(0.2f, 0.2f, 0.2f, 0.95f);
        
        // Title text
        GameObject titleObj = new GameObject("TitleText");
        titleObj.transform.SetParent(content.transform, false);
        
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 1);
        titleRect.anchorMax = new Vector2(0.5f, 1);
        titleRect.pivot = new Vector2(0.5f, 1);
        titleRect.anchoredPosition = new Vector2(0, -30);
        titleRect.sizeDelta = new Vector2(400, 60);
        
        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = title;
        titleText.font = Font.CreateDynamicFontFromOSFont("Arial", 36);
        titleText.fontSize = 36;
        titleText.fontStyle = FontStyle.Bold;
        titleText.color = isWin ? Color.green : Color.red;
        titleText.alignment = TextAnchor.MiddleCenter;
        
        // Create buttons
        if (isWin)
        {
            CreatePanelButton("NextButton", content.transform, "Next Level", 
                new Vector2(0, -50), new Color(0.2f, 0.7f, 0.2f));
            CreatePanelButton("RetryButton", content.transform, "Retry", 
                new Vector2(0, -120), new Color(0.7f, 0.7f, 0.2f));
        }
        else
        {
            CreatePanelButton("RetryButton", content.transform, "Retry", 
                new Vector2(0, -50), new Color(0.7f, 0.7f, 0.2f));
            CreatePanelButton("BuyAxesButton", content.transform, "Buy Axes (5 coins)", 
                new Vector2(0, -120), new Color(0.2f, 0.5f, 0.7f));
        }
        
        panel.SetActive(false);
        return panel;
    }
    
    private static GameObject CreatePanelButton(string name, Transform parent, 
        string text, Vector2 position, Color color)
    {
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(parent, false);
        
        RectTransform rect = buttonObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(250, 50);
        
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = color;
        
        Button button = buttonObj.AddComponent<Button>();
        
        // Add text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);
        
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        Text textComp = textObj.AddComponent<Text>();
        textComp.text = text;
        textComp.font = Font.CreateDynamicFontFromOSFont("Arial", 20);
        textComp.fontSize = 20;
        textComp.color = Color.white;
        textComp.alignment = TextAnchor.MiddleCenter;
        
        return buttonObj;
    }
}