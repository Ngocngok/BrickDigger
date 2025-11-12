using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Settings Popup")]
    [SerializeField] private GameObject settingsPopup;
    
    [Header("Setting Buttons")]
    [SerializeField] private Button soundButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button hapticsButton;
    [SerializeField] private Button closeButton;
    
    [Header("Button Images")]
    [SerializeField] private Image soundButtonImage;
    [SerializeField] private Image musicButtonImage;
    [SerializeField] private Image hapticsButtonImage;
    
    [Header("On/Off Sprites")]
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private Sprite musicOnSprite;
    [SerializeField] private Sprite musicOffSprite;
    [SerializeField] private Sprite hapticsOnSprite;
    [SerializeField] private Sprite hapticsOffSprite;
    
    // PlayerPrefs keys
    private const string SOUND_KEY = "Settings_Sound";
    private const string MUSIC_KEY = "Settings_Music";
    private const string HAPTICS_KEY = "Settings_Haptics";
    
    // Current states
    private bool soundEnabled = true;
    private bool musicEnabled = true;
    private bool hapticsEnabled = true;
    
    private void Start()
    {
        // Load saved settings
        LoadSettings();
        
        // Update button visuals
        UpdateButtonVisuals();
        
        // Hide popup initially
        if (settingsPopup != null)
        {
            settingsPopup.SetActive(false);
        }
        
        // Setup button listeners
        if (soundButton != null)
            soundButton.onClick.AddListener(ToggleSound);
        
        if (musicButton != null)
            musicButton.onClick.AddListener(ToggleMusic);
        
        if (hapticsButton != null)
            hapticsButton.onClick.AddListener(ToggleHaptics);
        
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseSettings);
    }
    
    private void LoadSettings()
    {
        // Load settings from PlayerPrefs (default to 1 = enabled)
        soundEnabled = PlayerPrefs.GetInt(SOUND_KEY, 1) == 1;
        musicEnabled = PlayerPrefs.GetInt(MUSIC_KEY, 1) == 1;
        hapticsEnabled = PlayerPrefs.GetInt(HAPTICS_KEY, 1) == 1;
        
        // Apply settings to AudioManager if it exists
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXEnabled(soundEnabled);
            AudioManager.Instance.SetMusicEnabled(musicEnabled);
        }
    }
    
    private void SaveSettings()
    {
        PlayerPrefs.SetInt(SOUND_KEY, soundEnabled ? 1 : 0);
        PlayerPrefs.SetInt(MUSIC_KEY, musicEnabled ? 1 : 0);
        PlayerPrefs.SetInt(HAPTICS_KEY, hapticsEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    public void ShowSettings()
    {
        if (settingsPopup != null)
        {
            settingsPopup.SetActive(true);
        }
    }
    
    public void CloseSettings()
    {
        if (settingsPopup != null)
        {
            settingsPopup.SetActive(false);
        }
    }
    
    private void ToggleSound()
    {
        soundEnabled = !soundEnabled;
        SaveSettings();
        UpdateButtonVisuals();
        
        // Update AudioManager
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXEnabled(soundEnabled);
        }
    }
    
    private void ToggleMusic()
    {
        musicEnabled = !musicEnabled;
        SaveSettings();
        UpdateButtonVisuals();
        
        // Update AudioManager
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicEnabled(musicEnabled);
        }
    }
    
    private void ToggleHaptics()
    {
        hapticsEnabled = !hapticsEnabled;
        SaveSettings();
        UpdateButtonVisuals();
        
        // Here you can add actual haptics control logic
        // For example: Handheld.Vibrate() control
    }
    
    private void UpdateButtonVisuals()
    {
        // Update sound button
        if (soundButtonImage != null && soundOnSprite != null && soundOffSprite != null)
        {
            soundButtonImage.sprite = soundEnabled ? soundOnSprite : soundOffSprite;
        }
        
        // Update music button
        if (musicButtonImage != null && musicOnSprite != null && musicOffSprite != null)
        {
            musicButtonImage.sprite = musicEnabled ? musicOnSprite : musicOffSprite;
        }
        
        // Update haptics button
        if (hapticsButtonImage != null && hapticsOnSprite != null && hapticsOffSprite != null)
        {
            hapticsButtonImage.sprite = hapticsEnabled ? hapticsOnSprite : hapticsOffSprite;
        }
    }
    
    // Public getters for other scripts to check settings
    public bool IsSoundEnabled() => soundEnabled;
    public bool IsMusicEnabled() => musicEnabled;
    public bool IsHapticsEnabled() => hapticsEnabled;
}
