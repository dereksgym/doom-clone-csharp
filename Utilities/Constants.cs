namespace DoomClone.Utilities;

public static class Constants
{
    // Screen
    public const int SCREEN_WIDTH = 1024;
    public const int SCREEN_HEIGHT = 768;
    public const int TARGET_FPS = 60;
    public const float TARGET_FRAME_TIME = 1.0f / TARGET_FPS;

    // Rendering
    public const float FOV = 60.0f;  // Field of view in degrees
    public const float MAX_RENDER_DISTANCE = 20.0f;
    public const float WALL_HEIGHT = 1.0f;
    public const int TEXTURE_SIZE = 64;

    // Player
    public const float PLAYER_SPEED = 5.0f;
    public const float PLAYER_TURN_SPEED = 180.0f;  // Degrees per second
    public const float PLAYER_HEIGHT = 0.5f;
    public const float PLAYER_RADIUS = 0.25f;

    // World
    public const int WORLD_WIDTH = 24;
    public const int WORLD_HEIGHT = 24;
    public const float TILE_SIZE = 1.0f;

    // Physics
    public const float GRAVITY = 9.81f;
    public const float COLLISION_EPSILON = 0.1f;

    // Enemy
    public const float ENEMY_SPEED = 2.0f;
    public const float ENEMY_VISION_RANGE = 10.0f;
    public const float ENEMY_ATTACK_RANGE = 5.0f;

    // HUD
    public const int HUD_HEIGHT = 100;
    public const int HUD_BG_COLOR = 0x444444;

    // Colors (RGB)
    public const uint COLOR_BLACK = 0x000000FF;
    public const uint COLOR_WHITE = 0xFFFFFFFF;
    public const uint COLOR_RED = 0xFF0000FF;
    public const uint COLOR_GREEN = 0x00FF00FF;
    public const uint COLOR_BLUE = 0x0000FFFF;
    public const uint COLOR_GRAY = 0x808080FF;
    public const uint COLOR_DARK_GRAY = 0x404040FF;
    public const uint COLOR_YELLOW = 0xFFFF00FF;
}
