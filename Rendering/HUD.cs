namespace DoomClone.Rendering;

using DoomClone.Core;
using DoomClone.Utilities;
using SDL2;
using System;

public class HUD
{
    private IntPtr _renderer;
    private int _screenWidth;
    private int _screenHeight;
    private const int MINIMAP_SIZE = 150;
    private const int MINIMAP_SCALE = 6;

    public HUD(IntPtr renderer, int width, int height)
    {
        _renderer = renderer;
        _screenWidth = width;
        _screenHeight = height;
    }

    public void RenderMinimap(Player player, World world)
    {
        int minimapX = _screenWidth - MINIMAP_SIZE - 10;
        int minimapY = 10;

        // Draw background
        var bgRect = new SDL.SDL_Rect { x = minimapX, y = minimapY, w = MINIMAP_SIZE, h = MINIMAP_SIZE };
        SDL.SDL_SetRenderDrawColor(_renderer, 30, 30, 30, 200);
        SDL.SDL_RenderFillRect(_renderer, ref bgRect);

        // Draw border
        SDL.SDL_SetRenderDrawColor(_renderer, 200, 200, 200, 255);
        SDL.SDL_RenderDrawRect(_renderer, ref bgRect);

        // Draw tiles
        for (int x = 0; x < world.Width; x++)
        {
            for (int y = 0; y < world.Height; y++)
            {
                if (world.GetTile(x, y) == '#')
                {
                    int px = minimapX + x * MINIMAP_SCALE;
                    int py = minimapY + y * MINIMAP_SCALE;
                    var rect = new SDL.SDL_Rect { x = px, y = py, w = MINIMAP_SCALE - 1, h = MINIMAP_SCALE - 1 };
                    SDL.SDL_SetRenderDrawColor(_renderer, 100, 100, 100, 255);
                    SDL.SDL_RenderFillRect(_renderer, ref rect);
                }
            }
        }

        // Draw entities
        foreach (var entity in world.Entities)
        {
            int px = minimapX + (int)entity.Position.X * MINIMAP_SCALE;
            int py = minimapY + (int)entity.Position.Z * MINIMAP_SCALE;
            var rect = new SDL.SDL_Rect { x = px - 2, y = py - 2, w = 4, h = 4 };
            SDL.SDL_SetRenderDrawColor(_renderer, 255, 0, 0, 255);
            SDL.SDL_RenderFillRect(_renderer, ref rect);
        }

        // Draw player
        int playerMinimapX = minimapX + (int)player.Position.X * MINIMAP_SCALE;
        int playerMinimapY = minimapY + (int)player.Position.Z * MINIMAP_SCALE;
        var playerRect = new SDL.SDL_Rect { x = playerMinimapX - 3, y = playerMinimapY - 3, w = 6, h = 6 };
        SDL.SDL_SetRenderDrawColor(_renderer, 0, 255, 0, 255);
        SDL.SDL_RenderFillRect(_renderer, ref playerRect);

        // Draw player direction
        float playerAngleRad = MathHelper.ToRadians(player.Angle);
        float dirX = MathF.Cos(playerAngleRad) * 15;
        float dirY = MathF.Sin(playerAngleRad) * 15;
        SDL.SDL_SetRenderDrawColor(_renderer, 0, 255, 0, 255);
        SDL.SDL_RenderDrawLine(_renderer, playerMinimapX, playerMinimapY, (int)(playerMinimapX + dirX), (int)(playerMinimapY + dirY));
    }
}
