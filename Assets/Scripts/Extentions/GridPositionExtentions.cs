using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Mathf;

public static class GridPositionExtentions
{
    public static int CalculateHeuristicDistanceCost(this GridPosition grid, GridPosition other, int straightCost)
    {
        return RoundToInt(straightCost * Vector3.Distance(grid.GetWorldPosition(), other.GetWorldPosition()));
    }

    public static List<Vector3> ToListVector3(this IEnumerable<GridPosition> positions)
    {
        List<Vector3> list = new(positions.Count());
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
}
