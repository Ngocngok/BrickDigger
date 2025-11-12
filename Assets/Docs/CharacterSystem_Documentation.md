# Character System Documentation

## Overview
The character spawning system allows dynamic loading and switching of character models at runtime. Characters are spawned from a library of 120+ character prefabs with full animation support.

## Components

### 1. CharacterSpawner.cs
Located: `Assets/Scripts/Player/CharacterSpawner.cs`

**Purpose**: Manages spawning, switching, and persistence of player character models.

**Key Features**:
- Spawns character models at game start
- Supports 120+ character variations (Character_1 to Character_120)
- Saves character selection in PlayerPrefs
- Hides/replaces placeholder visuals (Capsule)
- Provides animator reference to PlayerController

**Inspector Settings**:
- `Character Prefab Path`: Path to character prefabs folder
- `Default Character Index`: Which character to spawn by default (1-120)
- `Spawn Parent`: Where to spawn the character (usually Player GameObject)
- `Local Position/Rotation`: Transform offset for spawned character
- `Character Scale`: Scale multiplier for character model
- `Visual To Replace`: Placeholder visual to hide (Capsule)

**Public Methods**:
```csharp
// Spawn the saved/default character
void SpawnCharacter()

// Spawn a specific character by index (1-120)
void SpawnCharacter(int characterIndex)

// Change to a different character
void ChangeCharacter(int newCharacterIndex)

// Get the character's animator
Animator GetAnimator()

// Get the spawned character GameObject
GameObject GetSpawnedCharacter()

// Get current character index
int GetCurrentCharacterIndex()
```

### 2. PlayerAnimatorController
Located: `Assets/Animation/PlayerAnimatorController.controller`

**Animation States**:
1. **Idle** - Default standing animation
2. **Walking** - Movement animation
3. **Jump** - Jumping animation
4. **Shovel** - Digging animation

**Parameters**:
- `Speed` (Float) - Movement speed, controls Idle ↔ Walking transition
- `IsJumping` (Bool) - Triggers jump animation
- `IsDigging` (Bool) - Indicates digging state
- `Dig` (Trigger) - Triggers shovel animation

**Transitions**:
- Idle → Walking: When Speed > 0.1
- Walking → Idle: When Speed < 0.1
- Any State → Jump: When IsJumping = true
- Jump → Idle: When animation finishes and IsJumping = false
- Any State → Shovel: When Dig trigger is set
- Shovel → Idle: When animation finishes

### 3. PlayerController Integration
The PlayerController has been updated to work with the character system:

**Animation Triggers**:
- **Movement**: Sets `Speed` parameter based on movement input
- **Jump**: Sets `IsJumping` to true when jumping
- **Dig**: Triggers `Dig` when digging starts

## Animation Clips Used

| State | Animation File | Description |
|-------|---------------|-------------|
| Idle | Anim@Stand_Idle1.FBX | Standing idle animation |
| Walking | Anim@Action_Walk.FBX | Walking animation |
| Jump | Anim@Action_Jump.FBX | Jumping animation |
| Shovel | Anim@Interaction_Shovel.FBX | Digging with shovel |

## Setup Instructions

### Initial Setup (Already Done)
1. ✅ CharacterSpawner component added to Player GameObject
2. ✅ Animator Controller created with all states and transitions
3. ✅ Character_1 spawned as default character
4. ✅ Capsule placeholder hidden
5. ✅ PlayerController updated to trigger animations

### Testing the System

#### In Editor:
1. Open GameplayScene
2. Select Player GameObject
3. In CharacterSpawner component, change `Default Character Index` to try different characters (1-120)
4. Use menu: `Tools > Setup Player Character` to respawn with new character

#### At Runtime:
1. Enter Play mode
2. Use WASD or joystick to move - Walking animation should play
3. Press Space or Jump button - Jump animation should play
4. Press E or Dig button - Shovel animation should play
5. Stand still - Idle animation should play

## Character Selection System

### Current Implementation
- Default character: Character_1
- Selection saved in PlayerPrefs with key: "SelectedCharacter"
- Persists between game sessions

### Future Enhancement: Character Selection Screen
To add a character selection screen:

```csharp
// Example: In a character selection UI script
public void OnCharacterSelected(int characterIndex)
{
    // Save selection
    PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
    PlayerPrefs.Save();
    
    // Preview character (optional)
    // characterSpawner.SpawnCharacter(characterIndex);
}
```

## Changing Characters at Runtime

### Method 1: Via Script
```csharp
// Get reference to CharacterSpawner
CharacterSpawner spawner = FindObjectOfType<CharacterSpawner>();

// Change to Character_5
spawner.ChangeCharacter(5);
```

### Method 2: Via Inspector (Editor Only)
1. Select Player GameObject
2. Find CharacterSpawner component
3. Change `Default Character Index`
4. Run menu: `Tools > Setup Player Character`

## Character Library

