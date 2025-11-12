using UnityEngine;
using UnityEditor;

public class InitializeCharacterShop
{
    [MenuItem("Tools/Initialize Character Shop")]
    public static void Initialize()
    {
        // Find the CharacterShopManager
        CharacterShopManager shopManager = GameObject.FindFirstObjectByType<CharacterShopManager>();
        
        if (shopManager == null)
        {
            Debug.LogError("CharacterShopManager not found in scene!");
            return;
        }
        
        // Trigger the character display
        // Use reflection to call private ShowCharacter method
        var showCharacterMethod = typeof(CharacterShopManager).GetMethod("ShowCharacter", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (showCharacterMethod != null)
        {
            // Get selected character index
            var getSelectedMethod = typeof(CharacterShopManager).GetMethod("GetSelectedCharacter");
            int selectedIndex = (int)getSelectedMethod.Invoke(shopManager, null);
            
            // Show the character
            showCharacterMethod.Invoke(shopManager, new object[] { selectedIndex });
            
            Debug.Log($"Character shop initialized with Character_{selectedIndex}");
        }
        
        // Mark scene as dirty
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }
}
