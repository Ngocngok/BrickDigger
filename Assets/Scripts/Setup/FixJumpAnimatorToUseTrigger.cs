using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Linq;

public class FixJumpAnimatorToUseTrigger
{
    [MenuItem("Tools/Fix Jump Animator - Use Trigger")]
    public static void FixJumpAnimator()
    {
        // Load the animator controller
        string controllerPath = "Assets/Animation/PlayerAnimatorController.controller";
        AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
        
        if (controller == null)
        {
            Debug.LogError("Animator controller not found!");
            return;
        }
        
        // Remove the old IsJumping boolean parameter if it exists
        var isJumpingParam = controller.parameters.FirstOrDefault(p => p.name == "IsJumping");
        if (isJumpingParam != null)
        {
            controller.RemoveParameter(isJumpingParam);
            Debug.Log("Removed IsJumping boolean parameter");
        }
        
        // Add Jump trigger parameter if it doesn't exist
        var jumpParam = controller.parameters.FirstOrDefault(p => p.name == "Jump");
        if (jumpParam == null)
        {
            controller.AddParameter("Jump", AnimatorControllerParameterType.Trigger);
            Debug.Log("Added Jump trigger parameter");
        }
        
        // Get the root state machine
        AnimatorStateMachine rootStateMachine = controller.layers[0].stateMachine;
        
        // Find the Jump state
        AnimatorState jumpState = rootStateMachine.states.FirstOrDefault(s => s.state.name == "Jump").state;
        
        if (jumpState == null)
        {
            Debug.LogError("Jump state not found!");
            return;
        }
        
        // Remove all existing transitions to Jump state
        var anyStateTransitions = rootStateMachine.anyStateTransitions.Where(t => t.destinationState == jumpState).ToArray();
        foreach (var transition in anyStateTransitions)
        {
            rootStateMachine.RemoveAnyStateTransition(transition);
            Debug.Log("Removed old Any State -> Jump transition");
        }
        
        // Create new Any State -> Jump transition using trigger
        AnimatorStateTransition anyToJump = rootStateMachine.AddAnyStateTransition(jumpState);
        anyToJump.AddCondition(AnimatorConditionMode.If, 0, "Jump");
        anyToJump.hasExitTime = false;
        anyToJump.duration = 0.1f;
        anyToJump.canTransitionToSelf = false;
        Debug.Log("Created new Any State -> Jump transition with trigger");
        
        // Update Jump -> Idle transition to use exit time only
        var jumpTransitions = jumpState.transitions.ToArray();
        foreach (var transition in jumpTransitions)
        {
            jumpState.RemoveTransition(transition);
        }
        
        // Find Idle state
        AnimatorState idleState = rootStateMachine.states.FirstOrDefault(s => s.state.name == "Idle").state;
        
        if (idleState != null)
        {
            // Create Jump -> Idle transition with exit time
            AnimatorStateTransition jumpToIdle = jumpState.AddTransition(idleState);
            jumpToIdle.hasExitTime = true;
            jumpToIdle.exitTime = 0.9f; // Exit near end of animation
            jumpToIdle.duration = 0.1f;
            Debug.Log("Created Jump -> Idle transition with exit time");
        }
        
        // Save the controller
        EditorUtility.SetDirty(controller);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log("Jump animator fixed successfully!");
        Debug.Log("Now using Jump trigger instead of IsJumping boolean");
        
        // Update the character's animator
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
                    Debug.Log("Animator controller updated on character!");
                }
            }
        }
    }
}