**Available Characters**: 120 variations
- Path: `Assets/Layer lab/3D Casual Character/3D Casual Character/Prefabs/Characters/`
- Naming: Character_1.prefab to Character_120.prefab
- All characters share the same skeleton and animations

## Animation System Details

### Animator Controller Structure
```
Root State Machine
├── Idle (Default State)
├── Walking
├── Jump
└── Shovel

Transitions:
- Idle ↔ Walking (based on Speed)
- Any State → Jump (when IsJumping)
- Any State → Shovel (when Dig trigger)
- Jump/Shovel → Idle (when finished)
```

### Animation Blending
- Idle ↔ Walking: 0.2s blend time
- Jump: 0.1s blend time, no exit time
- Shovel: 0.1s blend time, plays once then returns to Idle

## Troubleshooting

### Character Not Spawning
**Problem**: Character doesn't appear in scene
**Solutions**:
1. Check CharacterSpawner component is on Player GameObject
2. Verify character index is valid (1-120)
3. Check console for error messages
4. Ensure character prefabs exist in the specified path

### Animations Not Playing
**Problem**: Character spawns but doesn't animate
**Solutions**:
1. Check Animator component has PlayerAnimatorController assigned
2. Verify animation clips are properly imported
3. Check PlayerController is triggering animation parameters
4. Use Animator window to debug state transitions

### Character Scale Issues
**Problem**: Character is too big or too small
**Solutions**:
1. Adjust `Character Scale` in CharacterSpawner component
2. Default scale is 1.0, try values between 0.8-1.2
3. Ensure CharacterController height matches character size

### Character Position Offset
**Problem**: Character is not aligned with Player GameObject
**Solutions**:
1. Adjust `Local Position` in CharacterSpawner component
2. Default is (0, 0, 0) - character spawns at Player's position
3. May need Y offset if character feet don't touch ground

## Performance Considerations

- Characters use GPU skinning for efficient animation
- Only one character is active at a time
- Previous character is destroyed when switching
- Animator controller is shared across all characters
- Animation clips are loaded from asset database (not duplicated)

## File Structure

```
Assets/
├── Scripts/
│   ├── Player/
│   │   ├── CharacterSpawner.cs
│   │   └── PlayerController.cs (updated)
│   └── Setup/
│       ├── CreatePlayerAnimatorController.cs
│       └── SetupPlayerCharacter.cs
├── Animation/
│   └── PlayerAnimatorController.controller
├── Layer lab/
│   └── 3D Casual Character/
│       ├── 3D Casual Character/
│       │   └── Prefabs/
│       │       └── Characters/
│       │           ├── Character_1.prefab
│       │           ├── Character_2.prefab
│       │           └── ... (up to Character_120.prefab)
│       └── Animation/
│           ├── Anim@Stand_Idle1.FBX
│           ├── Anim@Action_Walk.FBX
│           ├── Anim@Action_Jump.FBX
│           └── Anim@Interaction_Shovel.FBX
└── Docs/
    └── CharacterSystem_Documentation.md (this file)
```

## API Reference

### CharacterSpawner

#### Properties
- `characterPrefabPath` (string): Path to character prefabs
- `defaultCharacterIndex` (int): Default character to spawn
- `spawnParent` (Transform): Parent transform for spawned character
- `localPosition` (Vector3): Local position offset
- `localRotation` (Vector3): Local rotation offset
- `characterScale` (float): Scale multiplier
- `visualToReplace` (GameObject): Placeholder to hide

#### Methods
- `SpawnCharacter()`: Spawn saved/default character
- `SpawnCharacter(int)`: Spawn specific character
- `ChangeCharacter(int)`: Switch to different character
- `GetAnimator()`: Get character's Animator component
- `GetSpawnedCharacter()`: Get spawned character GameObject
- `GetCurrentCharacterIndex()`: Get current character index

### PlayerController Animation Integration

#### Animation Parameters Set by PlayerController
- `Speed`: Set based on movement input magnitude
- `IsJumping`: Set to true when jumping, false when grounded
- `Dig`: Triggered when dig action is performed

## Future Enhancements

### Planned Features
1. **Character Selection UI**: Menu to preview and select characters
2. **Character Unlocking**: Unlock characters through gameplay
3. **Character Customization**: Mix and match character parts
4. **Skin System**: Apply different materials/colors to characters
5. **Animation Variations**: Multiple idle/walk animations per character
6. **Emotes**: Add dance and emoji animations to character

### Integration Points
- **Shop System**: Purchase characters with coins
- **Achievement System**: Unlock characters via achievements
- **Save System**: Save unlocked characters
- **Multiplayer**: Show other players' character selections

## Credits
- Character Models: Layer lab 3D Casual Character Pack
- Animations: Layer lab 3D Casual Character Animation Pack
- Implementation: Brick Digger 3D Development Team

---

Last Updated: 2025-11-12
Version: 1.0
