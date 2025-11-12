# Brick Digger 3D – Game Design Spec (v1.1)

## 1) Overview
**Type:** 3D Grid Puzzle / Digging Action (Single Player)  
**Core Objective:** The player moves a human character on a blocky 3D map and digs dirt blocks underneath to uncover a hidden Lego-shaped object (Tetromino). The player must reveal all parts of the hidden piece before running out of axes (dig attempts).

**Gameplay Summary:**
1. Move across the dirt surface.  
2. Dig the block directly under the player.  
3. Reveal what’s underneath (bedrock, Lego piece, or coin).  
4. Collect coins and continue until the Lego piece is fully uncovered.  
5. Auto-collect the Lego piece when all its parts are revealed → Win.  
6. If axes run out → Lose.

---

## 2) World & Grid System

### Grid Structure
- Map is a **2D grid** in X-Y plane (cell size = 1 unit).  
- **Two layers:**
  - **Layer 1 (Top):** Diggable dirt blocks. The player moves and stands here.
  - **Layer 0 (Bottom):** Bedrock plane + Lego piece cells. This ensures the player never falls out of bounds.

### Cell Types
| Type | Description |
|------|--------------|
| Dirt | Diggable block on top layer |
| Air | Empty cell after digging |
| Bedrock | Base layer, solid and walkable |
| Piece | Part of the Lego shape on bottom layer |
| Coin | Coin embedded in a dirt block |

**Rule:** Only one Lego piece per map.

---

## 3) Player Mechanics

### Movement
- Player moves freely on the dirt grid surface (WASD or joystick).  
- Snap to cell centers when idle for alignment.  
- If the player digs the cell they’re standing on → falls to bedrock.  
- On **bedrock**, the player **can’t move horizontally** but can **jump back** up to adjacent dirt blocks (N/E/S/W).

### Jumping
- Player can jump from bedrock to an adjacent dirt cell.  
- If multiple options exist, jump toward the input direction.  
- If no reachable cell exists → show hint “Find a nearby block.”  
- If none within 2 cells → optional **Auto-Assist Teleport** (cost: -1 axe or -N coins).

### Digging
- Player can only dig the block **they are standing on**.  
- When digging:
  1. Play dig animation (in-place).  
  2. After 0.3s, consume 1 axe.  
  3. Check cell content: coin or piece.  
  4. Replace dirt → air.  
  5. If it’s a piece cell → increment piece’s revealed count.  
  6. If all piece cells revealed → trigger win.

### Camera
- **Third-person follow camera**, slightly tilted (35–45°).  
- Auto-adjusts height or ghosts player if occluded.

---

## 4) Rules & Systems

### Axes (Dig Charges)
- Each level starts with `axes_start` (configurable).  
- Every dig consumes 1 axe.  
- If all axes are used before completion → **Lose**.  
- Coins can purchase more axes in-level.

### Coins
- Spawn randomly inside dirt cells (configurable count).  
- When the containing dirt is dug → instantly collected.  
- **Usage:**
  - **In-level:** Buy additional axes.
  - **Out-of-level:** Buy cosmetic skins (player, pickaxe).

### Lego Piece
- Each piece is defined as a set of occupied grid coordinates on the bottom layer.  
- Shapes follow Tetromino (I, O, T, L, J, S, Z), extensible via data.  
- When all corresponding dirt cells above are removed → auto-collect → Win.  
- Visual: colored plastic 3D block mesh.

---

## 5) Content Generation

### Map Size
- Configurable per level (e.g., 8×8 → 12×12).

### Level Generation Steps
1. Create bedrock plane.  
2. Randomly select a Lego shape from allowed pool.  
3. Rotate & position within map bounds.  
4. Fill top layer with dirt.  
5. Embed coins randomly in `n` dirt blocks.  
6. Set starting axes, coin cost, and reward parameters.  
7. Save seed for reproducibility.

---

## 6) UI & Feedback

### HUD Elements
- Axes remaining (with + button to buy more).  
- Coins collected (level + total).  
- Target piece silhouette and reveal progress.  
- Context hint (“Jump to a nearby Dirt block”).

