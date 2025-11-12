# Character Shop System Documentation

## Overview
The character shop allows players to preview, purchase, and equip different character models using coins earned from gameplay. The shop is integrated into the Home scene with a 3D character preview as the background.

## Features

### ✅ Character Preview
- 3D character model displayed in idle animation
- Full body view with proper lighting
- Character serves as background for Home UI
- Smooth character switching with Next/Prev buttons

### ✅ Character Management
- **Total Characters**: 8 (Character_1 to Character_8)
- **Free Character**: Character_1 (unlocked by default)
- **Purchasable**: Characters 2-8 (20 coins each)
- **Persistence**: Unlocked characters and selection saved in PlayerPrefs

### ✅ Purchase System
- **Price**: 20 coins per character
- **Visual Feedback**: 
  - Green "BUY (20)" button when affordable
  - Grey "BUY (20)" button when not affordable (disabled)
- **Coin Deduction**: Automatic when purchasing

### ✅ Equip System
- **Equip Button**: Shows for unlocked but not selected characters
- **Selected Button**: Shows for currently equipped character (disabled)
- **Persistence**: Selected character spawns in gameplay

## UI Layout

### Home Scene Structure
```
Background: 3D Character Preview (CharacterPreviewCamera)
  ↓
Foreground: UI Elements (Main Camera)
  ├── Title: "BRICK DIGGER 3D"
  ├── Level: "Level 1"
  ├── Coins: "Coins: 0" (top-left, gold color)
  ├── Character Name: "Character 1" (top-center)
  ├── Settings Button (top-right, gear icon)
  ├── Prev Button (left side, "<")
  ├── Next Button (right side, ">")
  ├── Action Button (bottom-center, "BUY/EQUIP/SELECTED")
  └── Play Button (bottom-center, "PLAY")
```

### Camera Setup
- **CharacterPreviewCamera**: 
  - Depth: -1 (renders first, background)
  - Position: (0, 1.2, -2.5)
  - Rotation: (5, 0, 0)
  - Clear Flags: Solid Color
  - Background: Dark grey
  
- **Main Camera (UI)**:
  - Depth: 1 (renders on top)
  - Clear Flags: Depth only
  - Renders UI canvas

### Transparent UI
- Canvas background: Fully transparent (0,0,0,0)
- UI elements float on top of character preview
- Settings popup: Semi-transparent dark background

## Code Implementation

### CharacterShopManager.cs

#### Key Properties
```csharp
[SerializeField] private int totalCharactersForSale = 8;
[SerializeField] private int characterPrice = 20;
[SerializeField] private Transform characterSpawnPoint;
[SerializeField] private RuntimeAnimatorController idleAnimatorController;
```

#### Core Methods

**ShowCharacter(int index)**
```csharp
// Spawns character at preview point
// Assigns idle animator
// Destroys previous preview
```

**OnNextCharacter()**
```csharp
// Cycles to next character (1→2→3...→8→1)
// Updates preview and UI
```

**OnPrevCharacter()**
```csharp
// Cycles to previous character (1←2←3...←8←1)
// Updates preview and UI
```

**OnActionButtonClicked()**
```csharp
// If locked: Try to buy (if enough coins)
// If unlocked but not selected: Equip character
// If selected: Do nothing (button disabled)
```

**UpdateActionButton()**
```csharp
// Updates button text and color based on state:
// - "BUY (20)" - Green if affordable, Grey if not
// - "EQUIP" - Blue
// - "SELECTED" - Dark grey (disabled)
```

#### PlayerPrefs Keys
```csharp
"SelectedCharacter"    // Currently equipped character (1-8)
"UnlockedCharacters"   // Comma-separated list (e.g., "1,2,5")
"TotalCoins"          // Player's total coins
```

### Integration with GameManager

#### Coin Synchronization
```csharp
// In HomeScene.cs
CharacterShopManager shop = FindFirstObjectByType<CharacterShopManager>();
shop.SyncCoins(totalCoins);
```

#### After Gameplay
When returning from gameplay to home:
1. GameManager saves total coins to PlayerPrefs
2. HomeScene loads coins from PlayerPrefs
3. HomeScene syncs coins with CharacterShopManager
4. Shop updates UI and button states

