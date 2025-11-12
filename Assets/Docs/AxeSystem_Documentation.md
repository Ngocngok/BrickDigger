# Axe System Documentation

## Overview
The axe system is the core resource management mechanic in Brick Digger 3D. Players have a limited number of axes (dig attempts) to reveal the hidden Lego piece. Running out of axes before revealing all pieces results in a loss.

## How It Works

### Axe Allocation
Each level gives the player a fixed number of axes based on the level number:

**Formula**: `Axes = 9 + Level Number`

| Level | Axes Given | Formula |
|-------|-----------|---------|
| 1 | 10 | 9 + 1 = 10 |
| 2 | 11 | 9 + 2 = 11 |
| 3 | 12 | 9 + 3 = 12 |
| 4 | 13 | 9 + 4 = 13 |
| 5 | 14 | 9 + 5 = 14 |
| ... | ... | ... |
| 10 | 19 | 9 + 10 = 19 |

### Axe Usage
- Each dig action consumes **1 axe**
- Axes are deducted **after** the dig animation completes
- Cannot dig if axes = 0
- UI shows remaining axes count

### Digging Flow
```
1. Player presses Dig button
2. Check: CanDig() → axes > 0?
3. If yes: Play dig animation (0.3s)
4. After animation: UseAxe() → axes -= 1
5. Check what was underneath:
   - Coin? → Collect coin
   - Lego piece? → Reveal piece
6. Update UI
7. Check win/lose conditions
```

## Win/Lose Conditions

### Win Condition
**Trigger**: All Lego piece cells are revealed

**Flow:**
```
1. Player digs block
2. Check: Is there a Lego piece underneath?
3. If yes: revealedPieces++
4. Check: revealedPieces >= totalPieces?
5. If yes: WIN!
```

**Win Sequence:**
1. Freeze player input
2. Wait 0.5 seconds
3. Award bonus coins (+5)
4. Show win panel with stats
5. Unlock next level
6. Save progress

### Lose Condition
**Trigger**: Axes reach 0 before all pieces are revealed

**Flow:**
```
1. Player uses last axe
2. Check: axes <= 0 AND revealedPieces < totalPieces?
3. If yes: LOSE!
```

**Lose Sequence:**
1. Freeze player input
2. Wait 0.5 seconds
3. Show lose panel
4. Offer retry or return to home

## Code Implementation

### LevelConfig.cs
```csharp
// Generate axes for level
config.axesStart = 9 + levelNumber;
```

### GameManager.cs

#### UseAxe()
```csharp
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
```

#### RevealPiece()
```csharp
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
```

#### CanDig()
```csharp
public bool CanDig()
{
    return axesRemaining > 0 && isGameActive;
}
```

### PlayerController.cs
```csharp
private IEnumerator DigBlock()
{
    if (!gameManager.CanDig())
    {
        Debug.Log("No axes left!");
        yield break;
    }
    
    // Play dig animation
    animator.SetTrigger("Dig");
    yield return new WaitForSeconds(digDuration);
    
    // Actually dig the block
    bool foundCoin, foundPiece;
    if (gridManager.DigBlock(currentCell, out foundCoin, out foundPiece))
    {
        gameManager.UseAxe(); // Deduct axe
        
        if (foundCoin)
            gameManager.CollectCoin();
        
        if (foundPiece)
            gameManager.RevealPiece(); // Check win condition
    }
}
```

## Balancing

### Current Formula Analysis
```
Level 1: 10 axes for ~4 piece cells = 2.5x buffer
Level 2: 11 axes for ~4 piece cells = 2.75x buffer
Level 5: 14 axes for ~4 piece cells = 3.5x buffer
```

### Difficulty Curve
- **Early Levels (1-3)**: Generous axe count, easy to win
- **Mid Levels (4-7)**: Moderate challenge, requires some strategy
- **Late Levels (8+)**: Tighter margins, strategic digging required

### Adjusting Difficulty

**Make Easier:**
```csharp
config.axesStart = 10 + levelNumber; // Level 1 = 11 axes
config.axesStart = 15 + levelNumber; // Level 1 = 16 axes
```

**Make Harder:**
```csharp
config.axesStart = 8 + levelNumber;  // Level 1 = 9 axes
config.axesStart = 7 + levelNumber;  // Level 1 = 8 axes
```

**Scale with Map Size:**
```csharp
int totalCells = config.width * config.height;
config.axesStart = totalCells / 5; // 20% of total cells
```

## Coin System Integration

### Buying Axes
Players can purchase additional axes using coins:

```csharp
public void BuyAxes()
{
    int axeCost = 5;      // Cost in coins
    int axePack = 3;      // Axes received
    
    if (totalCoins >= axeCost)
    {
        totalCoins -= axeCost;
        axesRemaining += axePack;
        UpdateUI();
    }
}
```

**Current Pricing:**
- Cost: 5 coins
- Reward: +3 axes
- Ratio: 1.67 coins per axe

### Coin Sources
- Found in dirt blocks (random placement)
- Bonus coins on level win (+5)
- Carried over between levels

## UI Integration

### HUD Display
The UI shows:
- Axes remaining (with + button to buy more)
- Coins collected (level + total)
- Piece progress (revealed / total)

### UI Updates
```csharp
private void UpdateUI()
{
    if (uiManager != null)
    {
        uiManager.UpdateAxesDisplay(axesRemaining);
        uiManager.UpdateCoinsDisplay(levelCoins, totalCoins);
        uiManager.UpdatePieceProgress(revealedPieces, totalPieces);
    }
}
```

