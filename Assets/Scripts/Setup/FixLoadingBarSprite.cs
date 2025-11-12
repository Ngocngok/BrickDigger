using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public static class FixLoadingBarSprite
{
    [MenuItem("BrickDigger/Fix Loading Bar Sprite")]
    public static void FixSprite()
    {
        // Open loading scene
        EditorSceneManager.OpenScene("Assets/Scenes/LoadingScene.unity");
        
        // Find the loading bar fill image
        GameObject loadingBarFill = GameObject.Find("LoadingBarFill");
        if (loadingBarFill == null)
        {
            Debug.LogError("LoadingBarFill not found!");
            return;
        }
        
        Image image = loadingBarFill.GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("Image component not found!");
            return;
        }
        
        // Get Unity's built-in sprite
        Sprite sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        if (sprite == null)
        {
            Debug.LogWarning("Built-in sprite not found, creating white texture sprite");
            
            // Create a simple white texture
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            
            sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
        }
        
        // Assign sprite
        image.sprite = sprite;
        
        // Ensure it's set to filled type
        image.type = Image.Type.Filled;
        image.fillMethod = Image.FillMethod.Horizontal;
        image.fillAmount = 0f;
        
        // Also fix the background
        GameObject loadingBarBG = GameObject.Find("LoadingBarBG");
        if (loadingBarBG != null)
        {
            Image bgImage = loadingBarBG.GetComponent<Image>();
            if (bgImage != null && bgImage.sprite == null)
            {
                bgImage.sprite = sprite;
            }
        }
        
        EditorUtility.SetDirty(image);
        EditorSceneManager.SaveOpenScenes();
        
        Debug.Log("Loading bar sprite fixed!");
    }
}