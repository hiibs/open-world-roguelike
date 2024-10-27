namespace Roguelike;

using Godot;
using System;
using System.Collections.Generic;

public partial class Grid : TileMap
{
    public Entity Observer;
    public int ShadowLayer => 8;

    private const int _layerCount = 10;
    private const int _maxCellItems = 8;
    private const int _overlayLayer = 9;

    private Vector2I selectedCell;

    private readonly Vector2I _wallAtlasCoords = new Vector2I(0, 1);

    private Dictionary<Vector2I, List<Entity>> _entities = new();

    public override void _Ready()
    {
        base._Ready();

        GD.Randomize(); // TODO: add option to use seed

        Clear();

        for (var i = 0; i < _layerCount; i++)
        {
            AddLayer(i);
            SetLayerEnabled(i, true);
        } 
        
        GenerateWorld();
    }

    public void GenerateWorld()
    {
        //GenerateRoom(new Vector2I(0, 0), 12, 8);

        //GenerateRoom(new Vector2I(3, 3), 6, 2);

        GenerateTrees(new Vector2I(-64, -64), new Vector2I(64, 64));
    }

    public bool TryMoveEntity(Entity entity, Vector2I delta)
    {
        if (!IsCellWalkable(entity.Position + delta))
        {
            GD.Print("Cell is not walkable");
            return false;
        }
 
        if (!_entities[entity.Position].Remove(entity))
        {
            GD.Print("Entity you're trying to move doesnt exist on the grid!");
            return false;
        }

        entity.Position += delta;

        Add(entity);
        
        return true;
    }

    private bool IsCellWalkable(Vector2I cell)
    {
        if (_entities.ContainsKey(cell))
        {
            foreach (var otherEntity in _entities[cell])
            {
                if (!otherEntity.IsWalkable)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public Entity InstantiateEntity(Entity entity, Vector2I spawnPosition)
	{
        entity.Position = spawnPosition;

        Add(entity);

        return entity;
	}

    public bool Add(Entity entity)
    {
        if (_entities.ContainsKey(entity.Position))
        {
            if (_entities[entity.Position].Count >= _maxCellItems)
            {
                return false;
            }

            _entities[entity.Position].Add(entity);
        }
        else
        {
            _entities.Add(entity.Position, new List<Entity>());
            _entities[entity.Position].Add(entity);
        }

        return true;
    }

    public void UpdateCell(Vector2I position)
    {
        for (var i = 0; i < _maxCellItems; i++)
        {
            EraseCell(i, position);
        }

        if (!_entities.ContainsKey(position))
        {
            return;
        }

        for (var i = 0; i < _entities[position].Count; i++)
        {
            SetCell(i, position, 0, _entities[position][i].AtlasCoords);
        }
    }

    public void Select(Vector2I cellPosition)
    {
        EraseCell(_overlayLayer, selectedCell);
        selectedCell = cellPosition;
        SetCell(_overlayLayer, cellPosition, 0, new Vector2I(0, 3));
    }

    private void GenerateRoom(Vector2I topLeft, int width, int height)
	{
        var wall = new Entity();

        wall.AtlasCoords = _wallAtlasCoords;
        wall.IsWalkable = false;
        wall.ObscuresVision = true;

        for (var x = 0; x < width; x++)
        {
            InstantiateEntity(wall, topLeft + new Vector2I(x, 0)); // Bottom wall
            InstantiateEntity(wall, topLeft + new Vector2I(x, height - 1)); // Top wall
        }
        
        for (var y = 1; y < height - 1; y++)
        {
            InstantiateEntity(wall, topLeft + new Vector2I(0, y)); // Left wall
            InstantiateEntity(wall, topLeft + new Vector2I(width - 1, y)); // Left wall
        }
    }

    

    private void GenerateTrees(Vector2I upperLeft, Vector2I lowerRight)
    {

        var width = lowerRight.X - upperLeft.X;
        var height = lowerRight.Y - upperLeft.X;
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var random = new RandomNumberGenerator();

                if (random.RandfRange(0f, 1f) > 0.2f)
                {
                    continue;
                }

                var treePosition = new Vector2I(x, y);

                if (!IsCellWalkable(treePosition))
                {
                    continue;
                }

                var tree = new Entity();
                tree.ObscuresVision = true;
                tree.IsWalkable = false;
                tree.AtlasCoords = new Vector2I(1, 1);
                InstantiateEntity(tree, treePosition);
            }
        }
    }

    /// <summary>
    /// Returns null if cell is empty
    /// </summary>
    internal List<Entity> TryGetEntities(Vector2I rayCellCoords)
    {
       if (!_entities.ContainsKey(rayCellCoords))
       {
           return null;
       }

        return _entities[rayCellCoords];
    }
}
