namespace DoomClone.Entity;

using DoomClone.Core;
using DoomClone.Utilities;
using System;

public class Item : Entity
{
    public enum ItemType
    {
        HealthPack,
        Ammo,
        Weapon
    }

    public ItemType Type { get; set; }
    private float _bobAmount = 0;
    private float _bobSpeed = 2.0f;

    public Item(Vector3 position, ItemType type) 
        : base(position, 1, 0)  // Items don't move
    {
        Type = type;
    }

    public override void Update(World world, float deltaTime)
    {
        // Bob up and down
        _bobAmount += _bobSpeed * deltaTime;
        Position = new Vector3(Position.X, MathF.Sin(_bobAmount) * 0.3f, Position.Z);
    }
}