### Visual & Audio Feedback
| Action | Feedback |
|---------|-----------|
| Dig Dirt | Dust burst + thud sound |
| Reveal Piece | Plastic clack + highlight |
| Collect Coin | Sparkle + jingle |
| Win | Confetti burst + summary panel |
| Lose | Grayscale flash + retry menu |

---

## 7) Data Model (for Unity)

```csharp
struct CellCoord { public int x, y; }

enum TopCellType { Dirt, Air }
struct TopCell { public TopCellType type; public bool hasCoin; }

enum BottomCellType { Bedrock, Piece }
struct BottomCell { public BottomCellType type; public int pieceId; }

class PieceDef {
    public string id; // e.g., "T", "L"
    public CellCoord[] shape; // relative coordinates
}

class PlacedPiece {
    public int id;
    public string defId;
    public CellCoord origin;
    public CellCoord[] worldCells;
    public int revealedCount;
}

class LevelConfig {
    public int width, height;
    public string[] allowedPieceIds;
    public int coinsCount;
    public int axesStart;
    public int coinPerExtraAxe;
    public int extraAxePack;
    public int seed;
}
```

---

## 8) Core Logic

### Dig Resolution
```
If (TopCell.type == Dirt && axes > 0):
    axes -= 1
    if TopCell.hasCoin:
        coins += 1
    TopCell.type = Air
    if BottomCell.type == Piece:
        piece.revealedCount += 1
        if revealedCount == totalCells:
            TriggerWin()
    RemoveTopCollider()
    if player standing on removed cell:
        FallToBedrock()
```

### Jumping Logic
```
If on Bedrock and Jump pressed:
    Check 4 neighboring cells
    If neighbor has Dirt:
        Move player to that cell
    Else if none:
        Hint("Find a nearby block")
```

### Purchase Axes
```
If coins >= coinPerExtraAxe:
    coins -= coinPerExtraAxe
    axes += extraAxePack
    PlayPurchaseFX()
```

---

## 9) Unity Implementation Notes
- **Prefabs:** Dirt, Bedrock, Piece, Coin, Player.  
- **Managers:**
  - `GridManager`: grid data + world conversion.
  - `PieceManager`: track piece progress.
  - `EconomyManager`: coins, purchases.
  - `LevelGenerator`: procedural placement.  
- **Optimization:** GPU instancing for static geometry, object pooling for FX.

---

## 10) Edge Cases
1. Stuck on bedrock → Auto-assist teleport (cost -1 axe or -N coins).  
2. Dig with 0 axes → ignored; HUD shakes.  
3. Out-of-bounds dig → red highlight feedback.  
4. Piece on edge → clamp placement inside bounds.  
5. Coin above piece → valid; both resolve correctly.  
6. Camera blocked → auto-adjust height or ghost player.  
7. Player position rounding → snap to (x+0.5, y+0.5).

---

## 11) Default Balancing
| Parameter | Easy | Mid | Hard |
|------------|------|-----|------|
| Map size | 8×8 | 10×10 | 12×12 |
| Starting Axes | 18 | 24 | 30 |
| Dig Time | 0.3s | 0.3s | 0.3s |
| Coin Count | 6 | 8 | 10 |
| Axe Price | 5 coins | 5 coins | 5 coins |
| Axe Reward | +3 axes | +3 axes | +3 axes |

---

## 12) Game Flow

### Win
- Freeze input.  
- Celebrate (confetti, sfx).  
- Show summary (axes left, coins gained).  
- Buttons: `Next`, `Retry`, `Home`.

### Lose
- Offer to buy axes (if affordable).  
- If declined or no coins → lose screen (`Retry`, `Home`).

---

## 13) Extensibility
- Add new `PieceDef` shapes easily.  
- Multi-piece levels (future).  
- Add traps, obstacles, or destructible types.  
- Player upgrade system (optional future).

---

## 14) Testing Checklist
- Walk, fall, jump, dig, win, lose all functional.  
- Dig resolves coin and piece correctly.  
- Axes purchase logic consistent.  
- Procedural generator produces valid placements.  
- No infinite fall or stuck situations.

---

© 2025 Brick Digger Project – Unity Design Specification v1.1
