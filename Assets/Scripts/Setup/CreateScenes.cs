using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using BrickDigger;

public static class CreateScenes
{
    [MenuItem("BrickDigger/Create All Scenes")]
    public static void CreateAllScenes()
    {
        CreateLoadingScene();
        CreateHomeScene();
        RenameGameplayScene();
        AddScenesToBuildSettings();
        
        Debug.Log("All scenes created!");
    }
    
    private static void CreateLoadingScene()
    {
        // Create new scene
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        
        // Create Canvas
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // Create background
        GameObject bg = new GameObject("Background");
        bg.transform.SetParent(canvasObj.transform, false);
        RectTransform bgRect = bg.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
        Image bgImage = bg.AddComponent<Image>();
        bgImage.color = new Color(0.1f, 0.1f, 0.15f);
        
        // Create title text
        GameObject titleObj = new GameObject("TitleText");
        titleObj.transform.SetParent(canvasObj.transform, false);
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 0.7f);
        titleRect.anchorMax = new Vector2(0.5f, 0.7f);
        titleRect.pivot = new Vector2(0.5f, 0.5f);
        titleRect.anchoredPosition = Vector2.zero;
        titleRect.sizeDelta = new Vector2(400, 80);
        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "BRICK DIGGER 3D";
        titleText.font = Font.CreateDynamicFontFromOSFont("Arial", 48);
        titleText.fontSize = 48;
        titleText.fontStyle = FontStyle.Bold;
        titleText.color = Color.white;
        titleText.alignment = TextAnchor.MiddleCenter;
        
        // Create loading bar background
        GameObject barBg = new GameObject("LoadingBarBG");
        barBg.transform.SetParent(canvasObj.transform, false);
        RectTransform barBgRect = barBg.AddComponent<RectTransform>();
        barBgRect.anchorMin = new Vector2(0.5f, 0.4f);
        barBgRect.anchorMax = new Vector2(0.5f, 0.4f);
        barBgRect.pivot = new Vector2(0.5f, 0.5f);
        barBgRect.anchoredPosition = Vector2.zero;
        barBgRect.sizeDelta = new Vector2(500, 40);
        Image barBgImage = barBg.AddComponent<Image>();
        barBgImage.color = new Color(0.2f, 0.2f, 0.2f);
        
        // Create loading bar fill
        GameObject barFill = new GameObject("LoadingBarFill");
        barFill.transform.SetParent(barBg.transform, false);
        RectTransform barFillRect = barFill.AddComponent<RectTransform>();
        barFillRect.anchorMin = Vector2.zero;
        barFillRect.anchorMax = Vector2.one;
        barFillRect.offsetMin = Vector2.zero;
        barFillRect.offsetMax = Vector2.zero;
        Image barFillImage = barFill.AddComponent<Image>();
        barFillImage.color = new Color(0.2f, 0.7f, 0.3f);
        barFillImage.type = Image.Type.Filled;
        barFillImage.fillMethod = Image.FillMethod.Horizontal;
        barFillImage.fillAmount = 0f;
        
        // Create percentage text
        GameObject percentObj = new GameObject("PercentageText");
        percentObj.transform.SetParent(canvasObj.transform, false);
        RectTransform percentRect = percentObj.AddComponent<RectTransform>();
        percentRect.anchorMin = new Vector2(0.5f, 0.3f);
        percentRect.anchorMax = new Vector2(0.5f, 0.3f);
        percentRect.pivot = new Vector2(0.5f, 0.5f);
        percentRect.anchoredPosition = Vector2.zero;
        percentRect.sizeDelta = new Vector2(200, 50);
        Text percentText = percentObj.AddComponent<Text>();
        percentText.text = "0%";
        percentText.font = Font.CreateDynamicFontFromOSFont("Arial", 32);
        percentText.fontSize = 32;
        percentText.color = Color.white;
        percentText.alignment = TextAnchor.MiddleCenter;
        
