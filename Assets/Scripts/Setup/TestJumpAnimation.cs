using UnityEngine;

/// <summary>
/// Simple test to verify jump animation works
/// Press J in Play mode to test jump animation
/// </summary>
public class TestJumpAnimation : MonoBehaviour
{
    private Animator animator;
    
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        
        if (animator == null)
        {
            Debug.LogError("No Animator found!");
            return;
        }
        
        Debug.Log("=== Jump Animation Test Ready ===");
        Debug.Log($"Animator: {animator.gameObject.name}");
        Debug.Log($"Controller: {animator.runtimeAnimatorController?.name}");
        Debug.Log("Press J to test jump animation");
        Debug.Log("Press I to show current state");
    }
    
    private void Update()
    {
        if (animator == null) return;
        
        // Press J to test jump
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("=== Testing Jump Animation ===");
            animator.SetBool("IsJumping", true);
            Debug.Log("IsJumping set to TRUE");
            
            // Reset after 1 second
            Invoke("ResetJump", 1f);
        }
        
        // Press I to show info
        if (Input.GetKeyDown(KeyCode.I))
        {
            ShowAnimatorInfo();
        }
    }
    
    private void ResetJump()
    {
        if (animator != null)
        {
            animator.SetBool("IsJumping", false);
            Debug.Log("IsJumping set to FALSE");
        }
    }
    
    private void ShowAnimatorInfo()
    {
        if (animator == null) return;
        
        Debug.Log("=== Animator Info ===");
        Debug.Log($"Controller: {animator.runtimeAnimatorController?.name}");
        Debug.Log($"Enabled: {animator.enabled}");
        
        // Show parameters
        Debug.Log("Parameters:");
        foreach (var param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Float)
                Debug.Log($"  {param.name} (Float) = {animator.GetFloat(param.name)}");
            else if (param.type == AnimatorControllerParameterType.Bool)
                Debug.Log($"  {param.name} (Bool) = {animator.GetBool(param.name)}");
            else if (param.type == AnimatorControllerParameterType.Trigger)
                Debug.Log($"  {param.name} (Trigger)");
        }
        
        // Show current state
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        string stateName = "Unknown";
        
        if (stateInfo.IsName("Idle")) stateName = "IDLE";
        else if (stateInfo.IsName("Walking")) stateName = "WALKING";
        else if (stateInfo.IsName("Jump")) stateName = "JUMP";
        else if (stateInfo.IsName("Shovel")) stateName = "SHOVEL";
        
        Debug.Log($"Current State: {stateName}");
        Debug.Log($"Normalized Time: {stateInfo.normalizedTime}");
    }
    
    private void OnGUI()
    {
        if (animator == null) return;
        
        GUILayout.BeginArea(new Rect(10, 10, 300, 200));
        GUILayout.Label("=== Jump Animation Test ===");
        GUILayout.Label("J - Test Jump Animation");
        GUILayout.Label("I - Show Animator Info");
        GUILayout.Space(10);
        
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        string stateName = "Unknown";
        
        if (stateInfo.IsName("Idle")) stateName = "IDLE";
        else if (stateInfo.IsName("Walking")) stateName = "WALKING";
        else if (stateInfo.IsName("Jump")) stateName = "JUMP";
        else if (stateInfo.IsName("Shovel")) stateName = "SHOVEL";
        
        GUILayout.Label($"Current State: {stateName}");
        GUILayout.Label($"IsJumping: {animator.GetBool("IsJumping")}");
        GUILayout.Label($"Speed: {animator.GetFloat("Speed"):F2}");
        
        GUILayout.EndArea();
    }
}
