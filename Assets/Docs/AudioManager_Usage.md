# AudioManager Usage Guide

## Overview
The AudioManager is a singleton that handles all audio in the game, including background music and sound effects. It integrates with the SettingsManager to respect user preferences.

## Features
- ✅ Singleton pattern (accessible from anywhere via `AudioManager.Instance`)
- ✅ Persistent across scenes (DontDestroyOnLoad)
- ✅ Separate control for music and sound effects
- ✅ Audio source pooling for overlapping sound effects
- ✅ Integration with Settings (on/off control)
- ✅ Support for looping background music
- ✅ One-shot sound effects
- ✅ Play audio by clip, index, or name

## Setup

### 1. AudioManager GameObject
The AudioManager GameObject should exist in your first scene (HomeScene). It will persist across all scenes automatically.

### 2. Assigning Audio Clips
Select the AudioManager GameObject in the hierarchy and assign your audio clips in the Inspector:
- **Music Clips**: Array of background music tracks
- **SFX Clips**: Array of sound effect clips

## Usage Examples

### Playing Background Music

```csharp
// Play music by AudioClip reference
AudioManager.Instance.PlayMusic(myMusicClip, loop: true);

// Play music by index (from musicClips array)
AudioManager.Instance.PlayMusic(0, loop: true);

// Play music by name
AudioManager.Instance.PlayMusicByName("MenuMusic", loop: true);

// Stop music
AudioManager.Instance.StopMusic();

// Pause music
AudioManager.Instance.PauseMusic();

// Resume music
AudioManager.Instance.ResumeMusic();

// Check if music is playing
bool isPlaying = AudioManager.Instance.IsMusicPlaying();
```

### Playing Sound Effects

```csharp
// Play SFX by AudioClip reference
AudioManager.Instance.PlaySFX(mySoundClip);

// Play SFX with custom volume (0-1)
AudioManager.Instance.PlaySFX(mySoundClip, volumeScale: 0.5f);

// Play SFX by index (from sfxClips array)
AudioManager.Instance.PlaySFX(0);

// Play SFX by name
AudioManager.Instance.PlaySFXByName("CoinCollect");
```

### Volume Control

```csharp
// Enable/disable music (respects settings)
AudioManager.Instance.SetMusicEnabled(true);

// Enable/disable sound effects (respects settings)
AudioManager.Instance.SetSFXEnabled(true);

// Set music volume (0-1)
AudioManager.Instance.SetMusicVolume(0.8f);

// Set SFX volume (0-1)
AudioManager.Instance.SetSFXVolume(0.7f);

// Get current states
bool musicEnabled = AudioManager.Instance.IsMusicEnabled();
bool sfxEnabled = AudioManager.Instance.IsSFXEnabled();
float musicVol = AudioManager.Instance.GetMusicVolume();
float sfxVol = AudioManager.Instance.GetSFXVolume();
```

## Integration with Game Systems

### In PlayerController (Digging)
```csharp
public void Dig()
{
    // Play dig sound
    AudioManager.Instance.PlaySFX(digSound);
    
    // ... rest of dig logic
}
```

### In Coin Collection
```csharp
private void OnCoinCollect()
{
    AudioManager.Instance.PlaySFX(coinCollectSound, volumeScale: 0.8f);
    // ... rest of collection logic
}
```

### In UI Buttons
```csharp
public void OnButtonClick()
{
    AudioManager.Instance.PlaySFX(buttonClickSound);
    // ... rest of button logic
}
```

### In Scene Loading
```csharp
private void Start()
{
    // Play appropriate music for the scene
    if (SceneManager.GetActiveScene().name == "HomeScene")
    {
        AudioManager.Instance.PlayMusicByName("MenuMusic", loop: true);
    }
    else if (SceneManager.GetActiveScene().name == "GameplayScene")
    {
        AudioManager.Instance.PlayMusicByName("GameplayMusic", loop: true);
    }
}
```

## Settings Integration

The AudioManager automatically integrates with the SettingsManager:
- When the user toggles **Sound** in settings, it calls `AudioManager.Instance.SetSFXEnabled()`
- When the user toggles **Music** in settings, it calls `AudioManager.Instance.SetMusicEnabled()`
- Settings are saved in PlayerPrefs and persist between sessions
- When disabled, volume is set to 0 (muted)

## Audio Source Pooling

The AudioManager uses a pool of AudioSources for sound effects to allow multiple sounds to play simultaneously without cutting each other off. The pool automatically expands if needed.

## Best Practices

1. **Always check for null**: 
   ```csharp
   if (AudioManager.Instance != null)
   {
       AudioManager.Instance.PlaySFX(clip);
   }
   ```

2. **Use appropriate volume scales**: For subtle sounds, use lower volume scales (0.3-0.7)

3. **Organize your clips**: Use the Inspector arrays to organize music and SFX clips by category

4. **Name your clips clearly**: Use descriptive names like "CoinCollect", "ButtonClick", "MenuMusic"

5. **Don't destroy AudioManager**: It persists across scenes automatically via DontDestroyOnLoad

## Troubleshooting

**No sound playing?**
- Check if AudioManager GameObject exists in the scene
- Verify audio clips are assigned in the Inspector
- Check if settings have sound/music enabled
- Ensure AudioListener exists in the scene (usually on the Camera)

**Sound cutting off?**
- The audio source pool will automatically expand, but check if you're playing too many sounds simultaneously

**Settings not working?**
- Ensure SettingsManager is properly set up and references AudioManager
- Check PlayerPrefs are being saved correctly

## Future Enhancements

Possible additions you might want to implement:
- Volume sliders (0-100%) instead of just on/off
- Crossfade between music tracks
- Audio ducking (lower music when SFX plays)
- 3D spatial audio for positional sounds
- Audio mixer groups for advanced control
- Pitch variation for sound effects