### Integration with CharacterSpawner

#### In Gameplay Scene
```csharp
// CharacterSpawner.cs already uses "SelectedCharacter" key
private const string CHARACTER_SELECTION_KEY = "SelectedCharacter";

// On Start()
currentCharacterIndex = PlayerPrefs.GetInt(CHARACTER_SELECTION_KEY, 1);
SpawnCharacter(currentCharacterIndex);
```

**Flow:**
1. Player selects character in shop → Saves to PlayerPrefs
2. Player starts game → CharacterSpawner loads from PlayerPrefs
3. Selected character spawns in gameplay

## User Flow

### First Time Player
```
1. Open game → Character_1 shown (free, selected)
2. See "SELECTED" button (disabled)
3. Click Next → See Character_2
4. See "BUY (20)" button (grey, not enough coins)
5. Play game → Earn coins
6. Return to home → Coins updated
7. Navigate to Character_2 → "BUY (20)" now green
8. Click Buy → Character unlocked, coins deducted
9. See "EQUIP" button (blue)
10. Click Equip → Character_2 now selected
11. Play game → Character_2 spawns
```

### Returning Player
```
1. Open game → Previously selected character shown
2. See "SELECTED" button
3. Navigate with Next/Prev to browse
4. Locked characters show "BUY (20)"
5. Unlocked characters show "EQUIP"
6. Selected character shows "SELECTED"
```

## Button States

### Action Button States

| State | Text | Color | Interactable | Condition |
|-------|------|-------|--------------|-----------|
| Locked (Affordable) | BUY (20) | Green | Yes | Not unlocked, coins >= 20 |
| Locked (Not Affordable) | BUY (20) | Grey | No | Not unlocked, coins < 20 |
| Unlocked (Not Selected) | EQUIP | Blue | Yes | Unlocked, not selected |
| Selected | SELECTED | Dark Grey | No | Currently equipped |

### Navigation Buttons
- **Prev (<)**: Always enabled, cycles backward
- **Next (>)**: Always enabled, cycles forward
- **Wrapping**: Character_1 ← Character_8, Character_8 → Character_1

## Data Persistence

### Save Format
```
PlayerPrefs:
  SelectedCharacter: 1 (int)
  UnlockedCharacters: "1,2,5,7" (string, comma-separated)
  TotalCoins: 45 (int)
```

### Load/Save Flow
```csharp
// On Start
LoadPlayerData()
  ↓
Load from PlayerPrefs
  ↓
Populate unlockedCharacters HashSet
  ↓
Show selected character
  ↓
Update UI

// On Purchase/Equip
SavePlayerData()
  ↓
Save to PlayerPrefs
  ↓
Update UI
```

## Character Library

### Available Characters
- **Character_1**: Free (always unlocked)
- **Character_2**: 20 coins
- **Character_3**: 20 coins
- **Character_4**: 20 coins
- **Character_5**: 20 coins
- **Character_6**: 20 coins
- **Character_7**: 20 coins
- **Character_8**: 20 coins

### Expansion
To add more characters:
```csharp
// In CharacterShopManager Inspector
totalCharactersForSale = 12; // Now Character_1 to Character_12
```

## Visual Design

### Color Scheme
- **Coins Text**: Gold (1, 0.84, 0, 1)
- **Character Name**: White (1, 1, 1, 1)
- **Buy Button (Affordable)**: Green (0.2, 0.8, 0.2, 1)
- **Buy Button (Not Affordable)**: Grey (0.5, 0.5, 0.5, 1)
- **Equip Button**: Blue (0.2, 0.6, 1, 1)
- **Selected Button**: Dark Grey (0.3, 0.3, 0.3, 1)
- **Background**: Transparent (character preview visible)

### Layout Positions
- **Coins**: Top-left corner
- **Character Name**: Top-center, below title
- **Settings**: Top-right corner
- **Prev/Next**: Left and right sides, vertically centered
- **Action Button**: Bottom-center, above Play button
- **Play Button**: Bottom-center

## Testing

