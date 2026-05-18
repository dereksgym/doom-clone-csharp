namespace DoomClone.Core;

using DoomClone.Utilities;
using System.Collections.Generic;

public class World
{
    private char[,] _tiles;
    public List<Entity.Entity> Entities { get; private set; }

    public int Width { get; private set; }
    public int Height { get; private set; }

    public World(int width = Constants.WORLD_WIDTH, int height = Constants.WORLD_HEIGHT)
    {
        Width = width;
        Height = height;
        _tiles = new char[width, height];
        Entities = new List<Entity.Entity>();
        GenerateDefaultMap();
    }

    public void GenerateDefaultMap()
    {
        // Create border walls
        for (int x = 0; x < Width; x++)
        {
            _tiles[x, 0] = '#';
            _tiles[x, Height - 1] = '#';
        }
        for (int y = 0; y < Height; y++)
        {
            _tiles[0, y] = '#';
            _tiles[Width - 1, y] = '#';
        }

        // Fill interior with open space
        for (int x = 1; x < Width - 1; x++)
        {
            for (int y = 1; y < Height - 1; y++)
            {
                _tiles[x, y] = '.';
            }
        }

        // Add some interior walls to create interesting layout
        // Vertical walls
        for (int y = 3; y < 10; y++)
            _tiles[8, y] = '#';

        for (int y = 14; y < 21; y++)
            _tiles[16, y] = '#';

        // Horizontal walls
        for (int x = 10; x < 16; x++)
            _tiles[x, 8] = '#';

        for (int x = 5; x < 12; x++)
            _tiles[x, 15] = '#';

        // Add some open rooms
        for (int x = 12; x < 20; x++)
        {
            for (int y = 3; y < 8; y++)
            {
                _tiles[x, y] = '.';
            }
        }
    }

    public bool IsWall(float x, float y, float radius = 0.1f)
    {
        // Check multiple points around the position for radius collision
        var checkPoints = new[]
        {
            (x, y),
            (x + radius, y),
            (x - radius, y),
            (x, y + radius),
            (x, y - radius),
        };

        foreach (var (px, py) in checkPoints)
        {
            int gridX = (int)(px / Constants.TILE_SIZE);
            int gridY = (int)(py / Constants.TILE_SIZE);

            if (gridX < 0 || gridX >= Width || gridY < 0 || gridY >= Height)
                return true;  // Outside world is considered a wall

            if (_tiles[gridX, gridY] == '#')
                return true;
        }

        return false;
    }

    public char GetTile(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            return '#';  // Out of bounds is wall
        return _tiles[x, y];
    }

    public void SetTile(int x, int y, char value)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
            _tiles[x, y] = value;
    }

    public void Update(float deltaTime)
    {
        foreach (var entity in Entities)
        {
            entity.Update(this, deltaTime);
        }

        // Remove dead entities
        Entities.RemoveAll(e => e.Health <= 0);
    }

    public void AddEntity(Entity.Entity entity)
    {
        Entities.Add(entity);
    }

    public List<Entity.Entity> GetEntitiesInRadius(Vector3 center, float radius)
    {
        var result = new List<Entity.Entity>();
        foreach (var entity in Entities)
        {
            if (Vector3.Distance(entity.Position, center) <= radius)
                result.Add(entity);
        }
        return result;
    }
}
