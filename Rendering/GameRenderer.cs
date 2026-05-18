namespace DoomClone.Rendering;

using DoomClone.Core;
using DoomClone.Utilities;
using SDL2;
using System;

public class GameRenderer
{
    private IntPtr _renderer;
    private int _screenWidth;
    private int _screenHeight;
    private RaycastEngine _raycastEngine;
    private SpriteRenderer _spriteRenderer;
    private HUD _hud;

    public GameRenderer(IntPtr renderer, int width, int height)
    {
        _renderer = renderer;
        _screenWidth = width;
        _screenHeight = height;
        _raycastEngine = new RaycastEngine(renderer, width, height);
        _spriteRenderer = new SpriteRenderer(renderer, width, height);
        _hud = new HUD(renderer, width, height);
    }

    public void Render(Player player, World world, float deltaTime)
    {
        // Render 3D view using raycasting
        _raycastEngine.Render(player, world);

        // Render sprites (enemies, items)
        _spriteRenderer.Render(player, world);
    }

    public void RenderMinimap(Player player, World world)
    {
        _hud.RenderMinimap(player, world);
    }
}
