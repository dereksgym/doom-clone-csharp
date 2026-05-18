namespace DoomClone.Rendering;

using DoomClone.Core;
using DoomClone.Utilities;
using SDL2;
using System;
using System.Collections.Generic;

public class SpriteRenderer
{
    private IntPtr _renderer;
    private int _screenWidth;
    private int _screenHeight;

    private struct SpriteDrawData
    {
        public int ScreenX;
        public int ScreenY;
        public int Width;
        public int Height;
        public float Distance;
    }

    public SpriteRenderer(IntPtr renderer, int screenWidth, int screenHeight)
    {
        _renderer = renderer;
        _screenWidth = screenWidth;
        _screenHeight = screenHeight;
    }

    public void Render(Player player, World world)
    {
        var spritesToDraw = new List<SpriteDrawData>();

        // Collect all sprites to draw
        foreach (var entity in world.Entities)
        {
            var spriteData = ProjectSprite(entity, player);
            if (spriteData.HasValue)
                spritesToDraw.Add(spriteData.Value);
        }

        // Sort by distance (painter's algorithm)
        spritesToDraw.Sort((a, b) => b.Distance.CompareTo(a.Distance));

        // Draw sprites
        foreach (var sprite in spritesToDraw)
        {
            DrawSprite(sprite);
        }
    }

    private SpriteDrawData? ProjectSprite(Entity.Entity entity, Player player)
    {
        // Vector from player to entity
        var toEntity = entity.Position - player.Position;
        float distance = toEntity.Length;

        if (distance > Constants.MAX_RENDER_DISTANCE || distance < 0.1f)
            return null;

        // Calculate angle relative to player
        float entityAngle = MathHelper.ToDegrees(MathF.Atan2(toEntity.Z, toEntity.X));
        float relativeAngle = entityAngle - player.Angle;

        // Normalize angle
        while (relativeAngle > 180) relativeAngle -= 360;
        while (relativeAngle < -180) relativeAngle += 360;

        // Only draw if visible
        if (Math.Abs(relativeAngle) > Constants.FOV / 2)
            return null;

        // Project to screen
        float screenX = _screenWidth / 2 + relativeAngle / Constants.FOV * _screenWidth;
        
        // Calculate sprite height based on distance
        float spriteHeight = (Constants.WALL_HEIGHT / distance) * (_screenWidth / (2 * MathF.Tan(MathHelper.ToRadians(Constants.FOV / 2))));
        float spriteWidth = spriteHeight * 0.8f;  // Aspect ratio

        int screenY = (int)(_screenHeight / 2 - spriteHeight / 2);

        return new SpriteDrawData
        {
            ScreenX = (int)screenX,
            ScreenY = screenY,
            Width = (int)spriteWidth,
            Height = (int)spriteHeight,
            Distance = distance
        };
    }

    private void DrawSprite(SpriteDrawData sprite)
    {
        // Draw a simple sprite as a colored rectangle
        var rect = new SDL.SDL_Rect
        {
            x = Math.Max(0, Math.Min(_screenWidth, sprite.ScreenX - sprite.Width / 2)),
            y = Math.Max(0, Math.Min(_screenHeight, sprite.ScreenY)),
            w = sprite.Width,
            h = sprite.Height
        };

        // Clamp to screen
        if (rect.x + rect.w > _screenWidth) rect.w = _screenWidth - rect.x;
        if (rect.y + rect.h > _screenHeight) rect.h = _screenHeight - rect.y;

        if (rect.w > 0 && rect.h > 0)
        {
            // Draw as red demon
            SDL.SDL_SetRenderDrawColor(_renderer, 200, 0, 0, 255);
            SDL.SDL_RenderFillRect(_renderer, ref rect);
            SDL.SDL_SetRenderDrawColor(_renderer, 255, 0, 0, 255);
            SDL.SDL_RenderDrawRect(_renderer, ref rect);
        }
    }
}
