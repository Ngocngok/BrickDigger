using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private AudioClip[] sfxClips;
    
    [Header("Settings")]
    [SerializeField] private float musicVolume = 1f;
    [SerializeField] private float sfxVolume = 1f;
    
    // Pool of audio sources for overlapping sound effects
    private List<AudioSource> sfxSourcePool = new List<AudioSource>();
    private const int INITIAL_SFX_POOL_SIZE = 5;
    
    // Current state
    private bool musicEnabled = true;
    private bool sfxEnabled = true;
    private int currentMusicIndex = -1;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        InitializeAudioSources();
    }

    private void InitializeAudioSources()
    {
        // Create music source if not assigned
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }
        
        // Create main SFX source if not assigned
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }
        
        // Create SFX source pool for overlapping sounds
        for (int i = 0; i < INITIAL_SFX_POOL_SIZE; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.loop = false;
            source.playOnAwake = false;
            sfxSourcePool.Add(source);
        }
        
        // Apply initial volumes
        UpdateMusicVolume();
        UpdateSFXVolume();
    }

    #region Music Control
    
    /// <summary>
    /// Play background music by clip
    /// </summary>
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioManager: Attempted to play null music clip");
            return;
        }
        
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }
    
    /// <summary>
    /// Play background music by index from musicClips array
    /// </summary>
    public void PlayMusic(int index, bool loop = true)
    {
        if (musicClips == null || index < 0 || index >= musicClips.Length)
        {
            Debug.LogWarning($"AudioManager: Invalid music index {index}");
            return;
        }
        
        currentMusicIndex = index;
        PlayMusic(musicClips[index], loop);
    }
    
    /// <summary>
    /// Play background music by name
    /// </summary>
    public void PlayMusicByName(string clipName, bool loop = true)
    {
        if (musicClips == null) return;
        
        for (int i = 0; i < musicClips.Length; i++)
        {
            if (musicClips[i] != null && musicClips[i].name == clipName)
            {
                PlayMusic(i, loop);
                return;
            }
        }
        
        Debug.LogWarning($"AudioManager: Music clip '{clipName}' not found");
    }
    
    /// <summary>
    /// Stop currently playing music
    /// </summary>
    public void StopMusic()
    {
        musicSource.Stop();
        currentMusicIndex = -1;
    }
    
    /// <summary>
    /// Pause currently playing music
    /// </summary>
    public void PauseMusic()
    {
        musicSource.Pause();
    }
    
    /// <summary>
    /// Resume paused music
    /// </summary>
    public void ResumeMusic()
    {
        musicSource.UnPause();
    }
    
    /// <summary>
    /// Check if music is currently playing
    /// </summary>
    public bool IsMusicPlaying()
    {
        return musicSource.isPlaying;
    }
    
    #endregion

    #region Sound Effects Control
    
    /// <summary>
    /// Play a one-shot sound effect
    /// </summary>
    public void PlaySFX(AudioClip clip, float volumeScale = 1f)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioManager: Attempted to play null SFX clip");
            return;
        }
        
        if (!sfxEnabled) return;
        
        // Try to find an available audio source from the pool
        AudioSource availableSource = GetAvailableSFXSource();
        
        if (availableSource != null)
        {
            availableSource.volume = sfxVolume * volumeScale;
            availableSource.PlayOneShot(clip);
        }
        else
        {
            // Fallback to main SFX source
            sfxSource.PlayOneShot(clip, volumeScale);
        }
    }
    
    /// <summary>
    /// Play a sound effect by index from sfxClips array
    /// </summary>
    public void PlaySFX(int index, float volumeScale = 1f)
    {
        if (sfxClips == null || index < 0 || index >= sfxClips.Length)
        {
            Debug.LogWarning($"AudioManager: Invalid SFX index {index}");
            return;
        }
        
        PlaySFX(sfxClips[index], volumeScale);
    }
    
    /// <summary>
    /// Play a sound effect by name
    /// </summary>
    public void PlaySFXByName(string clipName, float volumeScale = 1f)
    {
        if (sfxClips == null) return;
        
        for (int i = 0; i < sfxClips.Length; i++)
        {
            if (sfxClips[i] != null && sfxClips[i].name == clipName)
            {
                PlaySFX(i, volumeScale);
                return;
            }
        }
        
        Debug.LogWarning($"AudioManager: SFX clip '{clipName}' not found");
    }
    
    /// <summary>
    /// Get an available audio source from the pool
    /// </summary>
    private AudioSource GetAvailableSFXSource()
    {
        foreach (AudioSource source in sfxSourcePool)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        
        // If all sources are busy, create a new one
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.loop = false;
        newSource.playOnAwake = false;
        sfxSourcePool.Add(newSource);
        
        return newSource;
    }
    
    #endregion

    #region Volume Control
    
    /// <summary>
    /// Set music enabled/disabled
    /// </summary>
    public void SetMusicEnabled(bool enabled)
    {
        musicEnabled = enabled;
        UpdateMusicVolume();
    }
    
    /// <summary>
    /// Set sound effects enabled/disabled
    /// </summary>
    public void SetSFXEnabled(bool enabled)
    {
        sfxEnabled = enabled;
        UpdateSFXVolume();
    }
    
    /// <summary>
    /// Set music volume (0-1)
    /// </summary>
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        UpdateMusicVolume();
    }
    
    /// <summary>
    /// Set SFX volume (0-1)
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        UpdateSFXVolume();
    }
    
    /// <summary>
    /// Update music source volume based on enabled state
    /// </summary>
    private void UpdateMusicVolume()
    {
        musicSource.volume = musicEnabled ? musicVolume : 0f;
    }
    
    /// <summary>
    /// Update all SFX sources volume based on enabled state
    /// </summary>
    private void UpdateSFXVolume()
    {
        float volume = sfxEnabled ? sfxVolume : 0f;
        
        sfxSource.volume = volume;
        
        foreach (AudioSource source in sfxSourcePool)
        {
            source.volume = volume;
        }
    }
    
    #endregion

    #region Getters
    
    public bool IsMusicEnabled() => musicEnabled;
    public bool IsSFXEnabled() => sfxEnabled;
    public float GetMusicVolume() => musicVolume;
    public float GetSFXVolume() => sfxVolume;
    
    #endregion
}
