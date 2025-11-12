using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using BrickDigger;
using UnityEditor.SceneManagement;

public static class AddPauseMenu
{
    [MenuItem("BrickDigger/Add Pause Menu to Gameplay")]
    public static void AddPauseMenuToGameplay()
    {
        // Open gameplay scene
        EditorSceneManager.OpenScene("Assets/Scenes/GameplayScene.unity");
        
        // Find canvas
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas not found!");
            return;
        }
        
        // Find HUD Panel
        Transform hudPanel = canvas.transform.Find("HUDPanel");
        if (hudPanel == null)
        {
            Debug.LogError("HUDPanel not found!");
            return;
        }
        
        // Create Pause Button (top-right corner)
        GameObject pauseBtn = new GameObject("PauseButton");
        pauseBtn.transform.SetParent(hudPanel, false);
        
        RectTransform pauseRect = pauseBtn.AddComponent<RectTransform>();
        pauseRect.anchorMin = new Vector2(1, 1);
        pauseRect.anchorMax = new Vector2(1, 1);
        pauseRect.pivot = new Vector2(1, 1);
        pauseRect.anchoredPosition = new Vector2(-20, -20);
        pauseRect.sizeDelta = new Vector2(80, 80);
        
        Image pauseImage = pauseBtn.AddComponent<Image>();
        pauseImage.color = new Color(0.3f, 0.3f, 0.3f, 0.8f);
        
        Button pauseButton = pauseBtn.AddComponent<Button>();
        
        // Add pause icon text
        GameObject pauseTextObj = new GameObject("Text");
        pauseTextObj.transform.SetParent(pauseBtn.transform, false);
        RectTransform pauseTextRect = pauseTextObj.AddComponent<RectTransform>();
        pauseTextRect.anchorMin = Vector2.zero;
        pauseTextRect.anchorMax = Vector2.one;
        pauseTextRect.offsetMin = Vector2.zero;
        pauseTextRect.offsetMax = Vector2.zero;
        
        Text pauseText = pauseTextObj.AddComponent<Text>();
        pauseText.text = "||";
        pauseText.font = Font.CreateDynamicFontFromOSFont("Arial", 40);
        pauseText.fontSize = 40;
        pauseText.fontStyle = FontStyle.Bold;
        pauseText.color = Color.white;
        pauseText.alignment = TextAnchor.MiddleCenter;
        
        // Create Pause Panel
        GameObject pausePanel = new GameObject("PausePanel");
        pausePanel.transform.SetParent(canvas.transform, false);
        
        RectTransform pausePanelRect = pausePanel.AddComponent<RectTransform>();
        pausePanelRect.anchorMin = Vector2.zero;
        pausePanelRect.anchorMax = Vector2.one;
        pausePanelRect.offsetMin = Vector2.zero;
        pausePanelRect.offsetMax = Vector2.zero;
        
        Image pausePanelBg = pausePanel.AddComponent<Image>();
        pausePanelBg.color = new Color(0, 0, 0, 0.8f);
        
        // Create content panel
        GameObject contentPanel = new GameObject("ContentPanel");
        contentPanel.transform.SetParent(pausePanel.transform, false);
        
        RectTransform contentRect = contentPanel.AddComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0.5f, 0.5f);
        contentRect.anchorMax = new Vector2(0.5f, 0.5f);
        contentRect.pivot = new Vector2(0.5f, 0.5f);
        contentRect.anchoredPosition = Vector2.zero;
        contentRect.sizeDelta = new Vector2(500, 400);
        
        Image contentBg = contentPanel.AddComponent<Image>();
        contentBg.color = new Color(0.2f, 0.2f, 0.25f, 0.95f);
        
        // Create "PAUSED" title
        GameObject titleObj = new GameObject("TitleText");
        titleObj.transform.SetParent(contentPanel.transform, false);
        
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 1);
        titleRect.anchorMax = new Vector2(0.5f, 1);
        titleRect.pivot = new Vector2(0.5f, 1);
        titleRect.anchoredPosition = new Vector2(0, -40);
        titleRect.sizeDelta = new Vector2(400, 80);
        
        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "PAUSED";
        titleText.font = Font.CreateDynamicFontFromOSFont("Arial", 56);
        titleText.fontSize = 56;
        titleText.fontStyle = FontStyle.Bold;
        titleText.color = Color.white;
        titleText.alignment = TextAnchor.MiddleCenter;
        
        // Create Resume Button
        GameObject resumeBtn = CreatePanelButton("ResumeButton", contentPanel.transform, 
            "RESUME", new Vector2(0, 20), new Color(0.2f, 0.7f, 0.3f));
        Button resumeButton = resumeBtn.GetComponent<Button>();
        
        // Create Home Button
        GameObject homeBtn = CreatePanelButton("HomeButton", contentPanel.transform, 
            "BACK TO HOME", new Vector2(0, -80), new Color(0.7f, 0.3f, 0.2f));
        Button homeButton = homeBtn.GetComponent<Button>();
        
        // Create PauseMenu manager
        GameObject pauseManager = new GameObject("PauseManager");
        PauseMenu pauseMenu = pauseManager.AddComponent<PauseMenu>();
        
        // Connect references using reflection
        var pmType = pauseMenu.GetType();
        pmType.GetField("pausePanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(pauseMenu, pausePanel);
        pmType.GetField("resumeButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(pauseMenu, resumeButton);
        pmType.GetField("homeButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(pauseMenu, homeButton);
        pmType.GetField("pauseButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(pauseMenu, pauseButton);
        
        // Hide pause panel initially
        pausePanel.SetActive(false);
        
        // Save scene
        EditorSceneManager.SaveOpenScenes();
        
        Debug.Log("Pause menu added to gameplay scene!");
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
        rect.sizeDelta = new Vector2(350, 70);
        
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
        textComp.font = Font.CreateDynamicFontFromOSFont("Arial", 32);
        textComp.fontSize = 32;
        textComp.fontStyle = FontStyle.Bold;
        textComp.color = Color.white;
        textComp.alignment = TextAnchor.MiddleCenter;
        
        return buttonObj;
    }
}