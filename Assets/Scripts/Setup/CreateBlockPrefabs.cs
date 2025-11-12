using UnityEngine;
using UnityEditor;

public class CreateBlockPrefabs : MonoBehaviour
{
    [ContextMenu("Create All Block Prefabs")]
    public static void CreateAllPrefabs()
    {
        // Create Dirt Block
        GameObject dirtBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
        dirtBlock.name = "DirtBlock";
        Material dirtMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        dirtMat.color = new Color(0.4f, 0.25f, 0.13f); // Brown
        dirtBlock.GetComponent<Renderer>().material = dirtMat;
        
        // Create Bedrock Block
        GameObject bedrockBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bedrockBlock.name = "BedrockBlock";
        Material bedrockMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        bedrockMat.color = new Color(0.3f, 0.3f, 0.3f); // Dark gray
        bedrockBlock.GetComponent<Renderer>().material = bedrockMat;
        
        // Create Coin Block
        GameObject coinBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
        coinBlock.name = "CoinBlock";
        Material coinMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        coinMat.color = new Color(1f, 0.84f, 0f); // Gold
        coinMat.SetFloat("_Metallic", 0.8f);
        coinMat.SetFloat("_Smoothness", 0.8f);
        coinBlock.GetComponent<Renderer>().material = coinMat;
        
        // Add coin indicator sphere
        GameObject coinIndicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        coinIndicator.name = "CoinIndicator";
        coinIndicator.transform.SetParent(coinBlock.transform);
        coinIndicator.transform.localPosition = Vector3.zero;
        coinIndicator.transform.localScale = Vector3.one * 0.3f;
        Material indicatorMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        indicatorMat.color = Color.yellow;
        indicatorMat.EnableKeyword("_EMISSION");
        indicatorMat.SetColor("_EmissionColor", Color.yellow);
        coinIndicator.GetComponent<Renderer>().material = indicatorMat;
        
        // Create Lego Piece Block
        GameObject legoPiece = GameObject.CreatePrimitive(PrimitiveType.Cube);
        legoPiece.name = "LegoPiece";
        Material legoMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        legoMat.color = new Color(1f, 0f, 0f); // Red plastic
        legoMat.SetFloat("_Smoothness", 0.7f);
        legoPiece.GetComponent<Renderer>().material = legoMat;
        
        // Add studs to make it look like lego
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                GameObject stud = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                stud.name = "Stud";
                stud.transform.SetParent(legoPiece.transform);
                stud.transform.localPosition = new Vector3(x * 0.25f, 0.6f, z * 0.25f);
                stud.transform.localScale = new Vector3(0.15f, 0.1f, 0.15f);
                stud.GetComponent<Renderer>().material = legoMat;
            }
        }
        
        // Clean up
        DestroyImmediate(dirtBlock);
        DestroyImmediate(bedrockBlock);
        DestroyImmediate(coinBlock);
        DestroyImmediate(legoPiece);
        
        Debug.Log("Block prefabs creation script ready. Use context menu to create prefabs.");
    }
}