# Axe System - Quick Reference

## 🎯 Core Mechanics

### Axe Formula
```
Axes = 9 + Level Number

Level 1 → 10 axes
Level 2 → 11 axes
Level 3 → 12 axes
...
```

### Win/Lose
- **WIN**: Reveal all Lego pieces ✅
- **LOSE**: Run out of axes before revealing all pieces ❌

---

## 🎮 Gameplay Flow

```
Start Level
  ↓
Get Axes (9 + level)
  ↓
Dig Blocks (-1 axe per dig)
  ↓
Reveal Lego Pieces
  ↓
All Revealed? → WIN! 🎉
Axes = 0? → LOSE! 😢
```

---

## 🔧 Key Methods

### GameManager

```csharp
// Check if can dig
bool CanDig() // Returns: axes > 0 && gameActive

// Use one axe
void UseAxe() // axes--, check lose condition

// Reveal a piece
void RevealPiece() // revealed++, check win condition

// Buy more axes
void BuyAxes() // Cost: 5 coins, Get: +3 axes
```

### GridManager

```csharp
// Dig a block
bool DigBlock(coord, out coin, out piece)

// Count total pieces
int CountLegoPieces()

// Count revealed pieces
int CountRevealedPieces()
```

---

## 🧪 Testing Commands

**Menu: Tools > ...**

- `Test Win Condition` - Instantly reveal all pieces
- `Test Lose Condition` - Instantly use all axes
- `Show Level Info` - Display current stats

---

## 📊 Level Progression

| Level | Axes | Grid Size | Difficulty |
|-------|------|-----------|------------|
| 1 | 10 | 7x15 | Easy |
| 2 | 11 | 7x15 | Easy |
| 3 | 12 | 7x15 | Easy |
| 4 | 13 | 8x15 | Medium |
| 5 | 14 | 8x15 | Medium |
| 10 | 19 | 10x15 | Hard |

---

## 💰 Coin System

### Buying Axes
- **Cost**: 5 coins
- **Reward**: +3 axes
- **Ratio**: ~1.67 coins per axe

### Coin Sources
- Found in dirt blocks (random)
- Win bonus: +5 coins
- Persistent across levels

---

## ⚙️ Configuration

### Adjust Axe Formula
**File**: `Assets/Scripts/Core/LevelConfig.cs`

```csharp
// Current (10, 11, 12...)
config.axesStart = 9 + levelNumber;

// More generous (15, 16, 17...)
config.axesStart = 14 + levelNumber;

// Harder (8, 9, 10...)
config.axesStart = 7 + levelNumber;
```

### Adjust Axe Purchase
**File**: `Assets/Scripts/Core/GameManager.cs`

```csharp
// Current: 5 coins → 3 axes
int axeCost = 5;
int axePack = 3;

// Cheaper: 3 coins → 3 axes
int axeCost = 3;
int axePack = 3;

// More axes: 5 coins → 5 axes
int axeCost = 5;
int axePack = 5;
```

---

## 🐛 Common Issues

**Axes not deducting?**
→ Check CanDig() returns true

**Win not triggering?**
→ Verify all pieces revealed

**Lose not triggering?**
→ Check axes = 0 and pieces < total

**Wrong axe count?**
→ Verify level number and formula

---

## 📁 Key Files

```
Assets/Scripts/Core/
├── GameManager.cs (win/lose logic)
├── LevelConfig.cs (axe formula)
└── GridManager.cs (dig mechanics)

Assets/Docs/
├── AxeSystem_Documentation.md (full docs)
└── AxeSystem_QuickReference.md (this file)
```

---

## 💡 Quick Tips

- Start with 10 axes on level 1
- Each dig costs 1 axe
- Reveal all pieces to win
- Run out of axes = lose
- Buy axes with coins (5 coins → 3 axes)
- Strategic digging is key!

---

For detailed documentation, see:
`Assets/Docs/AxeSystem_Documentation.md`
