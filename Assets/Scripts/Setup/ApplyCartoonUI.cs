using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ApplyCartoonUI
{
    private const string SPRITES_PATH = "Assets/GUIPackCartoon/Demo/Sprites/";
    
    [MenuItem("Tools/Apply Cartoon UI to Home Scene")]
    public static void ApplyToHomeScene()
    {
        Debug.Log("=== Applying Cartoon UI to Home Scene ===");
        
        // Load sprites
        Sprite playButtonBg = LoadSprite("Buttons/Play Button/Bottom.png");
        Sprite playButtonTop = LoadSprite("Buttons/Play Button/Top.png");
        Sprite greenButton = LoadSprite("Buttons/Rectangles/Green.png");
        Sprite blueButton = LoadSprite("Buttons/Rectangles/Blue.png");
        Sprite greyButton = LoadSprite("Buttons/Rectangles/Gray.png");
        Sprite circleButton = LoadSprite("Buttons/Circles/Blue.png");
        Sprite arrowLeft = LoadSprite("Icons/Icons White/Arrows/Arrow - Left.png");
        Sprite arrowRight = LoadSprite("Icons/Icons White/Arrows/Arrow - Right.png");
        Sprite settingsIcon = LoadSprite("Icons/Icons White/Basic/Settings.png");
        Sprite coinIcon = LoadSprite("Icons/Icons Colored/Coins/Coin.png");
        Sprite popupBg = LoadSprite("Backgrounds/Popup/Blue.png");
        Sprite closeIcon = LoadSprite("Icons/Icons White/Basic/X Icon.png");
        
        // Apply to Play Button
        ApplyButtonSprite("Canvas/PlayButton", greenButton);
        
        // Apply to Action Button
        ApplyButtonSprite("Canvas/ActionButton", blueButton);
        
        // Apply to Prev/Next buttons
        ApplyButtonSprite("Canvas/PrevButton", circleButton);
        ApplyButtonSprite("Canvas/NextButton", circleButton);
        
        // Apply icons to Prev/Next
        ApplyImageSprite("Canvas/PrevButton/Text", arrowLeft);
        ApplyImageSprite("Canvas/NextButton/Text", arrowRight);
        
        // Apply to Settings button
        ApplyButtonSprite("Canvas/SettingsButton", circleButton);
        
        // Apply popup background
        ApplyImageSprite("Canvas/SettingsPopup/PopupBackground", popupBg);
        
        // Apply to settings popup buttons
        ApplyButtonSprite("Canvas/SettingsPopup/SoundButton", circleButton);
        ApplyButtonSprite("Canvas/SettingsPopup/MusicButton", circleButton);
        ApplyButtonSprite("Canvas/SettingsPopup/HapticsButton", circleButton);
        ApplyButtonSprite("Canvas/SettingsPopup/CloseButton", circleButton);
        
        Debug.Log("Cartoon UI applied to Home Scene!");
        
        // Mark scene as dirty
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }
    
    [MenuItem("Tools/Apply Cartoon UI to Gameplay Scene")]
    public static void ApplyToGameplayScene()
    {
        Debug.Log("=== Applying Cartoon UI to Gameplay Scene ===");
        
        // Load sprites
        Sprite circleButton = LoadSprite("Buttons/Circles/Blue.png");
        Sprite greenButton = LoadSprite("Buttons/Rectangles/Green.png");
        Sprite redButton = LoadSprite("Buttons/Rectangles/Red.png");
        Sprite popupBg = LoadSprite("Backgrounds/Popup/Blue.png");
        
        // Apply to HUD buttons
        ApplyButtonSprite("UICanvas/HUDPanel/JumpButton", circleButton);
        ApplyButtonSprite("UICanvas/HUDPanel/DigButton", greenButton);
        ApplyButtonSprite("UICanvas/HUDPanel/PauseButton", circleButton);
        
        Debug.Log("Cartoon UI applied to Gameplay Scene!");
        
        // Mark scene as dirty
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }
    
    private static Sprite LoadSprite(string relativePath)
    {
        string fullPath = SPRITES_PATH + relativePath;
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(fullPath);
        
        if (sprite == null)
        {
            Debug.LogWarning($"Failed to load sprite: {fullPath}");
        }
        
        return sprite;
    }
    
    private static void ApplyButtonSprite(string path, Sprite sprite)
    {
        if (sprite == null) return;
        
        GameObject obj = GameObject.Find(path);
        if (obj != null)
        {
            Image image = obj.GetComponent<Image>();
            if (image != null)
            {
                image.sprite = sprite;
                image.type = Image.Type.Sliced; // Use sliced for better scaling
                Debug.Log($"Applied sprite to button: {path}");
            }
        }
        else
        {
            Debug.LogWarning($"GameObject not found: {path}");
        }
    }
    
    private static void ApplyImageSprite(string path, Sprite sprite)
    {
        if (sprite == null) return;
        
        GameObject obj = GameObject.Find(path);
        if (obj != null)
        {
            // Try to get Image component first
            Image image = obj.GetComponent<Image>();
            if (image != null)
            {
                image.sprite = sprite;
                Debug.Log($"Applied sprite to image: {path}");
                return;
            }
            
            // If no Image, add one and remove Text
            Text text = obj.GetComponent<Text>();
            if (text != null)
            {
                Object.DestroyImmediate(text);
                image = obj.AddComponent<Image>();
                image.sprite = sprite;
                image.preserveAspect = true;
                Debug.Log($"Replaced text with image sprite: {path}");
            }
        }
        else
        {
            Debug.LogWarning($"GameObject not found: {path}");
        }
    }
}
