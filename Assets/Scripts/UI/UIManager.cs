using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace BrickDigger
{
    public class UIManager : MonoBehaviour
    {
        [Header("HUD Elements")]
        [SerializeField] private Text axesText;
        [SerializeField] private Text coinsText;
        [SerializeField] private Text pieceProgressText;
        [SerializeField] private Image pieceProgressBar;
        [SerializeField] private Button buyAxesButton;
        
        [Header("Win/Lose Panels")]
        [SerializeField] private GameObject winPanel;
        [SerializeField] private Text winCoinsText;
        [SerializeField] private Text winBonusText;
        [SerializeField] private Text winAxesText;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Button retryButtonWin;
        
        [SerializeField] private GameObject losePanel;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button buyAxesButtonLose;
        [SerializeField] private Button homeButton;
        
        [Header("Effects")]
        [SerializeField] private GameObject coinCollectPrefab;
        [SerializeField] private Transform effectsContainer;
        
        [Header("Mobile Controls")]
        [SerializeField] private MobileJoystick joystick;
        [SerializeField] private Button jumpButton;
        [SerializeField] private Button digButton;
        
        private GameManager gameManager;
        private PlayerController playerController;
        
        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
            playerController = FindObjectOfType<PlayerController>();
            
            // Setup button listeners
            if (buyAxesButton != null)
                buyAxesButton.onClick.AddListener(() => gameManager?.BuyAxes());
                
            if (nextLevelButton != null)
                nextLevelButton.onClick.AddListener(() => gameManager?.NextLevel());
                
            if (retryButton != null)
                retryButton.onClick.AddListener(() => gameManager?.RestartLevel());
                
            if (retryButtonWin != null)
                retryButtonWin.onClick.AddListener(() => gameManager?.RestartLevel());
                
            if (buyAxesButtonLose != null)
                buyAxesButtonLose.onClick.AddListener(() => {
                    gameManager?.BuyAxes();
                    HideLosePanel();
                });
                
            if (homeButton != null)
                homeButton.onClick.AddListener(() => LoadMainMenu());
                
            // Setup mobile controls
            if (jumpButton != null)
                jumpButton.onClick.AddListener(() => playerController?.OnJump());
                
            if (digButton != null)
                digButton.onClick.AddListener(() => playerController?.OnDig());
            
            // Hide panels initially
            if (winPanel != null) winPanel.SetActive(false);
            if (losePanel != null) losePanel.SetActive(false);
        }
        
        private void Update()
        {
            // Update joystick input
            if (joystick != null && playerController != null)
            {
                Vector2 input = joystick.GetInput();
                playerController.SetMoveInput(input);
            }
        }
        
        public void UpdateAxesDisplay(int axes)
        {
            if (axesText != null)
                axesText.text = $"Axes: {axes}";
                
            // Update buy button visibility
            if (buyAxesButton != null)
            {
                buyAxesButton.gameObject.SetActive(axes < 5);
            }
        }
        
        public void UpdateCoinsDisplay(int levelCoins, int totalCoins)
        {
            if (coinsText != null)
                coinsText.text = $"Coins: {levelCoins} ({totalCoins})";
        }
        
        public void UpdatePieceProgress(int revealed, int total)
        {
            if (pieceProgressText != null)
                pieceProgressText.text = $"Piece: {revealed}/{total}";
                
            if (pieceProgressBar != null && total > 0)
            {
                pieceProgressBar.fillAmount = (float)revealed / total;
            }
        }
        
        public void ShowWinPanel(int levelCoins, int bonusCoins, int axesLeft)
        {
            if (winPanel != null)
            {
                winPanel.SetActive(true);
                
                if (winCoinsText != null)
                    winCoinsText.text = $"Coins Collected: {levelCoins}";
                if (winBonusText != null)
                    winBonusText.text = $"Win Bonus: +{bonusCoins}";
                if (winAxesText != null)
                    winAxesText.text = $"Axes Saved: {axesLeft}";
            }
        }
        
        public void ShowLosePanel(bool canBuyAxes)
        {
            if (losePanel != null)
            {
                losePanel.SetActive(true);
                
                if (buyAxesButtonLose != null)
                    buyAxesButtonLose.gameObject.SetActive(canBuyAxes);
            }
        }
        
        public void HideLosePanel()
        {
            if (losePanel != null)
                losePanel.SetActive(false);
        }
        
        public void ShowCoinCollectEffect()
        {
            if (coinCollectPrefab != null && effectsContainer != null)
            {
                GameObject effect = Instantiate(coinCollectPrefab, effectsContainer);
                Destroy(effect, 2f);
            }
            else
            {
                // Simple text animation if no prefab
                StartCoroutine(CoinCollectAnimation());
            }
        }
        
        private IEnumerator CoinCollectAnimation()
        {
            if (coinsText != null)
            {
                Vector3 originalScale = coinsText.transform.localScale;
                coinsText.transform.localScale = originalScale * 1.5f;
                yield return new WaitForSeconds(0.2f);
                coinsText.transform.localScale = originalScale;
            }
        }
        
        private void LoadMainMenu()
        {
            // For now, just restart the level
            // In a full game, this would load the main menu scene
            gameManager?.StartLevel(1);
        }
    }
}