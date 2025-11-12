using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Linq;

public class FixAnimatorController
{
    [MenuItem("Tools/Fix Animator Controller")]
    public static void FixAnimator()
    {
        // Load the animator controller
        string controllerPath = "Assets/Animation/PlayerAnimatorController.controller";
        AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
        
        if (controller == null)
        {
            Debug.LogError("Animator controller not found!");
            return;
        }
        
        // Load animation clips from FBX files
        // The animation clips are sub-assets of the FBX files
        Object[] idleAssets = AssetDatabase.LoadAllAssetsAtPath("Assets/Layer lab/3D Casual Character/Animation/Anim@Stand_Idle1.FBX");
        Object[] walkAssets = AssetDatabase.LoadAllAssetsAtPath("Assets/Layer lab/3D Casual Character/Animation/Anim@Action_Walk.FBX");
        Object[] jumpAssets = AssetDatabase.LoadAllAssetsAtPath("Assets/Layer lab/3D Casual Character/Animation/Anim@Action_Jump.FBX");
        Object[] shovelAssets = AssetDatabase.LoadAllAssetsAtPath("Assets/Layer lab/3D Casual Character/Animation/Anim@Interaction_Shovel.FBX");
        
        // Extract AnimationClip from the assets
        AnimationClip idleClip = idleAssets.OfType<AnimationClip>().FirstOrDefault();
        AnimationClip walkClip = walkAssets.OfType<AnimationClip>().FirstOrDefault();
        AnimationClip jumpClip = jumpAssets.OfType<AnimationClip>().FirstOrDefault();
        AnimationClip shovelClip = shovelAssets.OfType<AnimationClip>().FirstOrDefault();
        
        if (idleClip == null || walkClip == null || jumpClip == null || shovelClip == null)
        {
            Debug.LogError("Failed to load animation clips!");
            Debug.LogError($"Idle: {idleClip != null}, Walk: {walkClip != null}, Jump: {jumpClip != null}, Shovel: {shovelClip != null}");
            
            // Debug: Show what was loaded
            Debug.Log("Idle assets:");
            foreach (var asset in idleAssets)
                Debug.Log($"  - {asset.name} ({asset.GetType().Name})");
            
            Debug.Log("Walk assets:");
            foreach (var asset in walkAssets)
                Debug.Log($"  - {asset.name} ({asset.GetType().Name})");
                
            Debug.Log("Jump assets:");
            foreach (var asset in jumpAssets)
                Debug.Log($"  - {asset.name} ({asset.GetType().Name})");
                
            Debug.Log("Shovel assets:");
            foreach (var asset in shovelAssets)
                Debug.Log($"  - {asset.name} ({asset.GetType().Name})");
            
            return;
        }
        
        Debug.Log($"Loaded clips: Idle={idleClip.name}, Walk={walkClip.name}, Jump={jumpClip.name}, Shovel={shovelClip.name}");
        
        // Get the root state machine
        AnimatorStateMachine rootStateMachine = controller.layers[0].stateMachine;
        
        // Find existing states
        AnimatorState idleState = rootStateMachine.states.FirstOrDefault(s => s.state.name == "Idle").state;
        AnimatorState walkState = rootStateMachine.states.FirstOrDefault(s => s.state.name == "Walking").state;
        AnimatorState jumpState = rootStateMachine.states.FirstOrDefault(s => s.state.name == "Jump").state;
        AnimatorState shovelState = rootStateMachine.states.FirstOrDefault(s => s.state.name == "Shovel").state;
        
        if (idleState == null || walkState == null || jumpState == null || shovelState == null)
        {
            Debug.LogError("One or more states not found in animator controller!");
            return;
        }
        
        // Assign animation clips to states
        idleState.motion = idleClip;
        walkState.motion = walkClip;
        jumpState.motion = jumpClip;
        shovelState.motion = shovelClip;
        
        // Set loop settings
        idleState.timeParameterActive = false;
        walkState.timeParameterActive = false;
        jumpState.timeParameterActive = false;
        shovelState.timeParameterActive = false;
        
        // Save the controller
        EditorUtility.SetDirty(controller);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log("Animator controller fixed successfully!");
        Debug.Log($"States updated: Idle, Walking, Jump, Shovel");
        
        // Now update the character's animator
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            BrickDigger.CharacterSpawner spawner = player.GetComponent<BrickDigger.CharacterSpawner>();
            if (spawner != null)
            {
                Animator animator = spawner.GetAnimator();
                if (animator != null)
                {
                    animator.runtimeAnimatorController = controller;
                    Debug.Log("Animator controller assigned to character!");
                }
            }
        }
    }
}
