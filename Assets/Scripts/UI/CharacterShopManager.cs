using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterShopManager : MonoBehaviour
{
    [Header("Character Settings")]
    [SerializeField] private int totalCharactersForSale = 8; // Character_1 to Character_8
    [SerializeField] private int characterPrice = 20;
    [SerializeField] private Transform characterSpawnPoint;
    [SerializeField] private string characterPrefabPath = "Assets/Layer lab/3D Casual Character/3D Casual Character/Prefabs/Characters/";
    
    [Header("UI References")]
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button actionButton; // Buy/Equip/Selected button
    [SerializeField] private Text actionButtonText;
    [SerializeField] private Text characterNameText;
    [SerializeField] private Text coinsText;
    
    [Header("Animator Controller")]
    [SerializeField] private RuntimeAnimatorController idleAnimatorController;
    
    // Current state
    private int currentCharacterIndex = 1;
    private int selectedCharacterIndex = 1;
    private int playerCoins = 0;
    private HashSet<int> unlockedCharacters = new HashSet<int>();
    private GameObject currentCharacterPreview;
    
    // PlayerPrefs keys
    private const string SELECTED_CHARACTER_KEY = "SelectedCharacter";
    private const string UNLOCKED_CHARACTERS_KEY = "UnlockedCharacters";
    private const string TOTAL_COINS_KEY = "TotalCoins";

    private void Start()
    {
        LoadPlayerData();
        SetupButtons();
        ShowCharacter(selectedCharacterIndex);
        UpdateUI();
    }

    private void LoadPlayerData()
    {
        // Load selected character
        selectedCharacterIndex = PlayerPrefs.GetInt(SELECTED_CHARACTER_KEY, 1);
        currentCharacterIndex = selectedCharacterIndex;
        
        // Load total coins
        playerCoins = PlayerPrefs.GetInt(TOTAL_COINS_KEY, 0);
        
        // Load unlocked characters
        unlockedCharacters.Clear();
        unlockedCharacters.Add(1); // Character_1 is always unlocked
        
        string unlockedData = PlayerPrefs.GetString(UNLOCKED_CHARACTERS_KEY, "1");
        string[] unlockedArray = unlockedData.Split(',');
        foreach (string indexStr in unlockedArray)
        {
            if (int.TryParse(indexStr, out int index))
            {
                unlockedCharacters.Add(index);
            }
        }
        
        Debug.Log($"Loaded: Selected={selectedCharacterIndex}, Coins={playerCoins}, Unlocked={unlockedCharacters.Count}");
    }

    private void SavePlayerData()
    {
        // Save selected character
        PlayerPrefs.SetInt(SELECTED_CHARACTER_KEY, selectedCharacterIndex);
        
        // Save unlocked characters
        List<int> unlockedList = new List<int>(unlockedCharacters);
        unlockedList.Sort();
        string unlockedData = string.Join(",", unlockedList);
        PlayerPrefs.SetString(UNLOCKED_CHARACTERS_KEY, unlockedData);
        
        // Save coins
        PlayerPrefs.SetInt(TOTAL_COINS_KEY, playerCoins);
        
        PlayerPrefs.Save();
        Debug.Log($"Saved: Selected={selectedCharacterIndex}, Coins={playerCoins}");
    }

    private void SetupButtons()
    {
        if (prevButton != null)
            prevButton.onClick.AddListener(OnPrevCharacter);
        
        if (nextButton != null)
            nextButton.onClick.AddListener(OnNextCharacter);
        
        if (actionButton != null)
            actionButton.onClick.AddListener(OnActionButtonClicked);
    }

    private void OnPrevCharacter()
    {
        currentCharacterIndex--;
        if (currentCharacterIndex < 1)
            currentCharacterIndex = totalCharactersForSale;
        
        ShowCharacter(currentCharacterIndex);
        UpdateUI();
    }

    private void OnNextCharacter()
    {
        currentCharacterIndex++;
        if (currentCharacterIndex > totalCharactersForSale)
            currentCharacterIndex = 1;
        
        ShowCharacter(currentCharacterIndex);
        UpdateUI();
    }

    private void OnActionButtonClicked()
    {
        bool isUnlocked = unlockedCharacters.Contains(currentCharacterIndex);
        bool isSelected = currentCharacterIndex == selectedCharacterIndex;
        
        if (!isUnlocked)
        {
            // Try to buy
            if (playerCoins >= characterPrice)
            {
                playerCoins -= characterPrice;
                unlockedCharacters.Add(currentCharacterIndex);
                SavePlayerData();
                UpdateUI();
                
                Debug.Log($"Purchased Character_{currentCharacterIndex} for {characterPrice} coins");
                
                // Play purchase sound
                if (AudioManager.Instance != null)
                {
                    // AudioManager.Instance.PlaySFX("Purchase");
                }
            }
            else
            {
                Debug.Log("Not enough coins!");
                // Show feedback - not enough coins
            }
        }
        else if (!isSelected)
        {
            // Equip this character
            selectedCharacterIndex = currentCharacterIndex;
            SavePlayerData();
            UpdateUI();
            
            Debug.Log($"Equipped Character_{selectedCharacterIndex}");
            
            // Play equip sound
            if (AudioManager.Instance != null)
            {
                // AudioManager.Instance.PlaySFX("Equip");
            }
        }
        // If already selected, button does nothing (shows "SELECTED")
    }

    private void ShowCharacter(int characterIndex)
    {
        // Destroy previous preview
        if (currentCharacterPreview != null)
        {
            Destroy(currentCharacterPreview);
        }
        
        // Load character prefab
        string prefabPath = $"{characterPrefabPath}Character_{characterIndex}.prefab";
        
#if UNITY_EDITOR
        GameObject characterPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
#else
        GameObject characterPrefab = Resources.Load<GameObject>(GetResourcePath(prefabPath));
#endif
        
        if (characterPrefab == null)
        {
            Debug.LogError($"Failed to load character prefab: {prefabPath}");
            return;
        }
        
        // Spawn character at preview point
        Transform spawnPoint = characterSpawnPoint != null ? characterSpawnPoint : transform;
        
#if UNITY_EDITOR
        currentCharacterPreview = UnityEditor.PrefabUtility.InstantiatePrefab(characterPrefab, spawnPoint) as GameObject;
#else
        currentCharacterPreview = Instantiate(characterPrefab, spawnPoint);
#endif
        
        currentCharacterPreview.name = $"CharacterPreview_{characterIndex}";
        currentCharacterPreview.transform.localPosition = Vector3.zero;
        currentCharacterPreview.transform.localRotation = Quaternion.identity;
        currentCharacterPreview.transform.localScale = Vector3.one;
        
        // Setup animator with idle animation
        Animator animator = currentCharacterPreview.GetComponentInChildren<Animator>();
        if (animator != null && idleAnimatorController != null)
        {
            animator.runtimeAnimatorController = idleAnimatorController;
            animator.SetFloat("Speed", 0f); // Ensure idle
        }
        
        Debug.Log($"Showing Character_{characterIndex}");
    }

    private void UpdateUI()
    {
        // Update character name
        if (characterNameText != null)
        {
            characterNameText.text = $"Character {currentCharacterIndex}";
        }
        
        // Update coins display
        if (coinsText != null)
        {
            coinsText.text = $"Coins: {playerCoins}";
        }
        
        // Update action button
        UpdateActionButton();
    }

    private void UpdateActionButton()
    {
        if (actionButton == null || actionButtonText == null)
            return;
        
        bool isUnlocked = unlockedCharacters.Contains(currentCharacterIndex);
        bool isSelected = currentCharacterIndex == selectedCharacterIndex;
        
        if (!isUnlocked)
        {
            // Show BUY button
            actionButtonText.text = $"BUY ({characterPrice})";
            
            // Green if affordable, grey if not
            Image buttonImage = actionButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                if (playerCoins >= characterPrice)
                {
                    buttonImage.color = new Color(0.2f, 0.8f, 0.2f, 1f); // Green
                    actionButton.interactable = true;
                }
                else
                {
                    buttonImage.color = new Color(0.5f, 0.5f, 0.5f, 1f); // Grey
                    actionButton.interactable = false;
                }
            }
        }
        else if (!isSelected)
        {
            // Show EQUIP button
            actionButtonText.text = "EQUIP";
            
            Image buttonImage = actionButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = new Color(0.2f, 0.6f, 1f, 1f); // Blue
                actionButton.interactable = true;
            }
        }
        else
        {
            // Show SELECTED button (disabled)
            actionButtonText.text = "SELECTED";
            
            Image buttonImage = actionButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = new Color(0.3f, 0.3f, 0.3f, 1f); // Dark grey
                actionButton.interactable = false;
            }
        }
    }

    private string GetResourcePath(string fullPath)
    {
        return fullPath.Replace("Assets/", "").Replace(".prefab", "");
    }

    public int GetSelectedCharacter()
    {
        return selectedCharacterIndex;
    }

    public bool IsCharacterUnlocked(int characterIndex)
    {
        return unlockedCharacters.Contains(characterIndex);
    }

    // Public method to add coins (called from GameManager)
    public void AddCoins(int amount)
    {
        playerCoins += amount;
        SavePlayerData();
        UpdateUI();
    }

    // Public method to sync coins from GameManager
    public void SyncCoins(int totalCoins)
    {
        playerCoins = totalCoins;
        UpdateUI();
    }
}
