using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using BrickDigger;

public static class CreateJoystickArea
{
    [MenuItem("BrickDigger/Create Joystick Touch Area")]
    public static void CreateTouchArea()
    {
        // Find the HUD Panel
        GameObject hudPanel = GameObject.Find("HUDPanel");
        if (hudPanel == null)
        {
            Debug.LogError("HUDPanel not found!");
            return;
        }
        
        // Create touch area
        GameObject touchArea = new GameObject("JoystickTouchArea");
        touchArea.transform.SetParent(hudPanel.transform, false);
        
        // Set as first child so it's behind everything
        touchArea.transform.SetAsFirstSibling();
        
        RectTransform rect = touchArea.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = new Vector2(0.5f, 1f); // Left half of screen
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        
        // Add image (invisible but blocks raycasts)
        Image image = touchArea.AddComponent<Image>();
        image.color = new Color(0, 0, 0, 0); // Fully transparent
        image.raycastTarget = true;
        
        // Add the dynamic joystick area component
        DynamicJoystickArea joystickArea = touchArea.AddComponent<DynamicJoystickArea>();
        
        // Find and assign joystick
        MobileJoystick joystick = Object.FindObjectOfType<MobileJoystick>();
        if (joystick != null)
        {
            var field = joystickArea.GetType().GetField("joystick", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(joystickArea, joystick);
        }
        
        Debug.Log("Joystick touch area created!");
    }
}