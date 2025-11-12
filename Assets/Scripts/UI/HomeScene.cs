using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace BrickDigger
{
    public class HomeScene : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Text levelText;
        [SerializeField] private Text coinsText;
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        
        [Header("Settings")]
        [SerializeField] private string gameplaySceneName = "GameplayScene";
        
        private int currentLevel;
        private int totalCoins;
        
        private void Start()
        {
            // Load player progress
            LoadProgress();
            
            // Update UI
            UpdateUI();
            
            // Setup buttons
            if (playButton != null)
            {
                playButton.onClick.AddListener(OnPlayClicked);
            }
            
            if (settingsButton != null)
            {
                settingsButton.onClick.AddListener(OnSettingsClicked);
            }
        }
        
        private void LoadProgress()
        {
            currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
            totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        }
        
        private void UpdateUI()
        {
            if (levelText != null)
            {
                levelText.text = $"Level {currentLevel}";
            }
            
            if (coinsText != null)
            {
                coinsText.text = $"Coins: {totalCoins}";
            }
        }
        
        private void OnPlayClicked()
        {
            // Save current level before loading
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
            PlayerPrefs.Save();
            
            // Load gameplay scene
            SceneManager.LoadScene(gameplaySceneName);
        }
        
        private void OnSettingsClicked()
        {
            // TODO: Open settings panel
            Debug.Log("Settings clicked");
        }
    }
}