## Events

### UnityEvents
```csharp
public UnityEvent OnLevelStart;
public UnityEvent OnLevelWin;
public UnityEvent OnLevelLose;
public UnityEvent<int> OnAxesChanged;
public UnityEvent<int> OnCoinsChanged;
public UnityEvent<int, int> OnPieceRevealed; // current, total
```

### Usage Example
```csharp
// Subscribe to events
gameManager.OnAxesChanged.AddListener(OnAxesUpdated);
gameManager.OnLevelWin.AddListener(OnWin);
gameManager.OnLevelLose.AddListener(OnLose);

// Event handlers
void OnAxesUpdated(int remaining)
{
    Debug.Log($"Axes: {remaining}");
    if (remaining <= 3)
        ShowLowAxesWarning();
}
```

## Testing

### Menu Commands
Use Unity menu: `Tools > ...`

**Test Win Condition:**
- Menu: `Tools > Test Win Condition`
- Reveals all pieces instantly
- Should trigger win sequence

**Test Lose Condition:**
- Menu: `Tools > Test Lose Condition`
- Uses all axes instantly
- Should trigger lose sequence

**Show Level Info:**
- Menu: `Tools > Show Level Info`
- Displays current level stats
- Shows axes, pieces, coins

### Manual Testing

**Test Win:**
1. Enter Play mode
2. Dig strategically to reveal all Lego pieces
3. When last piece revealed → Win panel appears
4. Verify bonus coins awarded
5. Verify next level unlocked

**Test Lose:**
1. Enter Play mode
2. Dig randomly until axes run out
3. If pieces not all revealed → Lose panel appears
4. Verify retry option available

**Test Axe Purchase:**
1. Enter Play mode
2. Collect coins
3. Click + button next to axes
4. Verify 5 coins deducted, 3 axes added

## Edge Cases

### Case 1: Exactly Enough Axes
**Scenario**: Player has exactly enough axes to reveal all pieces
**Expected**: Win on last dig
**Handled**: ✅ Win condition checked before lose condition

### Case 2: Last Axe Reveals Last Piece
**Scenario**: Last axe used reveals the final piece
**Expected**: Win (not lose)
**Handled**: ✅ RevealPiece() checks win before UseAxe() checks lose

### Case 3: Zero Axes at Start
**Scenario**: Bug causes level to start with 0 axes
**Expected**: Cannot dig, immediate lose
**Handled**: ✅ CanDig() returns false, lose triggers

### Case 4: Buy Axes After Running Out
**Scenario**: Player runs out of axes but has coins
**Expected**: Can buy axes and continue
**Handled**: ⚠️ Currently lose triggers immediately
**Future**: Add "buy axes to continue" prompt before lose

## Performance Considerations

- Axe count checked every dig (O(1) operation)
- Piece counting done once at level start (O(n²) where n = grid size)
- Win/lose checks are O(1) comparisons
- No performance concerns

## Save System Integration

### What's Saved
```csharp
PlayerPrefs.SetInt("CurrentLevel", currentLevel);
PlayerPrefs.SetInt("TotalCoins", totalCoins);
PlayerPrefs.SetInt("HighestLevel", highestLevel);
```

### What's NOT Saved
- Axes remaining (reset each level)
- Level coins (reset each level)
- Revealed pieces (reset each level)

### Why?
Each level is a fresh start. Only progression (level, total coins) persists.

## Future Enhancements

### Planned Features
1. **Dynamic Axe Pricing**: Cost increases with level
2. **Axe Efficiency Bonus**: Bonus axes for completing with few digs
3. **Axe Refund**: Refund unused axes as coins
4. **Axe Powerups**: Double dig, reveal adjacent cells, etc.
5. **Axe Challenges**: Complete level with limited axes for bonus

### Integration Points
- **Achievement System**: "Complete level with 5+ axes remaining"
- **Shop System**: Buy permanent axe bonuses
- **Daily Challenges**: Special axe constraints
- **Leaderboards**: Fewest axes used

## Troubleshooting

### Axes Not Deducting
**Check:**
- GameManager.UseAxe() is being called
- CanDig() returns true before digging
- UI is updating (OnAxesChanged event)

### Win Not Triggering
**Check:**
- All Lego pieces are actually revealed
- revealedPieces == totalPieces
- GridManager.CountLegoPieces() matches expected

### Lose Not Triggering
**Check:**
- Axes actually reached 0
- revealedPieces < totalPieces
- isGameActive is true

### Wrong Axe Count at Start
**Check:**
- LevelConfig.axesStart formula
- Current level number
- GameManager.StartLevel() is called

## Summary

The axe system provides:
- ✅ Simple, predictable progression (10, 11, 12...)
- ✅ Clear win condition (reveal all pieces)
- ✅ Clear lose condition (run out of axes)
- ✅ Strategic gameplay (where to dig?)
- ✅ Resource management (buy more axes?)
- ✅ Risk/reward decisions

**Current Implementation:**
- Level 1: 10 axes
- Level 2: 11 axes
- Level 3: 12 axes
- Formula: 9 + level number
- Win: Reveal all Lego pieces
- Lose: Run out of axes before revealing all pieces

---

**Version**: 1.0
**Status**: ✅ Implemented and Working
**Last Updated**: 2025-11-12
