using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace BrickDigger
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float autoJumpHeight = 1.2f; // Height difference to trigger auto-jump
        
        [Header("Digging Settings")]
        [SerializeField] private float digDuration = 0.3f;
        [SerializeField] private float digCooldown = 0.5f;
        
        [Header("References")]
        [SerializeField] private GridManager gridManager;
        [SerializeField] private GameManager gameManager;
        
        private CharacterController characterController;
        private Animator animator;
        
        private Vector2 moveInput;
        private Vector3 velocity;
        private bool isGrounded;
        private bool isDigging;
        private bool canDig = true;
        private bool isOnBedrock = false;
        
        private CellCoord currentCell;
        private float targetHeight;
        
        // Mobile input actions
        private bool jumpRequested;
        private bool digRequested;
        
        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            if (characterController == null)
            {
                characterController = gameObject.AddComponent<CharacterController>();
                characterController.radius = 0.3f;
                characterController.height = 1f;
                characterController.center = new Vector3(0, 0.5f, 0);
            }
            
            animator = GetComponent<Animator>();
        }
        
        private void Start()
        {
            if (gridManager == null)
                gridManager = FindObjectOfType<GridManager>();
            if (gameManager == null)
                gameManager = FindObjectOfType<GameManager>();
                
            // Start at a valid position
            PlaceOnGrid(new CellCoord(gridManager.Width / 2, gridManager.Height / 2));
        }
        
        private void Update()
        {
            UpdateGroundCheck();
            UpdateCurrentCell();
            HandleMovement();
            HandleDigging();
            
            // Highlight block under player
            if (gridManager != null && !isDigging)
            {
                gridManager.HighlightBlock(currentCell);
            }
        }
        
        private void UpdateGroundCheck()
        {
            // Simple ground check
            isGrounded = characterController.isGrounded;
            
            // Check if we're on bedrock (no blocks above us)
            if (gridManager != null)
            {
                BlockType topBlock = gridManager.GetTopBlockAt(currentCell);
                isOnBedrock = (topBlock == BlockType.Air);
            }
        }
        
        private void UpdateCurrentCell()
        {
            if (gridManager != null)
            {
                currentCell = gridManager.WorldToGrid(transform.position);
            }
        }
        
        private void HandleMovement()
        {
            if (isDigging)
                return;
                
            Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
            
            if (move.magnitude > 0.1f)
            {
                // Check for auto-jump ONLY when on bedrock (2nd layer)
                if (isOnBedrock && isGrounded)
                {
                    Vector3 moveDir = move.normalized;
                    Vector3 nextPos = transform.position + moveDir * 0.6f; // Check slightly ahead
                    CellCoord nextCell = gridManager.WorldToGrid(nextPos);
                    
                    if (gridManager.IsValidCoord(nextCell))
                    {
                        BlockType topBlockAhead = gridManager.GetTopBlockAt(nextCell);
                        
                        // Auto-jump if there's a dirt/coin block blocking the way
                        if (topBlockAhead == BlockType.Dirt || topBlockAhead == BlockType.CoinBlock)
                        {
                            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                        }
                    }
                }
                
                // Apply horizontal movement
                characterController.Move(move * moveSpeed * Time.deltaTime);
                
                // Face movement direction
                if (move != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, 
                        Quaternion.LookRotation(move), Time.deltaTime * 10f);
                }
            }
            
            // Handle manual jump (works at all times when grounded)
            if (jumpRequested && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                jumpRequested = false;
            }
            
            // Apply gravity
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            else
            {
                velocity.y += gravity * Time.deltaTime;
            }
            
            characterController.Move(velocity * Time.deltaTime);
        }
        
        private void HandleDigging()
        {
            if (digRequested && canDig && !isDigging && isGrounded)
            {
                StartCoroutine(DigBlock());
                digRequested = false;
            }
        }
        
        private IEnumerator DigBlock()
        {
            if (!gameManager.CanDig())
            {
                // Show feedback that no axes left
                Debug.Log("No axes left!");
                yield break;
            }
            
            isDigging = true;
            canDig = false;
            
            // Play dig animation
            if (animator != null)
            {
                animator.SetTrigger("Dig");
            }
            
            yield return new WaitForSeconds(digDuration);
            
            // Actually dig the block
            bool foundCoin, foundPiece;
            if (gridManager.DigBlock(currentCell, out foundCoin, out foundPiece))
            {
                gameManager.UseAxe();
                
                if (foundCoin)
                {
                    gameManager.CollectCoin();
                }
                
                if (foundPiece)
                {
                    gameManager.RevealPiece();
                }
                
                // Fall if we dug the block we're standing on
                float newHeight = gridManager.GetStandingHeight(currentCell);
                if (newHeight < transform.position.y - 0.5f)
                {
                    // We'll fall naturally with gravity
                }
            }
            
            isDigging = false;
            
            yield return new WaitForSeconds(digCooldown);
            canDig = true;
        }
        
        public void PlaceOnGrid(CellCoord coord)
        {
            if (gridManager != null && gridManager.IsValidCoord(coord))
            {
                float height = gridManager.GetStandingHeight(coord);
                transform.position = new Vector3(coord.x, height, coord.y);
                currentCell = coord;
            }
        }
        
        // Input System callbacks
        public void OnMove(Vector2 input)
        {
            moveInput = input;
        }
        
        public void OnJump()
        {
            jumpRequested = true;
        }
        
        public void OnDig()
        {
            digRequested = true;
        }
        
        // For mobile joystick
        public void SetMoveInput(Vector2 input)
        {
            moveInput = input;
        }
        
        public void SetJumpInput(bool jump)
        {
            jumpRequested = jump;
        }
        
        public void SetDigInput(bool dig)
        {
            digRequested = dig;
        }
    }
}