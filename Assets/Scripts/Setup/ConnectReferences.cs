using UnityEngine;
using UnityEditor;
using BrickDigger;
using UnityEngine.UI;
using TMPro;

public static class ConnectReferences
{
    [MenuItem("BrickDigger/Connect All References")]
    public static void ConnectAllReferences()
    {
        // Find all managers
        GridManager gridManager = Object.FindObjectOfType<GridManager>();
        GameManager gameManager = Object.FindObjectOfType<GameManager>();
        PlayerController playerController = Object.FindObjectOfType<PlayerController>();
        UIManager uiManager = Object.FindObjectOfType<UIManager>();
        CameraFollow cameraFollow = Object.FindObjectOfType<CameraFollow>();
        MobileJoystick joystick = Object.FindObjectOfType<MobileJoystick>();
        
        // Connect GameManager references
        if (gameManager != null)
        {
            var gmType = gameManager.GetType();
            gmType.GetField("gridManager", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(gameManager, gridManager);
            gmType.GetField("playerController", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(gameManager, playerController);
            gmType.GetField("uiManager", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(gameManager, uiManager);
        }
        
        // Connect PlayerController references
        if (playerController != null)
        {
            var pcType = playerController.GetType();
            pcType.GetField("gridManager", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(playerController, gridManager);
            pcType.GetField("gameManager", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(playerController, gameManager);
        }
        
        // Connect UIManager references
        if (uiManager != null)
        {
            var uiType = uiManager.GetType();
            
            // Find UI elements
            Text axesText = GameObject.Find("AxesText")?.GetComponent<Text>();
            Text coinsText = GameObject.Find("CoinsText")?.GetComponent<Text>();
            Text pieceProgressText = GameObject.Find("PieceProgressText")?.GetComponent<Text>();
            Button jumpButton = GameObject.Find("JumpButton")?.GetComponent<Button>();
            Button digButton = GameObject.Find("DigButton")?.GetComponent<Button>();
            GameObject winPanel = GameObject.Find("WinPanel");
            GameObject losePanel = GameObject.Find("LosePanel");
            
            uiType.GetField("axesText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(uiManager, axesText);
            uiType.GetField("coinsText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(uiManager, coinsText);
            uiType.GetField("pieceProgressText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(uiManager, pieceProgressText);
            uiType.GetField("jumpButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(uiManager, jumpButton);
            uiType.GetField("digButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(uiManager, digButton);
            uiType.GetField("joystick", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(uiManager, joystick);
            uiType.GetField("winPanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(uiManager, winPanel);
            uiType.GetField("losePanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(uiManager, losePanel);
        }
        
        // Connect GridManager prefab references
        if (gridManager != null)
        {
            GameObject dirtPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Blocks/DirtBlock.prefab");
            GameObject bedrockPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Blocks/BedrockBlock.prefab");
            GameObject coinPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Blocks/CoinBlock.prefab");
            GameObject legoPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Blocks/LegoPiece.prefab");
            
            var gridType = gridManager.GetType();
            gridType.GetField("dirtBlockPrefab", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(gridManager, dirtPrefab);
            gridType.GetField("bedrockBlockPrefab", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(gridManager, bedrockPrefab);
            gridType.GetField("coinBlockPrefab", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(gridManager, coinPrefab);
            gridType.GetField("legoPiecePrefab", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(gridManager, legoPrefab);
        }
        
        // Connect CameraFollow to Player
        if (cameraFollow != null && playerController != null)
        {
            var cfType = cameraFollow.GetType();
            cfType.GetField("target", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(cameraFollow, playerController.transform);
        }
        
        Debug.Log("All references connected!");
        EditorUtility.SetDirty(gridManager);
        EditorUtility.SetDirty(gameManager);
        EditorUtility.SetDirty(playerController);
        EditorUtility.SetDirty(uiManager);
        EditorUtility.SetDirty(cameraFollow);
    }
}