        // Add LoadingScene component
        GameObject manager = new GameObject("LoadingManager");
        LoadingScene loadingScene = manager.AddComponent<LoadingScene>();
        var lsType = loadingScene.GetType();
        lsType.GetField("loadingBar", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(loadingScene, barFillImage);
        lsType.GetField("percentageText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(loadingScene, percentText);
        
        // Save scene
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/LoadingScene.unity");
    }
    
    private static void CreateHomeScene()
    {
        // Create new scene
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        
        // Create Canvas
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // Create background
        GameObject bg = new GameObject("Background");
        bg.transform.SetParent(canvasObj.transform, false);
        RectTransform bgRect = bg.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
        Image bgImage = bg.AddComponent<Image>();
        bgImage.color = new Color(0.15f, 0.15f, 0.2f);
        
        // Create title
        GameObject titleObj = new GameObject("TitleText");
        titleObj.transform.SetParent(canvasObj.transform, false);
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 0.8f);
        titleRect.anchorMax = new Vector2(0.5f, 0.8f);
        titleRect.pivot = new Vector2(0.5f, 0.5f);
        titleRect.anchoredPosition = Vector2.zero;
        titleRect.sizeDelta = new Vector2(500, 100);
        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "BRICK DIGGER 3D";
        titleText.font = Font.CreateDynamicFontFromOSFont("Arial", 56);
        titleText.fontSize = 56;
        titleText.fontStyle = FontStyle.Bold;
        titleText.color = new Color(1f, 0.8f, 0.2f);
        titleText.alignment = TextAnchor.MiddleCenter;
        
        // Create level text
        GameObject levelObj = new GameObject("LevelText");
        levelObj.transform.SetParent(canvasObj.transform, false);
        RectTransform levelRect = levelObj.AddComponent<RectTransform>();
        levelRect.anchorMin = new Vector2(0.5f, 0.6f);
        levelRect.anchorMax = new Vector2(0.5f, 0.6f);
        levelRect.pivot = new Vector2(0.5f, 0.5f);
        levelRect.anchoredPosition = Vector2.zero;
        levelRect.sizeDelta = new Vector2(300, 60);
        Text levelText = levelObj.AddComponent<Text>();
        levelText.text = "Level 1";
        levelText.font = Font.CreateDynamicFontFromOSFont("Arial", 40);
        levelText.fontSize = 40;
        levelText.color = Color.white;
        levelText.alignment = TextAnchor.MiddleCenter;
        
        // Create coins text
        GameObject coinsObj = new GameObject("CoinsText");
        coinsObj.transform.SetParent(canvasObj.transform, false);
        RectTransform coinsRect = coinsObj.AddComponent<RectTransform>();
        coinsRect.anchorMin = new Vector2(0.5f, 0.5f);
        coinsRect.anchorMax = new Vector2(0.5f, 0.5f);
        coinsRect.pivot = new Vector2(0.5f, 0.5f);
        coinsRect.anchoredPosition = Vector2.zero;
        coinsRect.sizeDelta = new Vector2(300, 50);
        Text coinsText = coinsObj.AddComponent<Text>();
        coinsText.text = "Coins: 0";
        coinsText.font = Font.CreateDynamicFontFromOSFont("Arial", 32);
        coinsText.fontSize = 32;
        coinsText.color = new Color(1f, 0.84f, 0f);
        coinsText.alignment = TextAnchor.MiddleCenter;
        
        // Create play button
        GameObject playBtn = new GameObject("PlayButton");
        playBtn.transform.SetParent(canvasObj.transform, false);
        RectTransform playRect = playBtn.AddComponent<RectTransform>();
        playRect.anchorMin = new Vector2(0.5f, 0.3f);
        playRect.anchorMax = new Vector2(0.5f, 0.3f);
        playRect.pivot = new Vector2(0.5f, 0.5f);
        playRect.anchoredPosition = Vector2.zero;
        playRect.sizeDelta = new Vector2(300, 80);
        Image playImage = playBtn.AddComponent<Image>();
        playImage.color = new Color(0.2f, 0.7f, 0.3f);
        Button playButton = playBtn.AddComponent<Button>();
        
        GameObject playTextObj = new GameObject("Text");
        playTextObj.transform.SetParent(playBtn.transform, false);
        RectTransform playTextRect = playTextObj.AddComponent<RectTransform>();
        playTextRect.anchorMin = Vector2.zero;
        playTextRect.anchorMax = Vector2.one;
        playTextRect.offsetMin = Vector2.zero;
        playTextRect.offsetMax = Vector2.zero;
        Text playText = playTextObj.AddComponent<Text>();
        playText.text = "PLAY";
        playText.font = Font.CreateDynamicFontFromOSFont("Arial", 48);
        playText.fontSize = 48;
        playText.fontStyle = FontStyle.Bold;
        playText.color = Color.white;
        playText.alignment = TextAnchor.MiddleCenter;
        
        // Add HomeScene component
        GameObject manager = new GameObject("HomeManager");
        HomeScene homeScene = manager.AddComponent<HomeScene>();
        var hsType = homeScene.GetType();
        hsType.GetField("levelText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(homeScene, levelText);
        hsType.GetField("coinsText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(homeScene, coinsText);
        hsType.GetField("playButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(homeScene, playButton);
        
        // Create EventSystem
        GameObject eventSystem = new GameObject("EventSystem");
        eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
        eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        
        // Save scene
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/HomeScene.unity");
    }
    
    private static void RenameGameplayScene()
    {
        // Rename current game scene to GameplayScene
        string currentScenePath = "Assets/Scenes/SampleScene.unity";
        string newScenePath = "Assets/Scenes/GameplayScene.unity";
        
        if (System.IO.File.Exists(currentScenePath))
        {
            AssetDatabase.MoveAsset(currentScenePath, newScenePath);
        }
        else if (System.IO.File.Exists("Assets/BrickDiggerGame.unity"))
        {
            AssetDatabase.MoveAsset("Assets/BrickDiggerGame.unity", newScenePath);
        }
    }
    
    private static void AddScenesToBuildSettings()
    {
        // Get current build settings
        var scenes = new System.Collections.Generic.List<EditorBuildSettingsScene>();
        
        // Add scenes in order
        scenes.Add(new EditorBuildSettingsScene("Assets/Scenes/LoadingScene.unity", true));
        scenes.Add(new EditorBuildSettingsScene("Assets/Scenes/HomeScene.unity", true));
        scenes.Add(new EditorBuildSettingsScene("Assets/Scenes/GameplayScene.unity", true));
        
        // Update build settings
        EditorBuildSettings.scenes = scenes.ToArray();
        
        Debug.Log("Scenes added to build settings!");
    }
}