using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class EnhanceUIWithIcons
{
    private const string SPRITES_PATH = "Assets/GUIPackCartoon/Demo/Sprites/";
    
    [MenuItem("Tools/Enhance UI with Icons")]
    public static void EnhanceUI()
    {
        Debug.Log("=== Enhancing UI with Icons ===");
        
        // Load icon sprites
        Sprite coinIcon = LoadSprite("Icons/Icons Colored/Coins/Coin.png");
        Sprite settingsIcon = LoadSprite("Icons/Icons White/Basic/Settings.png");
        Sprite soundOnIcon = LoadSprite("Icons/Icons White/Media/Sound On.png");
        Sprite soundOffIcon = LoadSprite("Icons/Icons White/Media/Sound Off.png");
        Sprite musicIcon = LoadSprite("Icons/Icons White/Media/Music - Note.png");
        Sprite closeIcon = LoadSprite("Icons/Icons White/Basic/X Icon.png");
        
        // Add coin icon to coins text
        AddIconToText("Canvas/ShopCoinsText", coinIcon, "CoinIcon");
        
        // Update settings button icon
        UpdateSettingsIcon(settingsIcon);
        
        // Update settings popup icons
        UpdateSettingsPopupIcons(soundOnIcon, musicIcon, closeIcon);
        
        Debug.Log("UI enhanced with icons!");
        
        // Mark scene as dirty
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }
    
    private static void AddIconToText(string textPath, Sprite icon, string iconName)
    {
        if (icon == null) return;
        
        GameObject textObj = GameObject.Find(textPath);
        if (textObj == null) return;
        
        // Check if icon already exists
        Transform existingIcon = textObj.transform.Find(iconName);
        if (existingIcon != null)
        {
            Object.DestroyImmediate(existingIcon.gameObject);
        }
        
        // Create icon GameObject
        GameObject iconObj = new GameObject(iconName);
        iconObj.transform.SetParent(textObj.transform, false);
        
        // Add Image component
        Image iconImage = iconObj.AddComponent<Image>();
        iconImage.sprite = icon;
        iconImage.preserveAspect = true;
        
        // Position icon to the left of text
        RectTransform iconRect = iconObj.GetComponent<RectTransform>();
        iconRect.anchorMin = new Vector2(0, 0.5f);
        iconRect.anchorMax = new Vector2(0, 0.5f);
        iconRect.pivot = new Vector2(0, 0.5f);
        iconRect.anchoredPosition = new Vector2(0, 0);
        iconRect.sizeDelta = new Vector2(40, 40);
        
        // Adjust text position to make room for icon
        Text text = textObj.GetComponent<Text>();
        if (text != null)
        {
            text.alignment = TextAnchor.MiddleLeft;
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.offsetMin = new Vector2(50, textRect.offsetMin.y); // Add left padding
        }
        
        Debug.Log($"Added icon to: {textPath}");
    }
    
    private static void UpdateSettingsIcon(Sprite settingsIcon)
    {
        if (settingsIcon == null) return;
        
        GameObject settingsBtn = GameObject.Find("Canvas/SettingsButton");
        if (settingsBtn != null)
        {
            // Find or create icon child
            Transform iconTransform = settingsBtn.transform.Find("Icon");
            GameObject iconObj;
            
            if (iconTransform == null)
            {
                iconObj = new GameObject("Icon");
                iconObj.transform.SetParent(settingsBtn.transform, false);
            }
            else
            {
                iconObj = iconTransform.gameObject;
            }
            
            // Setup icon
            Image iconImage = iconObj.GetComponent<Image>();
            if (iconImage == null)
                iconImage = iconObj.AddComponent<Image>();
            
            iconImage.sprite = settingsIcon;
            iconImage.preserveAspect = true;
            iconImage.color = Color.white;
            
            // Center icon in button
            RectTransform iconRect = iconObj.GetComponent<RectTransform>();
            iconRect.anchorMin = new Vector2(0.5f, 0.5f);
            iconRect.anchorMax = new Vector2(0.5f, 0.5f);
            iconRect.pivot = new Vector2(0.5f, 0.5f);
            iconRect.anchoredPosition = Vector2.zero;
            iconRect.sizeDelta = new Vector2(40, 40);
            
            Debug.Log("Updated settings icon");
        }
    }
    
    private static void UpdateSettingsPopupIcons(Sprite soundIcon, Sprite musicIcon, Sprite closeIcon)
    {
        // These are already set by SettingsManager, but we can update them
        // The icons are managed by the SettingsManager script
        Debug.Log("Settings popup icons managed by SettingsManager");
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
}
