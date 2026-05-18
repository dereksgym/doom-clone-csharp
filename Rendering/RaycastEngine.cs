namespace DoomClone.Rendering;

using DoomClone.Core;
using DoomClone.Utilities;
using SDL2;
using System;
using System.Collections.Generic;

public class RaycastEngine
{
    private IntPtr _renderer;
    private int _screenWidth;
    private int _screenHeight;
    private float[] _depthBuffer;

    public RaycastEngine(IntPtr renderer, int width, int height)
    {
        _renderer = renderer;
        _screenWidth = width;
        _screenHeight = height;
        _depthBuffer = new float[width];
    }

    public void Render(Player player, World world)
    {
        // Render floor and ceiling
        RenderFloorCeiling();

        // Cast rays for each column
        for (int x = 0; x < _screenWidth; x++)
        {
            float columnAngle = -Constants.FOV / 2f + (x / (float)_screenWidth) * Constants.FOV;
            float rayAngle = player.Angle + columnAngle;
            float rayRadians = MathHelper.ToRadians(rayAngle);

            var rayDir = new Vector3(
                MathF.Cos(rayRadians),
                0,
                MathF.Sin(rayRadians)
            );

            var ray = new Ray(player.Position, rayDir);

            // Find wall intersection
            float distance = CastRay(ray, world, out bool isVerticalWall);
            distance = Math.Min(distance, Constants.MAX_RENDER_DISTANCE);
            _depthBuffer[x] = distance;

            if (distance < Constants.MAX_RENDER_DISTANCE)
            {
                // Calculate wall height
                float wallHeight = (Constants.WALL_HEIGHT / distance) * (Constants.SCREEN_WIDTH / (2 * MathF.Tan(MathHelper.ToRadians(Constants.FOV / 2))));
                
                // Draw wall column
                DrawWallColumn(x, wallHeight, distance, isVerticalWall);
            }
        }
    }

    private void RenderFloorCeiling()
    {
        // Ceiling (top half)
        var ceilingRect = new SDL.SDL_Rect { x = 0, y = 0, w = _screenWidth, h = _screenHeight / 2 };
        SDL.SDL_SetRenderDrawColor(_renderer, 50, 50, 50, 255);
        SDL.SDL_RenderFillRect(_renderer, ref ceilingRect);

        // Floor (bottom half)
        var floorRect = new SDL.SDL_Rect { x = 0, y = _screenHeight / 2, w = _screenWidth, h = _screenHeight / 2 };
        SDL.SDL_SetRenderDrawColor(_renderer, 100, 100, 100, 255);
        SDL.SDL_RenderFillRect(_renderer, ref floorRect);
    }

    private void DrawWallColumn(int x, float wallHeight, float distance, bool isVerticalWall)
    {
        int startY = (int)(_screenHeight / 2 - wallHeight / 2);
        int endY = (int)(_screenHeight / 2 + wallHeight / 2);
        startY = Math.Max(0, startY);
        endY = Math.Min(_screenHeight, endY);

        // Color based on distance and wall orientation (for shading effect)
        byte color = (byte)Math.Max(50, Math.Min(255, 200 - distance * 15));
        byte darkerColor = (byte)(color * 0.7f);

        // Alternate shading for vertical vs horizontal walls
        if (isVerticalWall)
            SDL.SDL_SetRenderDrawColor(_renderer, color, color, color, 255);
        else
            SDL.SDL_SetRenderDrawColor(_renderer, darkerColor, darkerColor, darkerColor, 255);

        SDL.SDL_RenderDrawLine(_renderer, x, startY, x, endY);
    }

    private float CastRay(Ray ray, World world, out bool isVerticalWall)
    {
        float minDistance = Constants.MAX_RENDER_DISTANCE;
        isVerticalWall = false;

        // Step through the ray in small increments
        float step = 0.05f;
        for (float dist = step; dist < Constants.MAX_RENDER_DISTANCE; dist += step)
        {
            var point = ray.GetPoint(dist);

            if (world.IsWall(point.X, point.Z, 0.1f))
            {
                // Determine if this is a vertical or horizontal wall
                var prevPoint = ray.GetPoint(dist - step);
                isVerticalWall = Math.Abs(point.X - prevPoint.X) > Math.Abs(point.Z - prevPoint.Z);
                return dist;
            }
        }

        return minDistance;
    }
}
