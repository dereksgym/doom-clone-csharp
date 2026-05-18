namespace DoomClone.Core;

using DoomClone.Rendering;
using DoomClone.Input;
using DoomClone.Utilities;
using SDL2;
using System;

public class Game : IDisposable
{
    private IntPtr _window;
    private IntPtr _renderer;
    private Player _player;
    private World _world;
    private InputManager _inputManager;
    private GameRenderer _gameRenderer;
    private bool _running = true;
    private float _deltaTime = 0;
    private bool _showMinimap = true;

    private ulong _lastFrameTime;
    private ulong _frameCount;

    public Game(int screenWidth, int screenHeight)
    {
        SDL.SDL_Init(SDL.SDL_INIT_VIDEO | SDL.SDL_INIT_EVENTS);

        _window = SDL.SDL_CreateWindow(
            "Doom Clone - C#",
            SDL.SDL_WINDOWPOS_CENTERED,
            SDL.SDL_WINDOWPOS_CENTERED,
            screenWidth,
            screenHeight,
            SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN
        );

        if (_window == IntPtr.Zero)
            throw new Exception($"Failed to create SDL window: {SDL.SDL_GetError()}");

        _renderer = SDL.SDL_CreateRenderer(
            _window,
            -1,
            SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC
        );

        if (_renderer == IntPtr.Zero)
            throw new Exception($"Failed to create SDL renderer: {SDL.SDL_GetError()}");

        // Initialize game systems
        _world = new World();
        _player = new Player(new Vector3(12, Constants.PLAYER_HEIGHT, 12));
        _inputManager = new InputManager();
        _gameRenderer = new GameRenderer(_renderer, screenWidth, screenHeight);

        // Spawn some enemies
        _world.AddEntity(new Entity.Enemy(new Vector3(5, 0, 5)));
        _world.AddEntity(new Entity.Enemy(new Vector3(18, 0, 18)));
        _world.AddEntity(new Entity.Enemy(new Vector3(12, 0, 18)));

        _lastFrameTime = SDL.SDL_GetTicks64();
        _frameCount = 0;
    }

    public void Run()
    {
        while (_running)
        {
            Update();
            Render();
        }
    }

    private void Update()
    {
        // Calculate delta time
        ulong currentTime = SDL.SDL_GetTicks64();
        _deltaTime = Math.Min((float)(currentTime - _lastFrameTime) / 1000f, 0.05f);  // Cap at 50ms
        _lastFrameTime = currentTime;

        // Update input
        _inputManager.Update();
        if (_inputManager.IsKeyPressed(SDL.SDL_Scancode.SDL_SCANCODE_ESCAPE))
            _running = false;

        if (_inputManager.IsKeyPressed(SDL.SDL_Scancode.SDL_SCANCODE_M))
            _showMinimap = !_showMinimap;

        // Update world
        _world.Update(_deltaTime);
        _player.Update(_inputManager, _world, _deltaTime);

        _frameCount++;
    }

    private void Render()
    {
        // Clear screen
        SDL.SDL_SetRenderDrawColor(_renderer, 0, 0, 0, 255);
        SDL.SDL_RenderClear(_renderer);

        // Render game
        _gameRenderer.Render(_player, _world, _deltaTime);

        // Render HUD
        RenderHUD();

        // Render minimap if enabled
        if (_showMinimap)
            _gameRenderer.RenderMinimap(_player, _world);

        // Present
        SDL.SDL_RenderPresent(_renderer);
    }

    private void RenderHUD()
    {
        // Health bar
        var healthWidth = (int)(_player.Health / 100f * 200);
        var healthRect = new SDL.SDL_Rect { x = 10, y = 10, w = healthWidth, h = 20 };
        SDL.SDL_SetRenderDrawColor(_renderer, 0, 255, 0, 255);
        SDL.SDL_RenderFillRect(_renderer, ref healthRect);

        // Health bar outline
        var healthOutlineRect = new SDL.SDL_Rect { x = 10, y = 10, w = 200, h = 20 };
        SDL.SDL_SetRenderDrawColor(_renderer, 255, 255, 255, 255);
        SDL.SDL_RenderDrawRect(_renderer, ref healthOutlineRect);
    }

    public void Dispose()
    {
        if (_renderer != IntPtr.Zero)
            SDL.SDL_DestroyRenderer(_renderer);
        if (_window != IntPtr.Zero)
            SDL.SDL_DestroyWindow(_window);
        SDL.SDL_Quit();
    }
}
