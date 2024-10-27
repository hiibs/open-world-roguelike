namespace Roguelike;

using System.Collections.Generic;

internal class Bow : Entity
{
    public Bow()
    {
        Name = "Bow";
        Movable = true;
        ObscuresVision = false;
        IsWalkable = true;
    }
}