namespace DoomClone.Entity;

using DoomClone.Core;
using DoomClone.Utilities;
using System;

public abstract class Entity
{
    public Vector3 Position { get; set; }
    public int Health { get; set; }
    public float Speed { get; set; }
    public bool IsActive { get; set; }

    protected Entity(Vector3 position, int health, float speed)
    {
        Position = position;
        Health = health;
        Speed = speed;
        IsActive = true;
    }

    public abstract void Update(World world, float deltaTime);

    public virtual void TakeDamage(int damage)
    {
        Health = Math.Max(0, Health - damage);
    }

    public virtual void OnDeath()
    {
        IsActive = false;
    }
}
