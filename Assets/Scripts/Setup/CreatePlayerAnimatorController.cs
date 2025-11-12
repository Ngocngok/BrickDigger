using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class CreatePlayerAnimatorController
{
    [MenuItem("Tools/Create Player Animator Controller")]
    public static void CreateAnimator()
    {
        // Create animator controller
        string controllerPath = "Assets/Animation/PlayerAnimatorController.controller";
        
        // Create directory if it doesn't exist
        if (!System.IO.Directory.Exists("Assets/Animation"))
        {
            System.IO.Directory.CreateDirectory("Assets/Animation");
        }
        
        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
        
        // Load animation clips
        AnimationClip idleClip = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/Layer lab/3D Casual Character/Animation/Anim@Stand_Idle1.FBX");
        AnimationClip walkClip = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/Layer lab/3D Casual Character/Animation/Anim@Action_Walk.FBX");
        AnimationClip jumpClip = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/Layer lab/3D Casual Character/Animation/Anim@Action_Jump.FBX");
        AnimationClip shovelClip = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/Layer lab/3D Casual Character/Animation/Anim@Interaction_Shovel.FBX");
        
        if (idleClip == null || walkClip == null || jumpClip == null || shovelClip == null)
        {
            Debug.LogError("Failed to load one or more animation clips. Please check the paths.");
            return;
        }
        
        // Add parameters
        controller.AddParameter("Speed", AnimatorControllerParameterType.Float);
        controller.AddParameter("IsJumping", AnimatorControllerParameterType.Bool);
        controller.AddParameter("IsDigging", AnimatorControllerParameterType.Bool);
        controller.AddParameter("Dig", AnimatorControllerParameterType.Trigger);
        
        // Get the root state machine
        AnimatorStateMachine rootStateMachine = controller.layers[0].stateMachine;
        
        // Create states
        AnimatorState idleState = rootStateMachine.AddState("Idle");
        AnimatorState walkState = rootStateMachine.AddState("Walking");
        AnimatorState jumpState = rootStateMachine.AddState("Jump");
        AnimatorState shovelState = rootStateMachine.AddState("Shovel");
        
        // Assign animation clips to states
        idleState.motion = idleClip;
        walkState.motion = walkClip;
        jumpState.motion = jumpClip;
        shovelState.motion = shovelClip;
        
        // Set default state
        rootStateMachine.defaultState = idleState;
        
        // Create transitions
        
        // Idle <-> Walking
        AnimatorStateTransition idleToWalk = idleState.AddTransition(walkState);
        idleToWalk.AddCondition(AnimatorConditionMode.Greater, 0.1f, "Speed");
        idleToWalk.hasExitTime = false;
        idleToWalk.duration = 0.2f;
        
        AnimatorStateTransition walkToIdle = walkState.AddTransition(idleState);
        walkToIdle.AddCondition(AnimatorConditionMode.Less, 0.1f, "Speed");
        walkToIdle.hasExitTime = false;
        walkToIdle.duration = 0.2f;
        
        // Any State -> Jump
        AnimatorStateTransition anyToJump = rootStateMachine.AddAnyStateTransition(jumpState);
        anyToJump.AddCondition(AnimatorConditionMode.If, 0, "IsJumping");
        anyToJump.hasExitTime = false;
        anyToJump.duration = 0.1f;
        
        // Jump -> Idle (when animation finishes)
        AnimatorStateTransition jumpToIdle = jumpState.AddTransition(idleState);
        jumpToIdle.AddCondition(AnimatorConditionMode.IfNot, 0, "IsJumping");
        jumpToIdle.hasExitTime = true;
        jumpToIdle.exitTime = 0.9f;
        jumpToIdle.duration = 0.1f;
        
        // Any State -> Shovel (using trigger)
        AnimatorStateTransition anyToShovel = rootStateMachine.AddAnyStateTransition(shovelState);
        anyToShovel.AddCondition(AnimatorConditionMode.If, 0, "Dig");
        anyToShovel.hasExitTime = false;
        anyToShovel.duration = 0.1f;
        
        // Shovel -> Idle (when animation finishes)
        AnimatorStateTransition shovelToIdle = shovelState.AddTransition(idleState);
        shovelToIdle.hasExitTime = true;
        shovelToIdle.exitTime = 0.95f;
        shovelToIdle.duration = 0.1f;
        
        // Save the controller
        EditorUtility.SetDirty(controller);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log($"Player Animator Controller created successfully at {controllerPath}");
        Debug.Log("Parameters: Speed (Float), IsJumping (Bool), IsDigging (Bool), Dig (Trigger)");
        Debug.Log("States: Idle, Walking, Jump, Shovel");
    }
}
