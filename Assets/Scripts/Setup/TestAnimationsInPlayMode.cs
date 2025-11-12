using UnityEngine;

/// <summary>
/// Simple script to test if animations are working
/// Add this to Player and enter Play mode
/// </summary>
public class TestAnimationsInPlayMode : MonoBehaviour
{
    private Animator animator;
    
    private void Start()
    {
        // Get animator from character
        animator = GetComponentInChildren<Animator>();
        
        if (animator == null)
        {
            Debug.LogError("No Animator found!");
            return;
        }
        
        Debug.Log("=== Animation Test Started ===");
        Debug.Log($"Animator found: {animator.gameObject.name}");
        Debug.Log($"Controller: {animator.runtimeAnimatorController?.name}");
        Debug.Log($"Has controller: {animator.runtimeAnimatorController != null}");
        
        // Check parameters
        if (animator.runtimeAnimatorController != null)
        {
            Debug.Log("Parameters:");
            foreach (var param in animator.parameters)
            {
                Debug.Log($"  - {param.name} ({param.type})");
            }
        }
        
        // Test idle animation
        Invoke("TestIdle", 1f);
        Invoke("TestWalk", 3f);
        Invoke("TestJump", 5f);
        Invoke("TestDig", 7f);
        Invoke("TestIdleAgain", 9f);
    }
    
    private void TestIdle()
    {
        Debug.Log("Testing IDLE: Setting Speed = 0");
        animator.SetFloat("Speed", 0f);
    }
    
    private void TestWalk()
    {
        Debug.Log("Testing WALK: Setting Speed = 1");
        animator.SetFloat("Speed", 1f);
    }
    
    private void TestJump()
    {
        Debug.Log("Testing JUMP: Setting IsJumping = true");
        animator.SetBool("IsJumping", true);
        Invoke("StopJump", 1f);
    }
    
    private void StopJump()
    {
        Debug.Log("Stopping JUMP: Setting IsJumping = false");
        animator.SetBool("IsJumping", false);
    }
    
    private void TestDig()
    {
        Debug.Log("Testing DIG: Triggering Dig");
        animator.SetTrigger("Dig");
    }
    
    private void TestIdleAgain()
    {
        Debug.Log("Back to IDLE: Setting Speed = 0");
        animator.SetFloat("Speed", 0f);
    }
    
    private void Update()
    {
        if (animator == null) return;
        
        // Show current state every 2 seconds
        if (Time.frameCount % 120 == 0)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            string stateName = "Unknown";
            
            if (stateInfo.IsName("Idle")) stateName = "IDLE";
            else if (stateInfo.IsName("Walking")) stateName = "WALKING";
            else if (stateInfo.IsName("Jump")) stateName = "JUMP";
            else if (stateInfo.IsName("Shovel")) stateName = "SHOVEL";
            
            Debug.Log($"Current State: {stateName} | Speed: {animator.GetFloat("Speed")} | IsJumping: {animator.GetBool("IsJumping")}");
        }
    }
}
