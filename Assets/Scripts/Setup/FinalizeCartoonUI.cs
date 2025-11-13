using UnityEngine;
using UnityEditor;

public class FinalizeCartoonUI
{
    [MenuItem("Tools/Finalize Cartoon UI")]
    public static void Finalize()
    {
        Debug.Log("=== Finalizing Cartoon UI ===");
        
        // Hide settings popup
        GameObject settingsPopup = GameObject.Find("Canvas/SettingsPopup");
        if (settingsPopup != null)
        {
            settingsPopup.SetActive(false);
            Debug.Log("Settings popup hidden");
        }
        
        // Ensure Prev/Next buttons have proper colors
        SetButtonColor("Canvas/PrevButton", new Color(1f, 1f, 1f, 1f));
        SetButtonColor("Canvas/NextButton", new Color(1f, 1f, 1f, 1f));
        
        Debug.Log("Cartoon UI finalized!");
        
        // Mark scene as dirty
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }
    
    private static void SetButtonColor(string path, Color color)
    {
        GameObject obj = GameObject.Find(path);
        if (obj != null)
        {
            UnityEngine.UI.Image image = obj.GetComponent<UnityEngine.UI.Image>();
            if (image != null)
            {
                image.color = color;
            }
        }
    }
}
