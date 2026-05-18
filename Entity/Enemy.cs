namespace DoomClone.Entity;

using DoomClone.Core;
using DoomClone.Utilities;
using System;

public class Enemy : Entity
{
    private float _wanderTimer = 0;
    private Vector3 _wanderDirection = new Vector3(1, 0, 0);
    private const float WANDER_CHANGE_INTERVAL = 3f;

    public Enemy(Vector3 position) 
        : base(position, 30, Constants.ENEMY_SPEED)
    {
    }

    public override void Update(World world, float deltaTime)
    {
        if (!IsActive) return;

        _wanderTimer -= deltaTime;
        if (_wanderTimer <= 0)
        {
            // Choose new wander direction
            float angle = (float)new Random().NextDouble() * MathHelper.TAU;
            _wanderDirection = new Vector3(MathF.Cos(angle), 0, MathF.Sin(angle));
            _wanderTimer = WANDER_CHANGE_INTERVAL;
        }

        // Try to move in wander direction
        var newPos = Position + _wanderDirection * Speed * deltaTime;
        Physics.TryMove(world, Position, newPos, 0.2f, out var finalPos);
        Position = finalPos;
    }

    public override void OnDeath()
    {
        base.OnDeath();
    }
}
