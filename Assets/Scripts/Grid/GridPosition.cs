using System;
using UnityEngine;
using static UnityEngine.Mathf;

public struct GridPosition : IFormattable, IEquatable<GridPosition>
{
    public int X;
    public int Z;

    public static readonly GridPosition UP = new(0, 1);
    public static readonly GridPosition DOWN = new(0, -1);
    public static readonly GridPosition RIGHT = new(1, 0);
    public static readonly GridPosition LEFT = new(-1, 0);

    public GridPosition(int x = 0, int z = 0)
    {
        X = x;
        Z = z;
    }

    public int Distance()
    {
        return Abs(X) + Abs(Z);
    }

    public int Distance(GridPosition other)
    {
        GridPosition c = this - other;
        return Abs(c.X) + Abs(c.Z);
    }

    public Vector3 GetWorldPosition()
    {
        return LevelGrid.Instance.GetWorldPosition(this);
    }

    public override bool Equals(object obj)
        => obj is GridPosition gridPosition && Equals(gridPosition);

    public bool Equals(GridPosition other) => X == other.X && Z == other.Z;

    public override int GetHashCode() => (X << 15) ^ Z;

    public override string ToString() => $"(X: {X}; Z: {Z})";

    public string ToString(string format, IFormatProvider formatProvider)
        => format switch
        {
            "N" => $"{X} : {Z}",
            "2LN" => $"{X}\n{Z}",
            "2L" => $"X: {X}\n Z: {Z}",
            _ => ToString()
        };

    public static bool operator == (GridPosition first, GridPosition second)
        => first.Equals(second);

    public static bool operator != (GridPosition first, GridPosition second)
        => !first.Equals(second);

    public static GridPosition operator +(GridPosition first, GridPosition second)
        => new GridPosition(first.X + second.X, first.Z + second.Z);

    public static GridPosition operator -(GridPosition first, GridPosition second)
        => new GridPosition(first.X - second.X, first.Z - second.Z);
}