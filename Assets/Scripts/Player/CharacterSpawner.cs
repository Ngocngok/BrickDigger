using UnityEngine;

namespace BrickDigger
{
    /// <summary>
    /// Handles spawning and managing the player character model
    /// </summary>
    public class CharacterSpawner : MonoBehaviour
    {
        [Header("Character Settings")]
        [SerializeField] private string characterPrefabPath = "Assets/Layer lab/3D Casual Character/3D Casual Character/Prefabs/Characters/";
        [SerializeField] private int defaultCharacterIndex = 1; // Character_1
        [SerializeField] private Transform spawnParent; // Where to spawn the character (usually Player GameObject)
        [SerializeField] private Vector3 localPosition = Vector3.zero;
        [SerializeField] private Vector3 localRotation = Vector3.zero;
        [SerializeField] private float characterScale = 1f;
        
        [Header("References")]
        [SerializeField] private Animator characterAnimator;
        [SerializeField] private GameObject visualToReplace; // The Capsule to hide/remove
        
        // PlayerPrefs key for character selection
        private const string CHARACTER_SELECTION_KEY = "SelectedCharacter";
        
        private GameObject spawnedCharacter;
        private int currentCharacterIndex;

        private void Start()
        {
            SpawnCharacter();
        }

        /// <summary>
        /// Spawn the selected character
        /// </summary>
        public void SpawnCharacter()
        {
            // Load saved character selection or use default
            currentCharacterIndex = PlayerPrefs.GetInt(CHARACTER_SELECTION_KEY, defaultCharacterIndex);
            
            // Spawn the character
            SpawnCharacter(currentCharacterIndex);
        }

        /// <summary>
        /// Spawn a specific character by index
        /// </summary>
        public void SpawnCharacter(int characterIndex)
        {
            // Destroy previous character if exists
            if (spawnedCharacter != null)
            {
                Destroy(spawnedCharacter);
            }
            
            // Build the prefab path
            string prefabPath = $"{characterPrefabPath}Character_{characterIndex}.prefab";
            
            // Load the character prefab
            GameObject characterPrefab = UnityEngine.Resources.Load<GameObject>(GetResourcePath(prefabPath));
            
            if (characterPrefab == null)
            {
                Debug.LogError($"CharacterSpawner: Failed to load character prefab at {prefabPath}");
                return;
            }
            
            // Determine spawn parent (use this transform if not specified)
            Transform parent = spawnParent != null ? spawnParent : transform;
            
            // Instantiate the character
            spawnedCharacter = Instantiate(characterPrefab, parent);
            spawnedCharacter.name = $"Character_{characterIndex}_Model";
            
            // Set local transform
            spawnedCharacter.transform.localPosition = localPosition;
            spawnedCharacter.transform.localRotation = Quaternion.Euler(localRotation);
            spawnedCharacter.transform.localScale = Vector3.one * characterScale;
            
            // Get the Animator component from the spawned character
            characterAnimator = spawnedCharacter.GetComponentInChildren<Animator>();
            
            if (characterAnimator == null)
            {
                Debug.LogWarning($"CharacterSpawner: No Animator found on spawned character {characterIndex}");
            }
            else
            {
                Debug.Log($"CharacterSpawner: Successfully spawned Character_{characterIndex} with Animator");
            }
            
            // Hide or remove the visual placeholder (Capsule)
            if (visualToReplace != null)
            {
                visualToReplace.SetActive(false);
            }
            
            // Save the current selection
            currentCharacterIndex = characterIndex;
            PlayerPrefs.SetInt(CHARACTER_SELECTION_KEY, characterIndex);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Change to a different character
        /// </summary>
        public void ChangeCharacter(int newCharacterIndex)
        {
            SpawnCharacter(newCharacterIndex);
        }

        /// <summary>
        /// Get the current character's animator
        /// </summary>
        public Animator GetAnimator()
        {
            return characterAnimator;
        }

        /// <summary>
        /// Get the currently spawned character GameObject
        /// </summary>
        public GameObject GetSpawnedCharacter()
        {
            return spawnedCharacter;
        }

        /// <summary>
        /// Get the current character index
        /// </summary>
        public int GetCurrentCharacterIndex()
        {
            return currentCharacterIndex;
        }

        /// <summary>
        /// Convert full asset path to Resources path
        /// </summary>
        private string GetResourcePath(string fullPath)
        {
            // Remove "Assets/" and file extension for Resources.Load
            string resourcePath = fullPath.Replace("Assets/", "").Replace(".prefab", "");
            return resourcePath;
        }

        /// <summary>
        /// Load character directly from asset path (Editor only)
        /// </summary>
        public void SpawnCharacterFromAssetPath(int characterIndex)
        {
#if UNITY_EDITOR
            // Destroy previous character if exists
            if (spawnedCharacter != null)
            {
                DestroyImmediate(spawnedCharacter);
            }
            
            // Build the prefab path
            string prefabPath = $"{characterPrefabPath}Character_{characterIndex}.prefab";
            
            // Load the character prefab using AssetDatabase
            GameObject characterPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            
            if (characterPrefab == null)
            {
                Debug.LogError($"CharacterSpawner: Failed to load character prefab at {prefabPath}");
                return;
            }
            
            // Determine spawn parent
            Transform parent = spawnParent != null ? spawnParent : transform;
            
            // Instantiate the character
            spawnedCharacter = UnityEditor.PrefabUtility.InstantiatePrefab(characterPrefab, parent) as GameObject;
            spawnedCharacter.name = $"Character_{characterIndex}_Model";
            
            // Set local transform
            spawnedCharacter.transform.localPosition = localPosition;
            spawnedCharacter.transform.localRotation = Quaternion.Euler(localRotation);
            spawnedCharacter.transform.localScale = Vector3.one * characterScale;
            
            // Get the Animator component
            characterAnimator = spawnedCharacter.GetComponentInChildren<Animator>();
            
            if (characterAnimator == null)
            {
                Debug.LogWarning($"CharacterSpawner: No Animator found on spawned character {characterIndex}");
            }
            
            // Hide the visual placeholder
            if (visualToReplace != null)
            {
                visualToReplace.SetActive(false);
            }
            
            // Save the current selection
            currentCharacterIndex = characterIndex;
            PlayerPrefs.SetInt(CHARACTER_SELECTION_KEY, characterIndex);
            PlayerPrefs.Save();
            
            Debug.Log($"CharacterSpawner: Successfully spawned Character_{characterIndex} in Editor");
#endif
        }
    }
}
