using UnityEngine;
using UnityEditor;

public class ImportSpritesAsUI
{
    [MenuItem("Tools/Import UI Sprites")]
    public static void ImportUISprites()
    {
        string[] spritePaths = new string[]
        {
            "Assets/Sprites/UI/SettingsIcon.png",
            "Assets/Sprites/UI/SoundOn.png",
            "Assets/Sprites/UI/SoundOff.png",
            "Assets/Sprites/UI/MusicOn.png",
            "Assets/Sprites/UI/MusicOff.png",
            "Assets/Sprites/UI/HapticsOn.png",
            "Assets/Sprites/UI/HapticsOff.png",
            "Assets/Sprites/UI/CloseIcon.png"
        };

        foreach (string path in spritePaths)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.mipmapEnabled = false;
                importer.filterMode = FilterMode.Bilinear;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                Debug.Log($"Imported sprite: {path}");
            }
            else
            {
                Debug.LogWarning($"Could not find texture at: {path}");
            }
        }
        
        AssetDatabase.Refresh();
        Debug.Log("All UI sprites imported successfully!");
    }
}
