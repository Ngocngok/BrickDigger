using UnityEngine;

/// <summary>
/// Example script showing how to use the AudioManager
/// This is for reference only - you can delete this file after understanding the usage
/// </summary>
public class AudioManagerExample : MonoBehaviour
{
    [Header("Example Audio Clips")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip coinCollectSound;
    [SerializeField] private AudioClip digSound;
    
    private void Start()
    {
        // Example: Play background music when the scene starts
        if (backgroundMusic != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(backgroundMusic, loop: true);
        }
    }
    
    // Example: Call this when a button is clicked
    public void OnButtonClick()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(buttonClickSound);
        }
    }
    
    // Example: Call this when collecting a coin
    public void OnCoinCollect()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(coinCollectSound, volumeScale: 0.8f);
        }
    }
    
    // Example: Call this when digging
    public void OnDig()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(digSound);
        }
    }
    
    // Example: Play music by name (if you have multiple music clips in AudioManager)
    public void PlayMenuMusic()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusicByName("MenuMusic", loop: true);
        }
    }
    
    // Example: Play music by index
    public void PlayGameplayMusic()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(0, loop: true); // Play first music clip
        }
    }
    
    // Example: Stop music
    public void StopBackgroundMusic()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
        }
    }
    
    // Example: Pause/Resume music
    public void ToggleMusicPause()
    {
        if (AudioManager.Instance != null)
        {
            if (AudioManager.Instance.IsMusicPlaying())
            {
                AudioManager.Instance.PauseMusic();
            }
            else
            {
                AudioManager.Instance.ResumeMusic();
            }
        }
    }
}
