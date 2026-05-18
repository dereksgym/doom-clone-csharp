namespace DoomClone.Input;

using DoomClone.Utilities;
using SDL2;
using System;
using System.Collections.Generic;

public class InputManager
{
    private Span<byte> _keyboardState;
    private int _mouseX;
    private int _mouseY;
    private int _prevMouseX;
    private int _prevMouseY;
    private bool _mouseGrabbed = false;

    public InputManager()
    {
        _keyboardState = new byte[SDL.SDL_NUM_SCANCODES];
        _mouseX = 0;
        _mouseY = 0;
        _prevMouseX = 0;
        _prevMouseY = 0;
    }

    public void Update()
    {
        _prevMouseX = _mouseX;
        _prevMouseY = _mouseY;

        SDL.SDL_PumpEvents();
        
        IntPtr keyboardStatePtr = SDL.SDL_GetKeyboardState(out _);
        System.Runtime.InteropServices.Marshal.Copy(keyboardStatePtr, _keyboardState.ToArray(), 0, SDL.SDL_NUM_SCANCODES);

        SDL.SDL_GetMouseState(out _mouseX, out _mouseY);

        // Handle events
        while (SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
        {
            switch (e.type)
            {
                case SDL.SDL_EventType.SDL_KEYDOWN:
                case SDL.SDL_EventType.SDL_KEYUP:
                case SDL.SDL_EventType.SDL_MOUSEMOTION:
                    // Handled by SDL_GetKeyboardState and SDL_GetMouseState
                    break;
            }
        }
    }

    public bool IsKeyPressed(SDL.SDL_Scancode key)
    {
        return _keyboardState[(int)key] != 0;
    }

    public Vector3 GetMouseDelta()
    {
        return new Vector3(
            _mouseX - _prevMouseX,
            0,
            _mouseY - _prevMouseY
        );
    }

    public (int x, int y) GetMousePosition() => (_mouseX, _mouseY);
}
