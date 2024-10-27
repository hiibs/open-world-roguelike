namespace Roguelike;

using Godot;
using System;
using System.Collections.Generic;

internal class Vision
{
    private void CastShadow(Grid grid, Vector2I observerPosition)
    {
        grid.ClearLayer(grid.ShadowLayer);

        for (var x = 0; x < 64; x++)
        {
            for (var y = 0; y < 64; y++)
            {
                var cellCoord = observerPosition + new Vector2I(-32, -32) + new Vector2I(x, y);

                grid.SetCell(grid.ShadowLayer, cellCoord, 0, new Vector2I(3, 3));
            }
        }
    }

    public void CastVision(Grid grid, Vector2I observerPosition)
    {
        CastShadow(grid, observerPosition);

        var raysCount = 512;
        var rayDirection = Vector2.Right;

        for (var i = 0; i < raysCount; i++)
        {
            rayDirection = rayDirection.Rotated(Mathf.DegToRad(360f / raysCount));
            var visible = CastVisible(grid, observerPosition + new Vector2(0.5f, 0.5f), observerPosition + new Vector2(0.5f, 0.5f) + rayDirection * 16f);
            foreach (var cell in visible)
            {
                grid.EraseCell(grid.ShadowLayer, cell);

                if (grid.TryGetEntities(cell) is not List<Entity>)
                {
                    continue;
                }

                grid.UpdateCell(cell);
            }
        }
    }

    private List<Vector2I> CastVisible(Grid grid, Vector2 start, Vector2 end)
    {
        var visible = new List<Vector2I>();

        var stepLength = 0.1f;

        var stepsDone = 0;
        var maxSteps = 100;

        Vector2 rayPosition = start;

        while ((end - rayPosition).Length() >= stepLength && stepsDone < maxSteps)
        {
            rayPosition += (end - start).Normalized() * stepLength;

            var rayCellCoords = (Vector2I)rayPosition.Floor();

            visible.Add(rayCellCoords);

            if (grid.TryGetEntities(rayCellCoords) is List<Entity> entities)
            {
                foreach (var entity in entities)
                {
                    if (entity.ObscuresVision)
                    {
                        return visible;
                    }
                }
            }

            stepsDone++;
        }

        return visible;
    }
}
