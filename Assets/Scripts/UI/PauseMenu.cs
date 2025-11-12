using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace BrickDigger
{
    public class PauseMenu : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button homeButton;
        [SerializeField] private Button pauseButton;
        
        [Header("Settings")]
        [SerializeField] private string homeSceneName = "HomeScene";
        
        private bool isPaused = false;
        private GameManager gameManager;
        
        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            
            // Setup buttons
            if (pauseButton != null)
            {
                pauseButton.onClick.AddListener(TogglePause);
            }
            
            if (resumeButton != null)
            {
                resumeButton.onClick.AddListener(Resume);
            }
            
            if (homeButton != null)
            {
                homeButton.onClick.AddListener(GoHome);
            }
            
            // Hide pause panel initially
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }
        }
        
        private void Update()
        {
            // Also allow ESC key to pause (for testing in editor)
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }
        
        public void TogglePause()
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        
        public void Pause()
        {
            isPaused = true;
            Time.timeScale = 0f; // Freeze game
            
            if (pausePanel != null)
            {
                pausePanel.SetActive(true);
            }
        }
        
        public void Resume()
        {
            isPaused = false;
            Time.timeScale = 1f; // Resume game
            
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }
        }
        
        public void GoHome()
        {
            // Resume time before loading
            Time.timeScale = 1f;
            
            // Save progress
            if (gameManager != null)
            {
                // Save is handled by GameManager
            }
            
            // Load home scene
            SceneManager.LoadScene(homeSceneName);
        }
    }
}