### Test Purchase Flow
1. Start with 0 coins
2. Navigate to Character_2
3. Verify "BUY (20)" is grey and disabled
4. Add coins via debug menu or gameplay
5. Return to shop
6. Verify "BUY (20)" is green and enabled
7. Click Buy
8. Verify coins deducted
9. Verify button changes to "EQUIP"

### Test Equip Flow
1. Navigate to unlocked character
2. Verify "EQUIP" button shows
3. Click Equip
4. Verify button changes to "SELECTED"
5. Navigate to another character
6. Navigate back
7. Verify still shows "SELECTED"

### Test Gameplay Integration
1. Select Character_2 in shop
2. Click Play
3. Enter gameplay
4. Verify Character_2 spawns (not Character_1)
5. Complete level
6. Return to home
7. Verify Character_2 still selected

### Test Persistence
1. Select and equip Character_3
2. Close game completely
3. Reopen game
4. Verify Character_3 is shown and selected
5. Verify unlocked characters still unlocked

## Menu Commands

### Tools Menu
- `Tools > Initialize Character Shop` - Setup character preview
- `Tools > Finalize Character Shop` - Configure cameras and UI

### Debug Commands
Add these to CharacterShopManager for testing:

```csharp
[ContextMenu("Add 100 Coins")]
private void Debug_AddCoins()
{
    playerCoins += 100;
    SavePlayerData();
    UpdateUI();
}

[ContextMenu("Unlock All Characters")]
private void Debug_UnlockAll()
{
    for (int i = 1; i <= totalCharactersForSale; i++)
    {
        unlockedCharacters.Add(i);
    }
    SavePlayerData();
    UpdateUI();
}

[ContextMenu("Reset Shop")]
private void Debug_Reset()
{
    unlockedCharacters.Clear();
    unlockedCharacters.Add(1);
    selectedCharacterIndex = 1;
    currentCharacterIndex = 1;
    SavePlayerData();
    ShowCharacter(1);
    UpdateUI();
}
```

## Performance Considerations

- Only one character preview active at a time
- Previous preview destroyed when switching
- Animator uses idle animation only (low CPU)
- No physics on preview character
- Efficient camera layering (depth-based)

## Future Enhancements

### Planned Features
1. **Character Stats Display**: Show character-specific abilities
2. **Character Rotation**: Swipe to rotate character preview
3. **Rarity System**: Common, Rare, Epic, Legendary characters
4. **Bundle Deals**: Buy multiple characters at discount
5. **Daily Free Character**: One free character per day
6. **Character Customization**: Change colors, accessories
7. **Preview Animations**: Show walk/jump animations in preview
8. **Character Showcase**: Special poses or emotes

### Integration Points
- **Achievement System**: Unlock characters via achievements
- **Daily Rewards**: Characters as rewards
- **Events**: Limited-time characters
- **Ads**: Watch ad to unlock character
- **IAP**: Premium characters for real money

## Troubleshooting

### Character Not Visible
**Problem**: Character preview not showing
**Solutions**:
1. Check CharacterPreviewCamera depth is -1
2. Verify character spawned at CharacterSpawnPoint
3. Check lighting (CharacterLight exists)
4. Run `Tools > Initialize Character Shop`

### Button Not Working
**Problem**: Buy/Equip button doesn't respond
**Solutions**:
1. Check button is interactable
2. Verify CharacterShopManager references are set
3. Check console for errors
4. Verify EventSystem exists in scene

### Coins Not Syncing
**Problem**: Coins don't update after gameplay
**Solutions**:
1. Check GameManager saves coins to PlayerPrefs
2. Verify HomeScene loads coins on Start
3. Check CharacterShopManager.SyncCoins() is called
4. Verify PlayerPrefs key matches: "TotalCoins"

### Character Not Spawning in Gameplay
**Problem**: Wrong character spawns in game
**Solutions**:
1. Check "SelectedCharacter" is saved in PlayerPrefs
2. Verify CharacterSpawner uses same PlayerPrefs key
3. Check character index is valid (1-8)
4. Run game from Home scene (not directly from Gameplay)

## File Structure

