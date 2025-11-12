using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.EventSystems;

public static class FixAllEventSystems
{
    [MenuItem("BrickDigger/Fix All Event Systems")]
    public static void FixAll()
    {
        string[] scenePaths = new string[]
        {
            "Assets/Scenes/LoadingScene.unity",
            "Assets/Scenes/HomeScene.unity"
        };
        
        foreach (string scenePath in scenePaths)
        {
            EditorSceneManager.OpenScene(scenePath);
            
            EventSystem eventSystem = Object.FindObjectOfType<EventSystem>();
            if (eventSystem != null)
            {
                // Remove StandaloneInputModule
                StandaloneInputModule oldModule = eventSystem.GetComponent<StandaloneInputModule>();
                if (oldModule != null)
                {
                    Object.DestroyImmediate(oldModule);
                }
                
                // Add InputSystemUIInputModule
                if (eventSystem.GetComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>() == null)
                {
                    eventSystem.gameObject.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
                }
                
                Debug.Log($"Fixed EventSystem in {scenePath}");
            }
            
            EditorSceneManager.SaveOpenScenes();
        }
        
        Debug.Log("All EventSystems fixed!");
    }
}