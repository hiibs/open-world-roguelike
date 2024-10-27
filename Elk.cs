namespace Roguelike;

using Godot;
using System;
using System.Collections.Generic;

internal partial class NPCController : Node
{
    private Grid _grid;
    private List<NPC> _npcs = new();

    public override void _Ready()
    {
        _grid = GetNode<Grid>("Grid");
    }

    public void MoveNPCs()
    {
        var random = new RandomNumberGenerator();

        // wander into random direction
        foreach (NPC npc in _npcs)
        {
            // try to find suitable direction randomly for 8 times max
            for (int i = 0; i < 16; i++)
            {
                var x = random.RandiRange(-1, 1);
                var y = random.RandiRange(-1, 1);

                if (_grid.TryMoveEntity(npc, new Vector2I(x, y)))
                {
                    break;
                }
            }
        }
    }

    public void RegisterNPC(NPC npc)
    {
        _npcs.Add(npc);
    }
}
