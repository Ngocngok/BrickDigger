using UnityEngine;
using UnityEditor;

public class SetupPlayerCharacter
{
    [MenuItem("Tools/Setup Player Character")]
    public static void Setup()
    {
        // Find the Player GameObject
        GameObject player = GameObject.Find("Player");
        
        if (player == null)
        {
            Debug.LogError("Player GameObject not found in scene!");
            return;
        }
        
        // Get the CharacterSpawner component
        BrickDigger.CharacterSpawner spawner = player.GetComponent<BrickDigger.CharacterSpawner>();
        
        if (spawner == null)
        {
            Debug.LogError("CharacterSpawner component not found on Player!");
            return;
        }
        
        // Spawn the character
        spawner.SpawnCharacterFromAssetPath(1); // Spawn Character_1
        
        // Wait a frame for the character to spawn
        EditorApplication.delayCall += () =>
        {
            // Get the spawned character's animator
            Animator animator = spawner.GetAnimator();
            
            if (animator != null)
            {
                // Load the animator controller
                RuntimeAnimatorController controller = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(
                    "Assets/Animation/PlayerAnimatorController.controller");
                
                if (controller != null)
                {
                    animator.runtimeAnimatorController = controller;
                    Debug.Log("Animator controller assigned successfully!");
                }
                else
                {
                    Debug.LogError("Failed to load PlayerAnimatorController.controller");
                }
            }
            else
            {
                Debug.LogError("Animator not found on spawned character!");
            }
            
            // Mark scene as dirty to save changes
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            
            Debug.Log("Player character setup complete!");
        };
    }
}
