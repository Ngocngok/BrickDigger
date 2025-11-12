using UnityEngine;

namespace BrickDigger
{
    /// <summary>
    /// Test script to verify character animations are working
    /// Attach to Player GameObject and use keyboard in Play mode
    /// </summary>
    public class TestCharacterAnimations : MonoBehaviour
    {
        private CharacterSpawner characterSpawner;
        private Animator animator;
        
        [Header("Test Settings")]
        [SerializeField] private bool showDebugInfo = true;
        
        private void Start()
        {
            characterSpawner = GetComponent<CharacterSpawner>();
            
            if (characterSpawner != null)
            {
                animator = characterSpawner.GetAnimator();
            }
            
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
            
            if (animator == null)
            {
                Debug.LogError("TestCharacterAnimations: No Animator found!");
            }
            else
            {
                Debug.Log("TestCharacterAnimations: Animator found and ready!");
            }
        }
        
        private void Update()
        {
            if (animator == null) return;
            
            // Test animation parameters with keyboard
            
            // Press 1-5 to test different speed values
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                animator.SetFloat("Speed", 0f);
                Debug.Log("Animation: Speed = 0 (Idle)");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                animator.SetFloat("Speed", 0.5f);
                Debug.Log("Animation: Speed = 0.5 (Walking)");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                animator.SetFloat("Speed", 1f);
                Debug.Log("Animation: Speed = 1 (Walking)");
            }
            
            // Press J to test jump animation
            if (Input.GetKeyDown(KeyCode.J))
            {
                animator.SetBool("IsJumping", true);
                Debug.Log("Animation: Jump triggered");
            }
            if (Input.GetKeyUp(KeyCode.J))
            {
                animator.SetBool("IsJumping", false);
                Debug.Log("Animation: Jump released");
            }
            
            // Press D to test dig animation
            if (Input.GetKeyDown(KeyCode.D))
            {
                animator.SetTrigger("Dig");
                Debug.Log("Animation: Dig triggered");
            }
            
            // Press C to change character (cycle through 1-10)
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (characterSpawner != null)
                {
                    int currentIndex = characterSpawner.GetCurrentCharacterIndex();
                    int nextIndex = (currentIndex % 10) + 1; // Cycle through 1-10
                    characterSpawner.ChangeCharacter(nextIndex);
                    animator = characterSpawner.GetAnimator();
                    Debug.Log($"Changed to Character_{nextIndex}");
                }
            }
            
            // Press I to show current animation info
            if (Input.GetKeyDown(KeyCode.I))
            {
                ShowAnimationInfo();
            }
        }
        
        private void ShowAnimationInfo()
        {
            if (animator == null) return;
            
            Debug.Log("=== Animation Info ===");
            Debug.Log($"Speed: {animator.GetFloat("Speed")}");
            Debug.Log($"IsJumping: {animator.GetBool("IsJumping")}");
            Debug.Log($"IsDigging: {animator.GetBool("IsDigging")}");
            
            // Get current state info
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            Debug.Log($"Current State: {stateInfo.shortNameHash}");
            Debug.Log($"Normalized Time: {stateInfo.normalizedTime}");
            
            if (characterSpawner != null)
            {
                Debug.Log($"Current Character: Character_{characterSpawner.GetCurrentCharacterIndex()}");
            }
        }
        
        private void OnGUI()
        {
            if (!showDebugInfo || animator == null) return;
            
            GUILayout.BeginArea(new Rect(10, 10, 400, 400));
            GUILayout.Label("=== Character Animation Test ===");
            GUILayout.Space(10);
            
            GUILayout.Label("Controls:");
            GUILayout.Label("1 - Set Speed to 0 (Idle)");
            GUILayout.Label("2 - Set Speed to 0.5 (Walking)");
            GUILayout.Label("3 - Set Speed to 1.0 (Walking)");
            GUILayout.Label("J - Toggle Jump Animation");
            GUILayout.Label("D - Trigger Dig Animation");
            GUILayout.Label("C - Change Character (cycle 1-10)");
            GUILayout.Label("I - Show Animation Info in Console");
            
            GUILayout.Space(10);
            GUILayout.Label("Current State:");
            GUILayout.Label($"Speed: {animator.GetFloat("Speed"):F2}");
            GUILayout.Label($"IsJumping: {animator.GetBool("IsJumping")}");
            
            if (characterSpawner != null)
            {
                GUILayout.Label($"Character: Character_{characterSpawner.GetCurrentCharacterIndex()}");
            }
            
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Idle"))
                GUILayout.Label("Animation: IDLE");
            else if (stateInfo.IsName("Walking"))
                GUILayout.Label("Animation: WALKING");
            else if (stateInfo.IsName("Jump"))
                GUILayout.Label("Animation: JUMP");
            else if (stateInfo.IsName("Shovel"))
                GUILayout.Label("Animation: SHOVEL");
            
            GUILayout.EndArea();
        }
    }
}
