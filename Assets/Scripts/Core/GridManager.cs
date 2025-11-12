using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace BrickDigger
{
    // Cell coordinate in the grid
    [System.Serializable]
    public struct CellCoord
    {
        public int x;
        public int y;
        
        public CellCoord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        public static CellCoord operator +(CellCoord a, CellCoord b)
        {
            return new CellCoord(a.x + b.x, a.y + b.y);
        }
        
        public Vector3 ToWorldPosition(float height = 0)
        {
            return new Vector3(x, height, y);
        }
        
        public override string ToString() => $"({x}, {y})";
    }
    
    // Different block types
    public enum BlockType
    {
        Air,
        Dirt,
        Bedrock,
        CoinBlock,
        LegoPiece
    }
    
    // Grid cell data
    [System.Serializable]
    public class GridCell
    {
        public BlockType topLayer;    // Layer 1 (dirt, coin blocks, or air)
        public BlockType bottomLayer; // Layer 0 (bedrock or lego piece)
        public int pieceId = -1;      // ID of the lego piece if this is a piece cell
        
        public GridCell()
        {
            topLayer = BlockType.Dirt;
            bottomLayer = BlockType.Bedrock;
        }
    }
    
    // Main grid manager
    public class GridManager : MonoBehaviour
    {
        [Header("Grid Settings")]
        [SerializeField] private int width = 7;
        [SerializeField] private int height = 15;
        
        [Header("Block Prefabs")]
        [SerializeField] private GameObject dirtBlockPrefab;
        [SerializeField] private GameObject bedrockBlockPrefab;
        [SerializeField] private GameObject coinBlockPrefab;
        [SerializeField] private GameObject legoPiecePrefab;
        
        [Header("Visual Settings")]
        [SerializeField] private float blockSize = 1f;
        [SerializeField] private float layerHeight = 1f;
        
        private GridCell[,] grid;
        private Dictionary<CellCoord, GameObject> topLayerBlocks = new Dictionary<CellCoord, GameObject>();
        private Dictionary<CellCoord, GameObject> bottomLayerBlocks = new Dictionary<CellCoord, GameObject>();
        
        // Highlighted block for digging
        private GameObject highlightedBlock;
        private CellCoord highlightedCoord;
        
        public int Width => width;
        public int Height => height;
        
        private void Awake()
        {
            InitializeGrid();
        }
        
        private void InitializeGrid()
        {
            grid = new GridCell[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = new GridCell();
                }
            }
        }
        
        public void GenerateLevel(LevelConfig config)
        {
            width = config.width;
            height = config.height;
            InitializeGrid();
            
            // Clear existing blocks
            ClearAllBlocks();
            
            // Generate bedrock layer
            GenerateBedrockLayer();
            
            // Place lego piece
            PlaceLegoPiece(config.pieceShape, config.piecePosition);
            
            // Generate dirt layer with coins
            GenerateDirtLayer(config.coinsCount);
            
            // Spawn blocks in world
            SpawnAllBlocks();
        }
        
        private void GenerateBedrockLayer()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y].bottomLayer = BlockType.Bedrock;
                }
            }
        }
        
        private void PlaceLegoPiece(List<CellCoord> shape, CellCoord position)
        {
            foreach (var coord in shape)
            {
                CellCoord worldCoord = position + coord;
                if (IsValidCoord(worldCoord))
                {
                    grid[worldCoord.x, worldCoord.y].bottomLayer = BlockType.LegoPiece;
                    grid[worldCoord.x, worldCoord.y].pieceId = 0;
                }
            }
        }
        
        private void GenerateDirtLayer(int coinsCount)
        {
            // Fill with dirt
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y].topLayer = BlockType.Dirt;
                }
            }
            
            // Place random coins
            List<CellCoord> availableCoords = new List<CellCoord>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    availableCoords.Add(new CellCoord(x, y));
                }
            }
            
            for (int i = 0; i < coinsCount && availableCoords.Count > 0; i++)
            {
                int randomIndex = Random.Range(0, availableCoords.Count);
                CellCoord coord = availableCoords[randomIndex];
                grid[coord.x, coord.y].topLayer = BlockType.CoinBlock;
                availableCoords.RemoveAt(randomIndex);
            }
        }
        
        private void SpawnAllBlocks()
        {
            // Spawn bottom layer
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    CellCoord coord = new CellCoord(x, y);
                    SpawnBottomBlock(coord, grid[x, y].bottomLayer);
                }
            }
            
            // Spawn top layer
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    CellCoord coord = new CellCoord(x, y);
                    if (grid[x, y].topLayer != BlockType.Air)
                    {
                        SpawnTopBlock(coord, grid[x, y].topLayer);
                    }
                }
            }
        }
        
        private void SpawnTopBlock(CellCoord coord, BlockType type)
        {
            GameObject prefab = GetBlockPrefab(type);
            if (prefab != null)
            {
                Vector3 position = coord.ToWorldPosition(layerHeight);
                GameObject block = Instantiate(prefab, position, Quaternion.identity, transform);
                
                // Set layer for dirt blocks
                int dirtLayer = LayerMask.NameToLayer("Dirt");
                if (dirtLayer >= 0)
                {
                    block.layer = dirtLayer;
                }
                
                topLayerBlocks[coord] = block;
            }
        }
        
        private void SpawnBottomBlock(CellCoord coord, BlockType type)
        {
            GameObject prefab = GetBlockPrefab(type);
            if (prefab != null)
            {
                Vector3 position = coord.ToWorldPosition(0);
                GameObject block = Instantiate(prefab, position, Quaternion.identity, transform);
                
                // Set layer for bedrock blocks
                if (type == BlockType.Bedrock)
                {
                    int bedrockLayer = LayerMask.NameToLayer("Bedrock");
                    if (bedrockLayer >= 0)
                    {
                        block.layer = bedrockLayer;
                    }
                }
                
                bottomLayerBlocks[coord] = block;
            }
        }
        
        private GameObject GetBlockPrefab(BlockType type)
        {
            switch (type)
            {
                case BlockType.Dirt:
                    return dirtBlockPrefab;
                case BlockType.Bedrock:
                    return bedrockBlockPrefab;
                case BlockType.CoinBlock:
                    return coinBlockPrefab;
                case BlockType.LegoPiece:
                    return legoPiecePrefab;
                default:
                    return null;
            }
        }
        
        public bool DigBlock(CellCoord coord, out bool foundCoin, out bool foundPiece)
        {
            foundCoin = false;
            foundPiece = false;
            
            if (!IsValidCoord(coord))
                return false;
                
            GridCell cell = grid[coord.x, coord.y];
            
            if (cell.topLayer == BlockType.Air)
                return false;
                
            // Check what we're digging
            if (cell.topLayer == BlockType.CoinBlock)
            {
                foundCoin = true;
            }
            
            // Check if piece is underneath
            if (cell.bottomLayer == BlockType.LegoPiece)
            {
                foundPiece = true;
            }
            
            // Remove the block
            cell.topLayer = BlockType.Air;
            
            // Destroy the visual block
            if (topLayerBlocks.ContainsKey(coord))
            {
                // Play break animation here
                StartCoroutine(BreakBlockAnimation(topLayerBlocks[coord]));
                topLayerBlocks.Remove(coord);
            }
            
            return true;
        }
        
        private System.Collections.IEnumerator BreakBlockAnimation(GameObject block)
        {
            // Simple break animation - scale down and destroy
            float duration = 0.3f;
            float elapsed = 0;
            Vector3 originalScale = block.transform.localScale;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                block.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
                yield return null;
            }
            
            Destroy(block);
        }
        
        public void HighlightBlock(CellCoord coord)
        {
            if (highlightedCoord.x == coord.x && highlightedCoord.y == coord.y)
                return;
                
            ClearHighlight();
            
            if (topLayerBlocks.ContainsKey(coord))
            {
                highlightedBlock = topLayerBlocks[coord];
                highlightedCoord = coord;
                
                // Add highlight effect
                Renderer renderer = highlightedBlock.GetComponent<Renderer>();
                if (renderer != null)
                {
                    // Store original color and make it brighter
                    renderer.material.EnableKeyword("_EMISSION");
                    renderer.material.SetColor("_EmissionColor", Color.yellow * 0.3f);
                }
            }
        }
        
        public void ClearHighlight()
        {
            if (highlightedBlock != null)
            {
                Renderer renderer = highlightedBlock.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.DisableKeyword("_EMISSION");
                }
                highlightedBlock = null;
            }
        }
        
        public BlockType GetTopBlockAt(CellCoord coord)
        {
            if (!IsValidCoord(coord))
                return BlockType.Air;
            return grid[coord.x, coord.y].topLayer;
        }
        
        public BlockType GetBottomBlockAt(CellCoord coord)
        {
            if (!IsValidCoord(coord))
                return BlockType.Bedrock;
            return grid[coord.x, coord.y].bottomLayer;
        }
        
        public bool IsValidCoord(CellCoord coord)
        {
            return coord.x >= 0 && coord.x < width && coord.y >= 0 && coord.y < height;
        }
        
        public CellCoord WorldToGrid(Vector3 worldPos)
        {
            return new CellCoord(
                Mathf.RoundToInt(worldPos.x),
                Mathf.RoundToInt(worldPos.z)
            );
        }
        
        public int CountLegoPieces()
        {
            int count = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (grid[x, y].bottomLayer == BlockType.LegoPiece)
                        count++;
                }
            }
            return count;
        }
        
        public int CountRevealedPieces()
        {
            int count = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (grid[x, y].bottomLayer == BlockType.LegoPiece && 
                        grid[x, y].topLayer == BlockType.Air)
                        count++;
                }
            }
            return count;
        }
        
        private void ClearAllBlocks()
        {
            foreach (var block in topLayerBlocks.Values)
            {
                if (block != null) Destroy(block);
            }
            topLayerBlocks.Clear();
            
            foreach (var block in bottomLayerBlocks.Values)
            {
                if (block != null) Destroy(block);
            }
            bottomLayerBlocks.Clear();
        }
        
        // Check if there's a solid block to stand on
        public bool HasSolidGround(CellCoord coord)
        {
            if (!IsValidCoord(coord))
                return false;
                
            // Can stand on dirt/coin blocks or bedrock if top is air
            BlockType top = grid[coord.x, coord.y].topLayer;
            if (top == BlockType.Dirt || top == BlockType.CoinBlock)
                return true;
                
            // If top is air, can stand on bedrock
            if (top == BlockType.Air)
                return true;
                
            return false;
        }
        
        // Get the height player should be at for a given coordinate
        public float GetStandingHeight(CellCoord coord)
        {
            if (!IsValidCoord(coord))
                return 0;
                
            BlockType top = grid[coord.x, coord.y].topLayer;
            if (top == BlockType.Dirt || top == BlockType.CoinBlock)
                return layerHeight + 0.5f; // Stand on top of dirt
            else
                return 0.5f; // Stand on bedrock
        }
    }
}