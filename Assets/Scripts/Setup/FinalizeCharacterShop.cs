using UnityEngine;
using UnityEditor;

public class FinalizeCharacterShop
{
    [MenuItem("Tools/Finalize Character Shop")]
    public static void Finalize()
    {
        // Hide settings popup
        GameObject settingsPopup = GameObject.Find("Canvas/SettingsPopup");
        if (settingsPopup != null)
        {
            settingsPopup.SetActive(false);
            Debug.Log("Settings popup hidden");
        }
        
        // Hide the old CoinsText (we're using ShopCoinsText now)
        GameObject oldCoinsText = GameObject.Find("Canvas/CoinsText");
        if (oldCoinsText != null)
        {
            oldCoinsText.SetActive(false);
            Debug.Log("Old CoinsText hidden");
        }
        
        // Set character preview camera to only render character layer
        Camera previewCamera = GameObject.Find("CharacterPreviewCamera")?.GetComponent<Camera>();
        if (previewCamera != null)
        {
            // Set culling mask to render everything (we'll use depth to layer cameras)
            previewCamera.clearFlags = CameraClearFlags.SolidColor;
            previewCamera.backgroundColor = new Color(0.078f, 0.078f, 0.078f, 1f);
            previewCamera.depth = -1; // Render first (background)
            Debug.Log("Preview camera configured");
        }
        
        // Set UI camera to render on top
        Camera uiCamera = GameObject.Find("Camera")?.GetComponent<Camera>();
        if (uiCamera != null)
        {
            uiCamera.clearFlags = CameraClearFlags.Depth;
            uiCamera.depth = 1; // Render on top
            Debug.Log("UI camera configured");
        }
        
        // Update Canvas to use UI camera
        Canvas canvas = GameObject.Find("Canvas")?.GetComponent<Canvas>();
        if (canvas != null && uiCamera != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = uiCamera;
            canvas.planeDistance = 1;
            Debug.Log("Canvas configured for camera rendering");
        }
        
        // Mark scene as dirty
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        
        Debug.Log("Character shop finalized!");
    }
}
