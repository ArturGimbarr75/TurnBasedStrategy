using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : IFormattable
{
    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost => GCost + HCost;
    public GridPosition GridPosition { get; private set; }

    public PathNode CameFromPathNode { get; set; }

    public bool IsWalkable { get; set; } = true;

    public PathNode(GridPosition gridPosition)
    {
        GridPosition = gridPosition;
    }

    public void ResetCameFromPathNode()
    {
        CameFromPathNode = null;
    }

    public override string ToString()
    {
        return GridPosition.ToString();
    }

    public string ToString(string format, IFormatProvider formatProvider)
    { 
        return GridPosition.ToString(format, formatProvider);
    }
}
