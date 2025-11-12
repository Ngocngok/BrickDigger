using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace BrickDigger
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Settings")]
        [SerializeField] private int currentLevel = 1;
        [SerializeField] private int totalCoins = 0;
        
        [Header("Level Settings")]
        [SerializeField] private int axesRemaining = 25;
        [SerializeField] private int levelCoins = 0;
        [SerializeField] private int totalPieces = 0;
        [SerializeField] private int revealedPieces = 0;
        
        [Header("References")]
        [SerializeField] private GridManager gridManager;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private UIManager uiManager;
        
        [Header("Events")]
        public UnityEvent OnLevelStart;
        public UnityEvent OnLevelWin;
        public UnityEvent OnLevelLose;
        public UnityEvent<int> OnAxesChanged;
        public UnityEvent<int> OnCoinsChanged;
        public UnityEvent<int, int> OnPieceRevealed; // current, total
        
        private LevelConfig currentLevelConfig;
        private bool isGameActive = false;
        
        private void Awake()
        {
            // Find references if not set
            if (gridManager == null)
                gridManager = FindObjectOfType<GridManager>();
            if (playerController == null)
                playerController = FindObjectOfType<PlayerController>();
            if (uiManager == null)
                uiManager = FindObjectOfType<UIManager>();
        }
        
        private void Start()
        {
            // Load saved progress
            LoadGameProgress();
            
            // Start first level
            StartLevel(currentLevel);
        }
        
        public void StartLevel(int levelNumber)
        {
            currentLevel = levelNumber;
            isGameActive = true;
            
            // Generate level config
            currentLevelConfig = LevelConfig.GenerateLevel(levelNumber);
            
            // Reset level state
            axesRemaining = currentLevelConfig.axesStart;
            levelCoins = 0;
            revealedPieces = 0;
            
            // Generate the level
            if (gridManager != null)
            {
                gridManager.GenerateLevel(currentLevelConfig);
                totalPieces = gridManager.CountLegoPieces();
            }
            
            // Place player at starting position
            if (playerController != null)
            {
                CellCoord startPos = new CellCoord(
                    currentLevelConfig.width / 2,
                    currentLevelConfig.height / 2
                );
                playerController.PlaceOnGrid(startPos);
            }
            
            // Update UI
            UpdateUI();
            
            // Fire event
            OnLevelStart?.Invoke();
        }
        
        public void RestartLevel()
        {
            StartLevel(currentLevel);
        }
        
        public void NextLevel()
        {
            currentLevel++;
            SaveGameProgress();
            StartLevel(currentLevel);
        }
        
        public bool CanDig()
        {
            return axesRemaining > 0 && isGameActive;
        }
        
        public void UseAxe()
        {
            if (axesRemaining > 0)
            {
                axesRemaining--;
                OnAxesChanged?.Invoke(axesRemaining);
                UpdateUI();
                
                // Check lose condition
                if (axesRemaining <= 0 && revealedPieces < totalPieces)
                {
                    StartCoroutine(LoseLevel());
                }
            }
        }
        
        public void CollectCoin()
        {
            levelCoins++;
            totalCoins++;
            OnCoinsChanged?.Invoke(levelCoins);
            UpdateUI();
            
            // Play coin collect effect
            if (uiManager != null)
            {
                uiManager.ShowCoinCollectEffect();
            }
        }
        
        public void RevealPiece()
        {
            revealedPieces++;
            OnPieceRevealed?.Invoke(revealedPieces, totalPieces);
            UpdateUI();
            
            // Check win condition
            if (revealedPieces >= totalPieces)
            {
                StartCoroutine(WinLevel());
            }
        }
        
        public void BuyAxes()
        {
            int axeCost = 5;
            int axePack = 3;
            
            if (totalCoins >= axeCost)
            {
                totalCoins -= axeCost;
                axesRemaining += axePack;
                OnAxesChanged?.Invoke(axesRemaining);
                OnCoinsChanged?.Invoke(totalCoins);
                UpdateUI();
            }
        }
        
        private IEnumerator WinLevel()
        {
            isGameActive = false;
            
            yield return new WaitForSeconds(0.5f);
            
            // Award bonus coins for winning
            int bonusCoins = 5;
            totalCoins += bonusCoins;
            
            // Show win UI
            if (uiManager != null)
            {
                uiManager.ShowWinPanel(levelCoins, bonusCoins, axesRemaining);
            }
            
            // Unlock next level
            int highestLevel = PlayerPrefs.GetInt("HighestLevel", 1);
            if (currentLevel >= highestLevel)
            {
                PlayerPrefs.SetInt("HighestLevel", currentLevel + 1);
            }
            
            SaveGameProgress();
            OnLevelWin?.Invoke();
        }
        
        private IEnumerator LoseLevel()
        {
            isGameActive = false;
            
            yield return new WaitForSeconds(0.5f);
            
            // Show lose UI
            if (uiManager != null)
            {
                uiManager.ShowLosePanel(totalCoins >= 5);
            }
            
            OnLevelLose?.Invoke();
        }
        
        private void UpdateUI()
        {
            if (uiManager != null)
            {
                uiManager.UpdateAxesDisplay(axesRemaining);
                uiManager.UpdateCoinsDisplay(levelCoins, totalCoins);
                uiManager.UpdatePieceProgress(revealedPieces, totalPieces);
            }
        }
        
        private void SaveGameProgress()
        {
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
            PlayerPrefs.SetInt("TotalCoins", totalCoins);
            PlayerPrefs.SetInt("HighestLevel", Mathf.Max(currentLevel, PlayerPrefs.GetInt("HighestLevel", 1)));
            PlayerPrefs.Save();
        }
        
        private void LoadGameProgress()
        {
            currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
            totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        }
        
        // Debug methods
        [ContextMenu("Add 10 Axes")]
        private void Debug_AddAxes()
        {
            axesRemaining += 10;
            UpdateUI();
        }
        
        [ContextMenu("Add 10 Coins")]
        private void Debug_AddCoins()
        {
            totalCoins += 10;
            UpdateUI();
        }
        
        [ContextMenu("Win Level")]
        private void Debug_WinLevel()
        {
            StartCoroutine(WinLevel());
        }
    }
}