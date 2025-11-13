using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class ApplyTextOutlines : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/Apply Black Outlines to All Text")]
    public static void Execute()
    {
        int count = 0;

        // Get the active scene
        Scene activeScene = SceneManager.GetActiveScene();
        
        // Find all TextMeshProUGUI components in the active scene
        GameObject[] rootObjects = activeScene.GetRootGameObjects();
        foreach (GameObject rootObject in rootObjects)
        {
            TextMeshProUGUI[] tmpTexts = rootObject.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (TextMeshProUGUI tmpText in tmpTexts)
            {
                SerializedObject so = new SerializedObject(tmpText);
                
                // Set outline width
                SerializedProperty outlineWidthProp = so.FindProperty("m_outlineWidth");
                if (outlineWidthProp != null)
                {
                    outlineWidthProp.floatValue = 0.2f;
                }
                
                // Set outline color
                SerializedProperty outlineColorProp = so.FindProperty("m_outlineColor");
                if (outlineColorProp != null)
                {
                    outlineColorProp.colorValue = Color.black;
                }
                
                so.ApplyModifiedProperties();
                EditorUtility.SetDirty(tmpText);
                count++;
            }
        }

        Debug.Log($"Applied black outlines to {count} TextMeshPro texts in scene '{activeScene.name}'.");
        
        // Mark scene as dirty
        EditorSceneManager.MarkSceneDirty(activeScene);
    }
#endif
}
