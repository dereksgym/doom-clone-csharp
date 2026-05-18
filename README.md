# Doom Clone in C#

A lower-level Doom-inspired first-person shooter clone written in C# using SDL2 for graphics and input handling.

## Features

- **3D Ray-casting Engine**: Pseudo-3D rendering using raycasting (like original Doom)
- **Player Movement**: WASD for movement, mouse/arrow keys for turning
- **Simple Level Map**: 2D grid-based level system with walls and open spaces
- **Sprite Rendering**: Enemies and items rendered as billboards
- **Basic Enemy AI**: Simple wandering and line-of-sight detection
- **Weapon System**: Basic attack mechanics
- **HUD**: Health, ammo, and mini-map display
- **Collision Detection**: Player-wall and enemy-wall collisions

## Requirements

- **.NET 6.0** or higher
- **SDL2** graphics library
- **SDL2-CS** (C# SDL2 bindings)

## Installation

### Windows
```bash
# Clone the repository
git clone https://github.com/dereksgym/doom-clone-csharp.git
cd doom-clone-csharp

# Restore dependencies
dotnet restore

# Build
dotnet build

# Run
dotnet run
```

### Linux
```bash
# Install SDL2
sudo apt-get install libsdl2-dev libsdl2-image-dev

# Clone and build
git clone https://github.com/dereksgym/doom-clone-csharp.git
cd doom-clone-csharp
dotnet restore
dotnet build
dotnet run
```

### macOS
```bash
# Install SDL2 via Homebrew
brew install sdl2 sdl2-image

# Clone and build
git clone https://github.com/dereksgym/doom-clone-csharp.git
cd doom-clone-csharp
dotnet restore
dotnet build
dotnet run
```

## Controls

- **W** - Move Forward
- **S** - Move Backward
- **A** - Strafe Left
- **D** - Strafe Right
- **Left/Right Arrow** or **Mouse** - Turn Left/Right
- **Up/Down Arrow** - Look Up/Down
- **Space** - Fire Weapon
- **E** - Open Door / Interact
- **M** - Toggle Mini-map
- **ESC** - Exit Game

## Project Structure

```
├── DoomClone.csproj          # Project file
├── Program.cs                # Entry point
├── Core/
│   ├── Game.cs              # Main game loop
│   ├── Player.cs            # Player class
│   ├── World.cs             # Level/map system
│   └── Physics.cs           # Collision detection
├── Rendering/
│   ├── Renderer.cs          # Main renderer
│   ├── RaycastEngine.cs     # Raycasting logic
│   ├── SpriteRenderer.cs    # Enemy/item rendering
│   └── HUD.cs               # UI rendering
├── Entities/
│   ├── Entity.cs            # Base entity class
│   ├── Enemy.cs             # Enemy implementation
│   └── Item.cs              # Items/pickups
├── Input/
│   └── InputManager.cs      # Keyboard/mouse input
└── Utilities/
    ├── Vector3.cs           # 3D vector math
    ├── Ray.cs               # Raycasting utilities
    └── Constants.cs         # Game constants
```

## How the Raycasting Engine Works

1. **Ray Casting**: For each column of pixels on screen, cast a ray from the player's position
2. **Wall Intersection**: Find where the ray hits a wall
3. **Distance Calculation**: Calculate distance to determine wall height (closer = taller)
4. **Column Rendering**: Draw a vertical column of wall texture based on distance
5. **Floor/Ceiling**: Render flat colors for floor and ceiling
6. **Sprite Rendering**: Render enemies/items as textured quads

## Customization

### Level Editor
Edit `assets/level.txt` to create custom maps:
- `#` = Wall
- `.` = Open space
- `P` = Player spawn
- `E` = Enemy spawn
- `@` = Item spawn

### Textures
Place `.bmp` or `.png` files in `assets/textures/` and update sprite definitions.

### Game Settings
Modify constants in `Utilities/Constants.cs`:
- `SCREEN_WIDTH` / `SCREEN_HEIGHT`: Resolution
- `FOV`: Field of view angle
- `MAX_RENDER_DISTANCE`: How far to render
- `PLAYER_SPEED`: Movement speed

## Performance

- Optimized raycasting for real-time 60+ FPS rendering
- Efficient sprite sorting for transparency
- Collision grid for fast entity queries

## License

MIT License - See LICENSE file for details

## References

- [Doom Engine Architecture](https://en.wikipedia.org/wiki/Doom_engine)
- [Raycasting Tutorial](https://lodev.org/cgtutor/raycasting.html)
- [SDL2 Documentation](https://wiki.libsdl.org/)
