using UnityEngine;

namespace BrickDigger
{
    /// <summary>
    /// Test script to verify win/lose conditions
    /// Add to GameManager and use menu commands
    /// </summary>
    public class TestWinLoseConditions : MonoBehaviour
    {
        [UnityEditor.MenuItem("Tools/Test Win Condition")]
        public static void TestWin()
        {
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            GridManager gridManager = FindFirstObjectByType<GridManager>();
            
            if (gameManager == null || gridManager == null)
            {
                Debug.LogError("GameManager or GridManager not found!");
                return;
            }
            
            // Get total pieces
            int totalPieces = gridManager.CountLegoPieces();
            Debug.Log($"Total Lego pieces in level: {totalPieces}");
            
            // Reveal all pieces by calling RevealPiece multiple times
            for (int i = 0; i < totalPieces; i++)
            {
                gameManager.RevealPiece();
            }
            
            Debug.Log("All pieces revealed - Win condition should trigger!");
        }
        
        [UnityEditor.MenuItem("Tools/Test Lose Condition")]
        public static void TestLose()
        {
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            
            if (gameManager == null)
            {
                Debug.LogError("GameManager not found!");
                return;
            }
            
            // Use all axes
            while (gameManager.CanDig())
            {
                gameManager.UseAxe();
            }
            
            Debug.Log("All axes used - Lose condition should trigger!");
        }
        
        [UnityEditor.MenuItem("Tools/Show Level Info")]
        public static void ShowLevelInfo()
        {
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            GridManager gridManager = FindFirstObjectByType<GridManager>();
            
            if (gameManager == null || gridManager == null)
            {
                Debug.LogError("GameManager or GridManager not found!");
                return;
            }
            
            Debug.Log("=== LEVEL INFO ===");
            
            // Use reflection to get private fields
            var axesField = typeof(GameManager).GetField("axesRemaining", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var revealedField = typeof(GameManager).GetField("revealedPieces", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var totalPiecesField = typeof(GameManager).GetField("totalPieces", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var levelField = typeof(GameManager).GetField("currentLevel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            int axes = (int)axesField.GetValue(gameManager);
            int revealed = (int)revealedField.GetValue(gameManager);
            int total = (int)totalPiecesField.GetValue(gameManager);
            int level = (int)levelField.GetValue(gameManager);
            
            int totalInGrid = gridManager.CountLegoPieces();
            int revealedInGrid = gridManager.CountRevealedPieces();
            
            Debug.Log($"Current Level: {level}");
            Debug.Log($"Axes Remaining: {axes}");
            Debug.Log($"Pieces Revealed: {revealed} / {total}");
            Debug.Log($"Grid Total Pieces: {totalInGrid}");
            Debug.Log($"Grid Revealed Pieces: {revealedInGrid}");
            Debug.Log($"Can Dig: {gameManager.CanDig()}");
            
            // Calculate expected axes for this level
            int expectedAxes = 9 + level;
            Debug.Log($"Expected axes for level {level}: {expectedAxes}");
        }
    }
}