```
Assets/
├── Scripts/
│   ├── UI/
│   │   ├── CharacterShopManager.cs     ← NEW
│   │   └── HomeScene.cs                ← UPDATED
│   ├── Player/
│   │   └── CharacterSpawner.cs         ← Uses selected character
│   └── Setup/
│       ├── InitializeCharacterShop.cs  ← NEW
│       └── FinalizeCharacterShop.cs    ← NEW
├── Scenes/
│   └── HomeScene.unity                 ← Character shop integrated
└── Docs/
    ├── CharacterShop_Documentation.md  ← This file
    └── CharacterShop_QuickReference.md ← Quick guide
```

## API Reference

### CharacterShopManager

#### Public Methods
```csharp
int GetSelectedCharacter()
// Returns currently selected character index

bool IsCharacterUnlocked(int characterIndex)
// Check if character is unlocked

void AddCoins(int amount)
// Add coins to player's total

void SyncCoins(int totalCoins)
// Sync coins from GameManager
```

#### Inspector Fields
```csharp
totalCharactersForSale (int)           // Total characters (default: 8)
characterPrice (int)                   // Price per character (default: 20)
characterSpawnPoint (Transform)        // Where to spawn preview
idleAnimatorController (RuntimeAnimatorController) // Animator for idle
prevButton (Button)                    // Previous character button
nextButton (Button)                    // Next character button
actionButton (Button)                  // Buy/Equip/Selected button
actionButtonText (Text)                // Action button text
characterNameText (Text)               // Character name display
coinsText (Text)                       // Coins display
```

## Scene Setup

### GameObjects
```
HomeScene
├── Camera (UI Camera, depth: 1)
├── CharacterPreviewCamera (depth: -1)
├── CharacterSpawnPoint (0, 0, 0)
│   └── CharacterPreview_1 (spawned dynamically)
├── CharacterLight (directional light for preview)
├── CharacterShopManager (script holder)
├── HomeManager (existing)
└── Canvas (UI elements)
    ├── Background (transparent)
    ├── TitleText
    ├── LevelText
    ├── ShopCoinsText (gold, top-left)
    ├── CharacterNameText (white, top-center)
    ├── SettingsButton (top-right)
    ├── PrevButton (left, "<")
    ├── NextButton (right, ">")
    ├── ActionButton (bottom, "BUY/EQUIP/SELECTED")
    └── PlayButton (bottom, "PLAY")
```

## Workflow

### Player Journey
```
Home Scene
  ↓
Browse Characters (Next/Prev)
  ↓
See Character Preview (3D, Idle Animation)
  ↓
Check Price & Coins
  ↓
Buy Character (if affordable)
  ↓
Equip Character
  ↓
Play Game
  ↓
Selected Character Spawns
  ↓
Earn Coins
  ↓
Return to Home
  ↓
Buy More Characters
```

### Data Flow
```
CharacterShopManager
  ↓ (saves)
PlayerPrefs
  ↓ (loads)
CharacterSpawner (Gameplay)
  ↓ (spawns)
Selected Character in Game
```

## Configuration

### Adjust Character Count
```csharp
// In CharacterShopManager Inspector
totalCharactersForSale = 12; // Sell 12 characters instead of 8
```

### Adjust Pricing
```csharp
// In CharacterShopManager Inspector
characterPrice = 50; // Make characters cost 50 coins
```

### Adjust Camera View
```csharp
// CharacterPreviewCamera position
Position: (0, 1.2, -2.5)  // Current
Position: (0, 1.5, -3)    // Farther back, higher view
Position: (0, 1, -2)      // Closer, lower view
```

## Summary

The character shop system provides:
- ✅ **Visual Character Preview**: 3D character as background
- ✅ **Easy Navigation**: Next/Prev buttons
- ✅ **Clear Purchase System**: 20 coins per character
- ✅ **Visual Feedback**: Color-coded buttons
- ✅ **Persistence**: Unlocks and selection saved
- ✅ **Gameplay Integration**: Selected character spawns in game
- ✅ **Professional UI**: Transparent backgrounds, clean layout

**Current Status**: ✅ Fully Implemented and Functional

---

**Version**: 1.0
**Last Updated**: 2025-11-12
**Status**: Production Ready
