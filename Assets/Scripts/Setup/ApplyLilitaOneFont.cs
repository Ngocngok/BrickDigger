using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class ApplyLilitaOneFont : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/Apply LilitaOne Font to All Text")]
    public static void Execute()
    {
        // Load the LilitaOne font
        TMP_FontAsset lilitaOneFont = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/GUIPackCartoon/Demo/Fonts/LilitaOne - Regular SDF.asset");
        Font lilitaOneFontRegular = AssetDatabase.LoadAssetAtPath<Font>("Assets/GUIPackCartoon/Demo/Fonts/LilitaOne - Regular.ttf");
        
        if (lilitaOneFont == null)
        {
            Debug.LogError("LilitaOne SDF font not found!");
            return;
        }

        int textMeshProCount = 0;
        int textCount = 0;

        // Get the active scene
        Scene activeScene = SceneManager.GetActiveScene();
        
        // Find all TextMeshProUGUI components in the active scene
        GameObject[] rootObjects = activeScene.GetRootGameObjects();
        foreach (GameObject rootObject in rootObjects)
        {
            TextMeshProUGUI[] tmpTexts = rootObject.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (TextMeshProUGUI tmpText in tmpTexts)
            {
                Undo.RecordObject(tmpText, "Apply LilitaOne Font");
                
                // Set font
                tmpText.font = lilitaOneFont;
                
                // Set font size if it's too small
                if (tmpText.fontSize < 20)
                {
                    tmpText.fontSize = 24;
                }
                
                EditorUtility.SetDirty(tmpText);
                textMeshProCount++;
            }

            // Find all regular Text components
            Text[] regularTexts = rootObject.GetComponentsInChildren<Text>(true);
            foreach (Text text in regularTexts)
            {
                if (lilitaOneFontRegular != null)
                {
                    Undo.RecordObject(text, "Apply LilitaOne Font");
                    text.font = lilitaOneFontRegular;
                    
                    // Add outline component if it doesn't exist
                    Outline outline = text.GetComponent<Outline>();
                    if (outline == null)
                    {
                        outline = Undo.AddComponent<Outline>(text.gameObject);
                    }
                    else
                    {
                        Undo.RecordObject(outline, "Apply Black Outline");
                    }
                    
                    outline.effectColor = Color.black;
                    outline.effectDistance = new Vector2(2, -2);
                    
                    EditorUtility.SetDirty(text);
                    EditorUtility.SetDirty(outline);
                    textCount++;
                }
            }
        }

        Debug.Log($"Applied LilitaOne font to {textMeshProCount} TextMeshPro texts and {textCount} regular Text components in scene '{activeScene.name}'.");
        Debug.Log("Note: For TextMeshPro outline, please manually set 'Outline Width' to 0.2 and 'Outline Color' to black in the inspector.");
        
        // Mark scene as dirty
        EditorSceneManager.MarkSceneDirty(activeScene);
    }
#endif
}
