using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Mathf;

public static class GridPositionExtentions
{
    public static int CalculateDistanceCost(this GridPosition grid, GridPosition other, int straightCost, int diagonalCost)
    {
        GridPosition distance = grid - other;

        int xDistance = Abs(distance.X);
        int zDistance = Abs(distance.Z);
        int remaining = Abs(xDistance - zDistance);

        return diagonalCost * Min(xDistance, zDistance) + straightCost * remaining;
    }

    public static List<Vector3> ToListVector3(this IEnumerable<GridPosition> positions)
    {
        List<Vector3> list = new(positions.Count());
        Func<GridPosition, Vector3> convert = LevelGrid.Instance.GetWorldPosition;
        foreach (GridPosition position in positions)
            list.Add(convert(position));
        return list;
    }
}
