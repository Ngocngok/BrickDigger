# Character Shop - Quick Reference

## 🎮 How It Works

### Character Selection
- **Browse**: Use < and > buttons to navigate
- **Preview**: See 3D character with idle animation
- **Total**: 8 characters available (Character_1 to Character_8)

### Purchasing
- **Free**: Character_1 (always unlocked)
- **Price**: 20 coins per character (Characters 2-8)
- **Buy**: Click green "BUY (20)" button when affordable

### Equipping
- **Equip**: Click blue "EQUIP" button for unlocked characters
- **Selected**: Grey "SELECTED" button shows current character
- **Gameplay**: Selected character spawns in game

---

## 🎨 Button States

| Button Text | Color | Meaning |
|-------------|-------|---------|
| BUY (20) | 🟢 Green | Can afford, click to buy |
| BUY (20) | ⚫ Grey | Not enough coins |
| EQUIP | 🔵 Blue | Owned, click to equip |
| SELECTED | ⚫ Dark Grey | Currently equipped |

---

## 💰 Earning Coins

### Coin Sources
- Find coins in dirt blocks during gameplay
- Win bonus: +5 coins per level
- Coins persist across levels

### Spending Coins
- Buy characters: 20 coins each
- Buy axes in-game: 5 coins for 3 axes

---

## 🎯 Quick Actions

### Browse Characters
```
< Button: Previous character
> Button: Next character
Wraps around: 1 ↔ 8
```

### Buy Character
```
1. Navigate to locked character
2. Check coins (need 20)
3. Click green "BUY (20)" button
4. Character unlocked!
```

### Equip Character
```
1. Navigate to unlocked character
2. Click blue "EQUIP" button
3. Character equipped!
4. Will spawn in next game
```

---

## 🔧 Configuration

### In CharacterShopManager Inspector
- `Total Characters For Sale`: 8 (default)
- `Character Price`: 20 coins (default)
- `Character Spawn Point`: Where preview spawns
- `Idle Animator Controller`: PlayerAnimatorController

---

## 📋 Checklist

### First Time Setup
- [x] CharacterShopManager in scene
- [x] Character preview camera configured
- [x] UI buttons connected
- [x] Animator controller assigned
- [x] Character_1 unlocked by default

### Testing
- [ ] Navigate with Next/Prev buttons
- [ ] Verify character preview updates
- [ ] Buy a character with coins
- [ ] Equip different character
- [ ] Play game and verify correct character spawns
- [ ] Close and reopen game to verify persistence

---

## 🐛 Common Issues

**Character not visible?**
→ Check CharacterPreviewCamera depth = -1

**Button not working?**
→ Verify EventSystem exists in scene

**Wrong character in game?**
→ Equip character in shop before playing

**Coins not updating?**
→ Check HomeScene syncs coins on Start

---

## 📁 Key Files

```
Assets/Scripts/UI/CharacterShopManager.cs
Assets/Scenes/HomeScene.unity
Assets/Animation/PlayerAnimatorController.controller
```

---

## 💡 Tips

- Character_1 is always free
- Earn coins by playing levels
- Selected character persists between sessions
- Preview shows idle animation
- All characters share same animations

---

For detailed documentation, see:
`Assets/Docs/CharacterShop_Documentation.md`
