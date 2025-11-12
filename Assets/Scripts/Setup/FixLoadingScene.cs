using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using BrickDigger;
using UnityEditor.SceneManagement;

public static class FixLoadingScene
{
    [MenuItem("BrickDigger/Fix Loading Scene References")]
    public static void FixReferences()
    {
        // Open loading scene
        EditorSceneManager.OpenScene("Assets/Scenes/LoadingScene.unity");
        
        // Find components
        LoadingScene loadingScene = Object.FindObjectOfType<LoadingScene>();
        if (loadingScene == null)
        {
            Debug.LogError("LoadingScene component not found!");
            return;
        }
        
        // Find UI elements
        Image loadingBar = GameObject.Find("LoadingBarFill")?.GetComponent<Image>();
        Text percentageText = GameObject.Find("PercentageText")?.GetComponent<Text>();
        
        if (loadingBar == null)
        {
            Debug.LogError("LoadingBarFill not found!");
            return;
        }
        
        if (percentageText == null)
        {
            Debug.LogError("PercentageText not found!");
            return;
        }
        
        // Connect references using SerializedObject
        SerializedObject serializedObject = new SerializedObject(loadingScene);
        
        SerializedProperty loadingBarProp = serializedObject.FindProperty("loadingBar");
        SerializedProperty percentageTextProp = serializedObject.FindProperty("percentageText");
        
        if (loadingBarProp != null)
        {
            loadingBarProp.objectReferenceValue = loadingBar;
        }
        
        if (percentageTextProp != null)
        {
            percentageTextProp.objectReferenceValue = percentageText;
        }
        
        serializedObject.ApplyModifiedProperties();
        
        // Mark as dirty to save changes
        EditorUtility.SetDirty(loadingScene);
        
        // Save scene
        EditorSceneManager.SaveOpenScenes();
        
        Debug.Log("Loading scene references fixed!");
    }
}