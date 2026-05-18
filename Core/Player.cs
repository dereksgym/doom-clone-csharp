namespace DoomClone.Core;

using DoomClone.Input;
using DoomClone.Utilities;
using System;

public class Player
{
    public Vector3 Position { get; set; }
    public float Angle { get; set; }  // In degrees
    public float Pitch { get; set; }  // In degrees
    public int Health { get; set; }
    public int Ammo { get; set; }
    public float TotalTimeAlive { get; set; }

    public Vector3 Direction => new(
        MathF.Cos(MathHelper.ToRadians(Angle)),
        0,
        MathF.Sin(MathHelper.ToRadians(Angle))
    );

    public Player(Vector3 startPosition)
    {
        Position = startPosition;
        Angle = 0;
        Pitch = 0;
        Health = 100;
        Ammo = 50;
        TotalTimeAlive = 0;
    }

    public void Update(InputManager input, World world, float deltaTime)
    {
        TotalTimeAlive += deltaTime;

        // Handle rotation
        if (input.IsKeyPressed(SDL2.SDL.SDL_Scancode.SDL_SCANCODE_LEFT))
            Angle += Constants.PLAYER_TURN_SPEED * deltaTime;
        if (input.IsKeyPressed(SDL2.SDL.SDL_Scancode.SDL_SCANCODE_RIGHT))
            Angle -= Constants.PLAYER_TURN_SPEED * deltaTime;

        // Handle mouse look
        var mouseDelta = input.GetMouseDelta();
        if (mouseDelta != Vector3.Zero)
        {
            Angle -= mouseDelta.X * 0.5f;
            Pitch -= mouseDelta.Y * 0.5f;
            Pitch = MathHelper.Clamp(Pitch, -90, 90);
        }

        // Normalize angle
        while (Angle >= 360) Angle -= 360;
        while (Angle < 0) Angle += 360;

        // Handle movement
        var moveDir = Vector3.Zero;
        if (input.IsKeyPressed(SDL2.SDL.SDL_Scancode.SDL_SCANCODE_W))
            moveDir += Direction;
        if (input.IsKeyPressed(SDL2.SDL.SDL_Scancode.SDL_SCANCODE_S))
            moveDir -= Direction;

        // Strafe
        var rightDir = new Vector3(
            MathF.Cos(MathHelper.ToRadians(Angle - 90)),
            0,
            MathF.Sin(MathHelper.ToRadians(Angle - 90))
        );

        if (input.IsKeyPressed(SDL2.SDL.SDL_Scancode.SDL_SCANCODE_A))
            moveDir -= rightDir;
        if (input.IsKeyPressed(SDL2.SDL.SDL_Scancode.SDL_SCANCODE_D))
            moveDir += rightDir;

        // Apply movement
        if (moveDir.LengthSquared > 0)
        {
            moveDir = moveDir.Normalized;
            var newPos = Position + moveDir * Constants.PLAYER_SPEED * deltaTime;
            Physics.TryMove(world, Position, newPos, Constants.PLAYER_RADIUS, out var finalPos);
            Position = finalPos;
        }

        // Clamp position to world bounds
        Position = new Vector3(
            MathHelper.Clamp(Position.X, Constants.PLAYER_RADIUS, Constants.WORLD_WIDTH - Constants.PLAYER_RADIUS),
            Position.Y,
            MathHelper.Clamp(Position.Z, Constants.PLAYER_RADIUS, Constants.WORLD_HEIGHT - Constants.PLAYER_RADIUS)
        );
    }

    public void TakeDamage(int damage)
    {
        Health = Math.Max(0, Health - damage);
    }

    public void HealDamage(int amount)
    {
        Health = Math.Min(100, Health + amount);
    }
}
