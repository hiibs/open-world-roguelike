namespace Roguelike;

using Godot;

public class Entity
{
    public string Name;
    public Vector2I AtlasCoords;
    public Vector2I Position;
    public bool Movable = true;
    public bool IsWalkable = true;
    public bool ObscuresVision = false;
}