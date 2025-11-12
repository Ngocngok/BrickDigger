using UnityEngine;

/// <summary>
/// Quick test script to verify AudioManager and Settings integration
/// Attach this to any GameObject and press the keys in Play mode to test
/// </summary>
public class TestAudioIntegration : MonoBehaviour
{
    [Header("Test Audio Clips")]
    [SerializeField] private AudioClip testMusic;
    [SerializeField] private AudioClip testSFX;
    
    private void Update()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogWarning("AudioManager not found in scene!");
            return;
        }
        
        // Press M to play test music
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (testMusic != null)
            {
                AudioManager.Instance.PlayMusic(testMusic, loop: true);
                Debug.Log("Playing test music");
            }
            else
            {
                Debug.LogWarning("Test music clip not assigned!");
            }
        }
        
        // Press S to play test sound effect
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (testSFX != null)
            {
                AudioManager.Instance.PlaySFX(testSFX);
                Debug.Log("Playing test SFX");
            }
            else
            {
                Debug.LogWarning("Test SFX clip not assigned!");
            }
        }
        
        // Press P to pause/resume music
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (AudioManager.Instance.IsMusicPlaying())
            {
                AudioManager.Instance.PauseMusic();
                Debug.Log("Music paused");
            }
            else
            {
                AudioManager.Instance.ResumeMusic();
                Debug.Log("Music resumed");
            }
        }
        
        // Press X to stop music
        if (Input.GetKeyDown(KeyCode.X))
        {
            AudioManager.Instance.StopMusic();
            Debug.Log("Music stopped");
        }
        
        // Press 1 to toggle music on/off
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            bool currentState = AudioManager.Instance.IsMusicEnabled();
            AudioManager.Instance.SetMusicEnabled(!currentState);
            Debug.Log($"Music enabled: {!currentState}");
        }
        
        // Press 2 to toggle SFX on/off
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            bool currentState = AudioManager.Instance.IsSFXEnabled();
            AudioManager.Instance.SetSFXEnabled(!currentState);
            Debug.Log($"SFX enabled: {!currentState}");
        }
        
        // Press I to show current audio info
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("=== Audio Manager Info ===");
            Debug.Log($"Music Enabled: {AudioManager.Instance.IsMusicEnabled()}");
            Debug.Log($"SFX Enabled: {AudioManager.Instance.IsSFXEnabled()}");
            Debug.Log($"Music Volume: {AudioManager.Instance.GetMusicVolume()}");
            Debug.Log($"SFX Volume: {AudioManager.Instance.GetSFXVolume()}");
            Debug.Log($"Music Playing: {AudioManager.Instance.IsMusicPlaying()}");
        }
    }
    
    private void OnGUI()
    {
        if (AudioManager.Instance == null) return;
        
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        GUILayout.Label("=== Audio Test Controls ===");
        GUILayout.Label("M - Play Music");
        GUILayout.Label("S - Play SFX");
        GUILayout.Label("P - Pause/Resume Music");
        GUILayout.Label("X - Stop Music");
        GUILayout.Label("1 - Toggle Music On/Off");
        GUILayout.Label("2 - Toggle SFX On/Off");
        GUILayout.Label("I - Show Audio Info");
        GUILayout.Space(10);
        GUILayout.Label($"Music: {(AudioManager.Instance.IsMusicEnabled() ? "ON" : "OFF")}");
        GUILayout.Label($"SFX: {(AudioManager.Instance.IsSFXEnabled() ? "ON" : "OFF")}");
        GUILayout.EndArea();
    }
}
