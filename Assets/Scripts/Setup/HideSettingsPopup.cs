using UnityEngine;

public class HideSettingsPopup
{
    public static void Execute()
    {
        GameObject settingsPopup = GameObject.Find("Canvas/SettingsPopup");
        if (settingsPopup != null)
        {
            settingsPopup.SetActive(false);
            Debug.Log("Settings popup hidden successfully!");
        }
        else
        {
            Debug.LogWarning("Settings popup not found!");
        }
    }
}
