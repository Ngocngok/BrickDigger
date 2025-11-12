using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace BrickDigger
{
    public class LoadingScene : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image loadingBar;
        [SerializeField] private Text percentageText;
        
        [Header("Settings")]
        [SerializeField] private float loadingDuration = 2f;
        [SerializeField] private string nextSceneName = "HomeScene";
        
        private void Start()
        {
            StartCoroutine(LoadingSequence());
        }
        
        private IEnumerator LoadingSequence()
        {
            float elapsed = 0f;
            
            while (elapsed < loadingDuration)
            {
                elapsed += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsed / loadingDuration);
                
                // Update loading bar
                if (loadingBar != null)
                {
                    loadingBar.fillAmount = progress;
                }
                
                // Update percentage text
                if (percentageText != null)
                {
                    percentageText.text = $"{Mathf.RoundToInt(progress * 100)}%";
                }
                
                yield return null;
            }
            
            // Ensure 100%
            if (loadingBar != null) loadingBar.fillAmount = 1f;
            if (percentageText != null) percentageText.text = "100%";
            
            yield return new WaitForSeconds(0.3f);
            
            // Load next scene
            SceneManager.LoadScene(nextSceneName);
        }
    }
}