namespace Roguelike;

using Godot;
using System;

internal partial class PlayerController : Node
{
    private const float _cameraFollowSpeed = 6;

    private Vector2I _lookDirection;

    private Grid _grid;
    private Character _player;
    private Camera2D _camera;

    public override void _Ready()
    {
        base._Ready();

        _grid = GetNode<Grid>("Grid");
        _camera = GetNode<Camera2D>("Camera");

        _player = new Character();

        _player.AddSkill(new Carpentry());

        _player.AtlasCoords = new Vector2I(0, 0);

        _grid.InstantiateEntity(_player, new Vector2I(1, 1));
        _grid.Observer = _player;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Input.IsActionPressed("LookModifier"))
        {
            PollLookActions();
        }
        else
        {
            PollMoveActions();
        }

        _camera.Position = _camera.Position.Lerp(_player.Position * 8, Mathf.Clamp((float)delta * _cameraFollowSpeed, 0f, 1f));
    }

    private void PollMoveActions()
    {
        if (Input.IsActionJustPressed("MoveUp")) // Moving
        {
            TryMove(Vector2I.Up);
        }
        else if (Input.IsActionJustPressed("MoveDown"))
        {
            TryMove(Vector2I.Down);
        }
        else if (Input.IsActionJustPressed("MoveLeft"))
        {
            TryMove(Vector2I.Left);
        }
        else if (Input.IsActionJustPressed("MoveRight"))
        {
            TryMove(Vector2I.Right);
        }
    }

    private void PollLookActions()
    {
        if (Input.IsActionJustPressed("MoveUp")) // Looking
        {
            Look(Vector2I.Up);
        }
        else if (Input.IsActionJustPressed("MoveDown"))
        {
            Look(Vector2I.Down);
        }
        else if (Input.IsActionJustPressed("MoveLeft"))
        {
            Look(Vector2I.Left);
        }
        else if (Input.IsActionJustPressed("MoveRight"))
        {
            Look(Vector2I.Right);
        }
    }

    private bool TryMove(Vector2I delta)
    {
        var vision = new Vision();

        if (!_grid.TryMoveEntity(_player, delta))
        {
            return false;
        }

        Look(_lookDirection);

        vision.CastVision(_grid, _player.Position);

        return true;
    }

    private void Look(Vector2I delta)
    {
        _lookDirection = delta;
        _grid.Select(_player.Position + delta);
    }

    
}