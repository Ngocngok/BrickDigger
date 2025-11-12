using UnityEngine;
using System.Collections.Generic;

namespace BrickDigger
{
    [System.Serializable]
    public class LevelConfig
    {
        public int levelNumber;
        public int width = 7;
        public int height = 15;
        public int coinsCount = 3;
        public int axesStart = 25;
        public List<CellCoord> pieceShape;
        public CellCoord piecePosition;
        
        // Tetromino shapes
        public static readonly Dictionary<string, List<CellCoord>> TetrominoShapes = new Dictionary<string, List<CellCoord>>
        {
            ["I"] = new List<CellCoord> { new CellCoord(0,0), new CellCoord(0,1), new CellCoord(0,2), new CellCoord(0,3) },
            ["O"] = new List<CellCoord> { new CellCoord(0,0), new CellCoord(0,1), new CellCoord(1,0), new CellCoord(1,1) },
            ["T"] = new List<CellCoord> { new CellCoord(1,0), new CellCoord(0,1), new CellCoord(1,1), new CellCoord(2,1) },
            ["L"] = new List<CellCoord> { new CellCoord(0,0), new CellCoord(0,1), new CellCoord(0,2), new CellCoord(1,2) },
            ["J"] = new List<CellCoord> { new CellCoord(1,0), new CellCoord(1,1), new CellCoord(1,2), new CellCoord(0,2) },
            ["S"] = new List<CellCoord> { new CellCoord(1,0), new CellCoord(2,0), new CellCoord(0,1), new CellCoord(1,1) },
            ["Z"] = new List<CellCoord> { new CellCoord(0,0), new CellCoord(1,0), new CellCoord(1,1), new CellCoord(2,1) }
        };
        
        public static LevelConfig GenerateLevel(int levelNumber)
        {
            LevelConfig config = new LevelConfig();
            config.levelNumber = levelNumber;
            
            // Increase difficulty with level
            int baseSize = 7;
            int sizeIncrease = (levelNumber - 1) / 3; // Increase every 3 levels
            config.width = baseSize + sizeIncrease;
            config.height = 15;
            
            // Axes and coins scale with map size
            int totalCells = config.width * config.height;
            config.axesStart = Mathf.Max(20, totalCells / 4); // About 25% of cells
            config.coinsCount = 3 + (levelNumber / 2); // More coins at higher levels
            
            // Select random tetromino
            string[] shapes = new string[] { "I", "O", "T", "L", "J", "S", "Z" };
            string selectedShape = shapes[Random.Range(0, shapes.Length)];
            config.pieceShape = new List<CellCoord>(TetrominoShapes[selectedShape]);
            
            // Random position for the piece (ensure it fits)
            int maxX = config.width - GetMaxX(config.pieceShape) - 1;
            int maxY = config.height - GetMaxY(config.pieceShape) - 1;
            config.piecePosition = new CellCoord(
                Random.Range(1, Mathf.Max(2, maxX)),
                Random.Range(1, Mathf.Max(2, maxY))
            );
            
            // Randomly rotate the piece
            int rotations = Random.Range(0, 4);
            for (int i = 0; i < rotations; i++)
            {
                config.pieceShape = RotatePiece(config.pieceShape);
            }
            
            return config;
        }
        
        private static List<CellCoord> RotatePiece(List<CellCoord> piece)
        {
            List<CellCoord> rotated = new List<CellCoord>();
            foreach (var coord in piece)
            {
                // 90 degree rotation: (x,y) -> (y,-x)
                rotated.Add(new CellCoord(coord.y, -coord.x));
            }
            
            // Normalize to positive coordinates
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            foreach (var coord in rotated)
            {
                minX = Mathf.Min(minX, coord.x);
                minY = Mathf.Min(minY, coord.y);
            }
            
            for (int i = 0; i < rotated.Count; i++)
            {
                rotated[i] = new CellCoord(rotated[i].x - minX, rotated[i].y - minY);
            }
            
            return rotated;
        }
        
        private static int GetMaxX(List<CellCoord> piece)
        {
            int max = 0;
            foreach (var coord in piece)
                max = Mathf.Max(max, coord.x);
            return max;
        }
        
        private static int GetMaxY(List<CellCoord> piece)
        {
            int max = 0;
            foreach (var coord in piece)
                max = Mathf.Max(max, coord.y);
            return max;
        }
    }
}