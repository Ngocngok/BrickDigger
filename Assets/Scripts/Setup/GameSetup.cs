using UnityEngine;
using UnityEditor;
using BrickDigger;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public static class GameSetup
{
    [MenuItem("BrickDigger/Setup Game Scene")]
    public static void SetupGameScene()
    {
        // Create block prefabs first
        CreateBlockPrefabs();
        
        // Create game managers
        CreateGameManagers();
        
        // Create player
        CreatePlayer();
        
        // Setup camera
        SetupCamera();
        
        // Create UI
        CreateUI();
        
        // Create EventSystem
        CreateEventSystem();
        
        Debug.Log("Game scene setup complete!");
    }
    
    private static void CreateBlockPrefabs()
    {
        // Create prefabs folder
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs/Blocks"))
        {
            AssetDatabase.CreateFolder("Assets/Prefabs", "Blocks");
        }
        
        // Create materials folder
        if (!AssetDatabase.IsValidFolder("Assets/Materials"))
        {
            AssetDatabase.CreateFolder("Assets", "Materials");
        }
        
        // Get the appropriate shader based on render pipeline
        Shader defaultShader = Shader.Find("Standard");
        if (defaultShader == null) defaultShader = Shader.Find("Sprites/Default");
        if (defaultShader == null) defaultShader = Shader.Find("Legacy Shaders/Diffuse");
        
        // Create Dirt Block
        GameObject dirtBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
        dirtBlock.name = "DirtBlock";
        Material dirtMat = new Material(defaultShader);
        dirtMat.color = new Color(0.4f, 0.25f, 0.13f);
        AssetDatabase.CreateAsset(dirtMat, "Assets/Materials/DirtMaterial.mat");
        dirtBlock.GetComponent<Renderer>().material = dirtMat;
        PrefabUtility.SaveAsPrefabAsset(dirtBlock, "Assets/Prefabs/Blocks/DirtBlock.prefab");
        Object.DestroyImmediate(dirtBlock);
        
        // Create Bedrock Block
        GameObject bedrockBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bedrockBlock.name = "BedrockBlock";
        Material bedrockMat = new Material(defaultShader);
        bedrockMat.color = new Color(0.3f, 0.3f, 0.3f);
        AssetDatabase.CreateAsset(bedrockMat, "Assets/Materials/BedrockMaterial.mat");
        bedrockBlock.GetComponent<Renderer>().material = bedrockMat;
        PrefabUtility.SaveAsPrefabAsset(bedrockBlock, "Assets/Prefabs/Blocks/BedrockBlock.prefab");
        Object.DestroyImmediate(bedrockBlock);
        
        // Create Coin Block
        GameObject coinBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
        coinBlock.name = "CoinBlock";
        Material coinMat = new Material(defaultShader);
        coinMat.color = new Color(1f, 0.84f, 0f);
        if (coinMat.HasProperty("_Metallic")) coinMat.SetFloat("_Metallic", 0.8f);
        if (coinMat.HasProperty("_Smoothness")) coinMat.SetFloat("_Smoothness", 0.8f);
        if (coinMat.HasProperty("_Glossiness")) coinMat.SetFloat("_Glossiness", 0.8f);
        AssetDatabase.CreateAsset(coinMat, "Assets/Materials/CoinMaterial.mat");
        coinBlock.GetComponent<Renderer>().material = coinMat;
        PrefabUtility.SaveAsPrefabAsset(coinBlock, "Assets/Prefabs/Blocks/CoinBlock.prefab");
        Object.DestroyImmediate(coinBlock);
        
        // Create Lego Piece Block
        GameObject legoPiece = GameObject.CreatePrimitive(PrimitiveType.Cube);
        legoPiece.name = "LegoPiece";
        Material legoMat = new Material(defaultShader);
        legoMat.color = new Color(1f, 0f, 0f);
        if (legoMat.HasProperty("_Smoothness")) legoMat.SetFloat("_Smoothness", 0.7f);
        if (legoMat.HasProperty("_Glossiness")) legoMat.SetFloat("_Glossiness", 0.7f);
        AssetDatabase.CreateAsset(legoMat, "Assets/Materials/LegoMaterial.mat");
        legoPiece.GetComponent<Renderer>().material = legoMat;
        
        // Add studs
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                GameObject stud = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                stud.name = "Stud";
                stud.transform.SetParent(legoPiece.transform);
                stud.transform.localPosition = new Vector3(x * 0.25f, 0.6f, z * 0.25f);
                stud.transform.localScale = new Vector3(0.15f, 0.1f, 0.15f);
                stud.GetComponent<Renderer>().material = legoMat;
            }
        }
        
        PrefabUtility.SaveAsPrefabAsset(legoPiece, "Assets/Prefabs/Blocks/LegoPiece.prefab");
        Object.DestroyImmediate(legoPiece);
        
        AssetDatabase.Refresh();
    }
    
    private static void CreateGameManagers()
    {
        // Create Grid Manager
        GameObject gridManagerObj = new GameObject("GridManager");
        GridManager gridManager = gridManagerObj.AddComponent<GridManager>();
        
        // Load and assign prefabs
        GameObject dirtPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Blocks/DirtBlock.prefab");
        GameObject bedrockPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Blocks/BedrockBlock.prefab");
        GameObject coinPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Blocks/CoinBlock.prefab");
        GameObject legoPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Blocks/LegoPiece.prefab");
        
        // Use reflection to set private fields
        var gridType = gridManager.GetType();
        gridType.GetField("dirtBlockPrefab", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(gridManager, dirtPrefab);
        gridType.GetField("bedrockBlockPrefab", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(gridManager, bedrockPrefab);
        gridType.GetField("coinBlockPrefab", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(gridManager, coinPrefab);
        gridType.GetField("legoPiecePrefab", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(gridManager, legoPrefab);
        
        // Create Game Manager
        GameObject gameManagerObj = new GameObject("GameManager");
        GameManager gameManager = gameManagerObj.AddComponent<GameManager>();
    }
    
    private static void CreatePlayer()
    {
        // Create player
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.tag = "Player";
        player.transform.position = new Vector3(3, 1.5f, 7);
        
        // Add player controller
        PlayerController playerController = player.AddComponent<PlayerController>();
        
        // Create player material
        Shader defaultShader = Shader.Find("Standard");
        if (defaultShader == null) defaultShader = Shader.Find("Sprites/Default");
        if (defaultShader == null) defaultShader = Shader.Find("Legacy Shaders/Diffuse");
        Material playerMat = new Material(defaultShader);
        playerMat.color = Color.blue;
        player.GetComponent<Renderer>().material = playerMat;
        
        // Save as prefab
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }
        PrefabUtility.SaveAsPrefabAsset(player, "Assets/Prefabs/Player.prefab");
    }
    
    private static void SetupCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            mainCamera = Object.FindObjectOfType<Camera>();
        }
        
        if (mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(3, 10, -5);
            mainCamera.transform.rotation = Quaternion.Euler(45, 0, 0);
            
            CameraFollow cameraFollow = mainCamera.gameObject.AddComponent<CameraFollow>();
            // Setup will auto-find player
        }
    }
    
    private static void CreateUI()
    {
        // Create Canvas
        GameObject canvasObj = new GameObject("UICanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // Create HUD Panel
        GameObject hudPanel = CreateUIElement("HUDPanel", canvasObj.transform);
        RectTransform hudRect = hudPanel.GetComponent<RectTransform>();
        hudRect.anchorMin = Vector2.zero;
        hudRect.anchorMax = Vector2.one;
        hudRect.offsetMin = Vector2.zero;
        hudRect.offsetMax = Vector2.zero;
        
        // Create Axes Display
        GameObject axesText = CreateTextElement("AxesText", hudPanel.transform, "Axes: 25");
        RectTransform axesRect = axesText.GetComponent<RectTransform>();
        axesRect.anchorMin = new Vector2(0, 1);
        axesRect.anchorMax = new Vector2(0, 1);
        axesRect.pivot = new Vector2(0, 1);
        axesRect.anchoredPosition = new Vector2(20, -20);
        
        // Create Coins Display
        GameObject coinsText = CreateTextElement("CoinsText", hudPanel.transform, "Coins: 0 (0)");
        RectTransform coinsRect = coinsText.GetComponent<RectTransform>();
        coinsRect.anchorMin = new Vector2(0, 1);
        coinsRect.anchorMax = new Vector2(0, 1);
        coinsRect.pivot = new Vector2(0, 1);
        coinsRect.anchoredPosition = new Vector2(20, -60);
        
        // Create Piece Progress
        GameObject progressText = CreateTextElement("PieceProgressText", hudPanel.transform, "Piece: 0/4");
        RectTransform progressRect = progressText.GetComponent<RectTransform>();
        progressRect.anchorMin = new Vector2(0.5f, 1);
        progressRect.anchorMax = new Vector2(0.5f, 1);
        progressRect.pivot = new Vector2(0.5f, 1);
        progressRect.anchoredPosition = new Vector2(0, -20);
        
        // Create Mobile Controls
        CreateMobileControls(hudPanel.transform);
        
        // Create Win/Lose Panels
        CreateWinLosePanel(canvasObj.transform);
        
        // Create UIManager
        GameObject uiManagerObj = new GameObject("UIManager");
        UIManager uiManager = uiManagerObj.AddComponent<UIManager>();
    }
    
    private static void CreateMobileControls(Transform parent)
    {
        // Create Joystick
        GameObject joystickObj = CreateUIElement("Joystick", parent);
        RectTransform joystickRect = joystickObj.GetComponent<RectTransform>();
        joystickRect.anchorMin = new Vector2(0, 0);
        joystickRect.anchorMax = new Vector2(0, 0);
        joystickRect.pivot = new Vector2(0, 0);
        joystickRect.anchoredPosition = new Vector2(150, 150);
        joystickRect.sizeDelta = new Vector2(200, 200);
        
        Image joystickBg = joystickObj.AddComponent<Image>();
        joystickBg.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        joystickBg.sprite = null; // Would use a circle sprite
        
        GameObject joystickHandle = CreateUIElement("Handle", joystickObj.transform);
        RectTransform handleRect = joystickHandle.GetComponent<RectTransform>();
        handleRect.anchorMin = new Vector2(0.5f, 0.5f);
        handleRect.anchorMax = new Vector2(0.5f, 0.5f);
        handleRect.pivot = new Vector2(0.5f, 0.5f);
        handleRect.anchoredPosition = Vector2.zero;
        handleRect.sizeDelta = new Vector2(80, 80);
        
        Image handleImage = joystickHandle.AddComponent<Image>();
        handleImage.color = new Color(0.3f, 0.3f, 0.3f, 0.8f);
        
        MobileJoystick joystick = joystickObj.AddComponent<MobileJoystick>();
        
        // Create Jump Button
        GameObject jumpButton = CreateButton("JumpButton", parent, "JUMP");
        RectTransform jumpRect = jumpButton.GetComponent<RectTransform>();
        jumpRect.anchorMin = new Vector2(1, 0);
        jumpRect.anchorMax = new Vector2(1, 0);
        jumpRect.pivot = new Vector2(1, 0);
        jumpRect.anchoredPosition = new Vector2(-200, 100);
        jumpRect.sizeDelta = new Vector2(120, 120);
        
        // Create Dig Button
        GameObject digButton = CreateButton("DigButton", parent, "DIG");
        RectTransform digRect = digButton.GetComponent<RectTransform>();
        digRect.anchorMin = new Vector2(1, 0);
        digRect.anchorMax = new Vector2(1, 0);
        digRect.pivot = new Vector2(1, 0);
        digRect.anchoredPosition = new Vector2(-50, 100);
        digRect.sizeDelta = new Vector2(120, 120);
    }
    
    private static void CreateWinLosePanel(Transform parent)
    {
        // Create Win Panel
        GameObject winPanel = CreateUIElement("WinPanel", parent);
        RectTransform winRect = winPanel.GetComponent<RectTransform>();
        winRect.anchorMin = Vector2.zero;
        winRect.anchorMax = Vector2.one;
        winRect.offsetMin = Vector2.zero;
        winRect.offsetMax = Vector2.zero;
        
        Image winBg = winPanel.AddComponent<Image>();
        winBg.color = new Color(0, 0, 0, 0.8f);
        
        GameObject winContent = CreateUIElement("Content", winPanel.transform);
        RectTransform contentRect = winContent.GetComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0.5f, 0.5f);
        contentRect.anchorMax = new Vector2(0.5f, 0.5f);
        contentRect.pivot = new Vector2(0.5f, 0.5f);
        contentRect.anchoredPosition = Vector2.zero;
        contentRect.sizeDelta = new Vector2(400, 300);
        
        Image contentBg = winContent.AddComponent<Image>();
        contentBg.color = Color.white;
        
        CreateTextElement("WinText", winContent.transform, "LEVEL COMPLETE!");
        CreateButton("NextLevelButton", winContent.transform, "Next Level");
        
        winPanel.SetActive(false);
        
        // Create Lose Panel (similar structure)
        GameObject losePanel = CreateUIElement("LosePanel", parent);
        RectTransform loseRect = losePanel.GetComponent<RectTransform>();
        loseRect.anchorMin = Vector2.zero;
        loseRect.anchorMax = Vector2.one;
        loseRect.offsetMin = Vector2.zero;
        loseRect.offsetMax = Vector2.zero;
        
        Image loseBg = losePanel.AddComponent<Image>();
        loseBg.color = new Color(0, 0, 0, 0.8f);
        
        GameObject loseContent = CreateUIElement("Content", losePanel.transform);
        RectTransform loseContentRect = loseContent.GetComponent<RectTransform>();
        loseContentRect.anchorMin = new Vector2(0.5f, 0.5f);
        loseContentRect.anchorMax = new Vector2(0.5f, 0.5f);
        loseContentRect.pivot = new Vector2(0.5f, 0.5f);
        loseContentRect.anchoredPosition = Vector2.zero;
        loseContentRect.sizeDelta = new Vector2(400, 300);
        
        Image loseContentBg = loseContent.AddComponent<Image>();
        loseContentBg.color = Color.white;
        
        CreateTextElement("LoseText", loseContent.transform, "OUT OF AXES!");
        CreateButton("RetryButton", loseContent.transform, "Retry");
        
        losePanel.SetActive(false);
    }
    
    private static GameObject CreateUIElement(string name, Transform parent)
    {
        GameObject element = new GameObject(name);
        element.transform.SetParent(parent, false);
        RectTransform rect = element.AddComponent<RectTransform>();
        return element;
    }
    
    private static GameObject CreateTextElement(string name, Transform parent, string text)
    {
        GameObject textObj = CreateUIElement(name, parent);
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 24;
        tmp.color = Color.white;
        RectTransform rect = textObj.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(200, 50);
        return textObj;
    }
    
    private static GameObject CreateButton(string name, Transform parent, string text)
    {
        GameObject buttonObj = CreateUIElement(name, parent);
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = new Color(0.3f, 0.3f, 0.3f);
        Button button = buttonObj.AddComponent<Button>();
        
        GameObject buttonText = CreateTextElement("Text", buttonObj.transform, text);
        RectTransform textRect = buttonText.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        return buttonObj;
    }
    
    private static void CreateEventSystem()
    {
        GameObject eventSystem = new GameObject("EventSystem");
        eventSystem.AddComponent<EventSystem>();
        eventSystem.AddComponent<StandaloneInputModule>();
    }
}