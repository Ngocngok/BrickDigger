# Character System - Quick Reference

## 🎮 Quick Start

### Testing Animations (Play Mode)
1. Enter Play mode
2. Move with WASD → **Walking animation**
3. Stand still → **Idle animation**
4. Press Space → **Jump animation**
5. Press E → **Dig animation**

### Changing Characters (Editor)
1. Select Player GameObject
2. CharacterSpawner → Default Character Index → Change (1-120)
3. Menu: `Tools > Setup Player Character`

### Changing Characters (Code)
```csharp
CharacterSpawner spawner = FindObjectOfType<CharacterSpawner>();
spawner.ChangeCharacter(5); // Character_5
```

---

## 📋 Animation Parameters

| Parameter | Type | Purpose |
|-----------|------|---------|
| Speed | Float | 0 = Idle, >0.1 = Walking |
| IsJumping | Bool | true = Jump animation |
| Dig | Trigger | Triggers shovel animation |

---

## 🎨 Available Characters

- **Total**: 120 characters
- **Range**: Character_1 to Character_120
- **Path**: `Assets/Layer lab/3D Casual Character/.../Characters/`

---

## 🔧 Key Components

### CharacterSpawner
- **Location**: Player GameObject
- **Purpose**: Spawns and manages character models
- **Key Settings**: 
  - Default Character Index (1-120)
  - Character Scale (default: 1.0)

### PlayerAnimatorController
- **Location**: `Assets/Animation/PlayerAnimatorController.controller`
- **States**: Idle, Walking, Jump, Shovel
- **Assigned To**: Spawned character's Animator

---

## 🧪 Testing Tools

### TestCharacterAnimations Script
Add to Player GameObject for testing:
- `1-3`: Test speed values
- `J`: Jump animation
- `D`: Dig animation
- `C`: Change character (cycle 1-10)
- `I`: Show info in console

---

## 📁 Important Files

```
Assets/
├── Scripts/Player/CharacterSpawner.cs
├── Scripts/Player/PlayerController.cs (updated)
├── Animation/PlayerAnimatorController.controller
└── Docs/
    ├── CharacterSystem_Documentation.md (full docs)
    ├── CharacterSystem_Implementation_Summary.md
    └── CharacterSystem_QuickReference.md (this file)
```

---

## 🐛 Common Issues

**Character not visible?**
→ Check Capsule is hidden, scale is 1.0

**Animations not playing?**
→ Verify animator controller is assigned

**Wrong character spawns?**
→ Check PlayerPrefs or Default Character Index

---

## 💡 Tips

- Character selection persists via PlayerPrefs
- All characters share same animations
- Only one character active at a time
- Use scale 0.8-1.2 for size adjustments

---

For detailed documentation, see:
`Assets/Docs/CharacterSystem_Documentation.md`
