using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Mathf;

public static class GridPositionExtentions
{
<<<<<<< HEAD
    public static int CalculateHeuristicDistanceCost(this GridPosition grid, GridPosition other, int straightCost)
    {
        return RoundToInt(straightCost * Vector3.Distance(grid.GetWorldPosition(), other.GetWorldPosition()));
=======
    public static int CalculateDistanceCost(this GridPosition grid, GridPosition other, int straightCost, int diagonalCost)
    {
        GridPosition distance = grid - other;

        int xDistance = Abs(distance.X);
        int zDistance = Abs(distance.Z);
        int remaining = Abs(xDistance - zDistance);

        return diagonalCost * Min(xDistance, zDistance) + straightCost * remaining;
>>>>>>> master
    }

    public static List<Vector3> ToListVector3(this IEnumerable<GridPosition> positions)
    {
        List<Vector3> list = new(positions.Count());
<<<<<<< HEAD
        foreach (GridPosition position in positions)
            list.Add(position.ToWorldPosition());
        return list;
    }

    public static Vector3 ToWorldPosition(this GridPosition position)
    {
        return LevelGrid.Instance.GetWorldPosition(position);
    }

    public static GridPosition ToGridPosition(this Vector3 position)
    {
        return LevelGrid.Instance.GetGridPosition(position);
    }
=======
        Func<GridPosition, Vector3> convert = LevelGrid.Instance.GetWorldPosition;
        foreach (GridPosition position in positions)
            list.Add(convert(position));
        return list;
    }
>>>>>>> master
}
