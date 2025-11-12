using UnityEngine;
using UnityEditor;

public static class SetupBlockLayers
{
    [MenuItem("BrickDigger/Setup Block Layers")]
    public static void SetupLayers()
    {
        // Create layers if they don't exist
        CreateLayer("Bedrock");
        CreateLayer("Dirt");
        
        // Find all blocks and set their layers
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
        
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("Bedrock"))
            {
                obj.layer = LayerMask.NameToLayer("Bedrock");
                Debug.Log($"Set {obj.name} to Bedrock layer");
            }
            else if (obj.name.Contains("Dirt") || obj.name.Contains("Coin"))
            {
                obj.layer = LayerMask.NameToLayer("Dirt");
                Debug.Log($"Set {obj.name} to Dirt layer");
            }
        }
        
        Debug.Log("Block layers setup complete!");
    }
    
    private static void CreateLayer(string layerName)
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layers = tagManager.FindProperty("layers");
        
        // Check if layer already exists
        for (int i = 8; i < layers.arraySize; i++)
        {
            SerializedProperty layerSP = layers.GetArrayElementAtIndex(i);
            if (layerSP.stringValue == layerName)
            {
                return; // Layer already exists
            }
        }
        
        // Find empty slot and add layer
        for (int i = 8; i < layers.arraySize; i++)
        {
            SerializedProperty layerSP = layers.GetArrayElementAtIndex(i);
            if (string.IsNullOrEmpty(layerSP.stringValue))
            {
                layerSP.stringValue = layerName;
                tagManager.ApplyModifiedProperties();
                Debug.Log($"Created layer: {layerName}");
                return;
            }
        }
    }
}