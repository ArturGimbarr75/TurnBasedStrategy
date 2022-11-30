using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static GridPosition;
using static UnityEngine.Mathf;

public class PathFinding : MonoBehaviour
{
    public static PathFinding Instance { get; private set; }

    [SerializeField] private LayerMask _obstaclesLayerMask;
    [SerializeField] private Transform _gridDebugObjectPrefab;
    [SerializeField] private bool _createDebugObjects;

    private int _width;
    private int _height;
    private float _cellSize;
<<<<<<< HEAD
    private GridSystemHex<PathNode> _gridSystem;

    private const int STRAIGHT_MOVE_COST = 10;
=======
    private GridSystem<PathNode> _gridSystem;

    private const int STRAIGHT_MOVE_COST = 10;
    private const int DIAGONAL_MOVE_COST = 14;
>>>>>>> master

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"There's more than one {typeof(PathFinding).FullName} " + transform + " - " + Instance);
            return;
        }
<<<<<<< HEAD
        Instance = this; 
=======
        Instance = this;

        
>>>>>>> master
    }

    public void Setup(int width, int height, float cellSize)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;

<<<<<<< HEAD
        _gridSystem = new GridSystemHex<PathNode>(_width, _height, (go, gp) => new(gp), _cellSize);
=======
        _gridSystem = new GridSystem<PathNode>(_width, _height, (go, gp) => new(gp), _cellSize);
>>>>>>> master
        if (_createDebugObjects)
        _gridSystem.CreateDebugObjects(_gridDebugObjectPrefab, transform);

        for (int x = 0; x < width; x++)
            for (int z = 0; z < height; z++)
            {
                GridPosition pos = new(x, z);
                Vector3 worldPos = _gridSystem.GetWorldPosition(pos);
                const float OFFSET_Y = 5f;
                GetNode(pos).IsWalkable = !Physics.Raycast(worldPos + Vector3.down * OFFSET_Y, Vector3.up, OFFSET_Y * 2, _obstaclesLayerMask);
            }
    }

    public bool TryFindPath(GridPosition startPos, GridPosition endPos, out List<GridPosition> path)
    {
        path = FindPath(startPos, endPos);
        return path != null;
    }

    public List<GridPosition> FindPath(GridPosition startPos, GridPosition endPos)
    {
        return FindPath(startPos, endPos, out _); 
    }

    public List<GridPosition> FindPath(GridPosition startPos, GridPosition endPos, out int pathLength)
    {
        List<PathNode> open = new();
        List<PathNode> closed = new();

        PathNode startNode = _gridSystem.GetGridObject(startPos);
        PathNode endNode = _gridSystem.GetGridObject(endPos);
        open.Add(startNode);

        (int width, int height) = _gridSystem.GetGridSize();
        for (int x = 0; x < width; x++)
            for (int z = 0; z < height; z++)
            {
                GridPosition pos = new GridPosition(x, z);
                PathNode node = _gridSystem.GetGridObject(pos);

                node.GCost = int.MaxValue;
                node.HCost = 0;
                node.ResetCameFromPathNode();
            }

        startNode.GCost = 0;
        startNode.HCost = startPos.Distance(endPos) * STRAIGHT_MOVE_COST;

        while (open.Count > 0)
        {
            int minF = open.Min(x => x.FCost);
            PathNode node = open.Where(x => x.FCost == minF).FirstOrDefault();

            if (node == endNode)
            {
                pathLength = (int)Ceil((float)endNode.FCost / STRAIGHT_MOVE_COST);
                return CalculatePath(endNode);
            }

            open.Remove(node);
            closed.Add(node);

            foreach (PathNode neighbour in GetNeigbours(node))
            {
                if (closed.Contains(neighbour))
                    continue;

                if (!neighbour.IsWalkable)
                {
                    closed.Add(neighbour);
                    continue;
                }

<<<<<<< HEAD
                int tentativeGCost = node.GCost + STRAIGHT_MOVE_COST;
=======
                int tentativeGCost = node.GCost
                    + node.GridPosition.CalculateDistanceCost(neighbour.GridPosition, STRAIGHT_MOVE_COST, DIAGONAL_MOVE_COST);
>>>>>>> master

                if (tentativeGCost < neighbour.GCost)
                {
                    neighbour.CameFromPathNode = node;
                    neighbour.GCost = tentativeGCost;
                    neighbour.HCost = neighbour.GridPosition.Distance(endPos);

                    if (!open.Contains(neighbour))
                        open.Add(neighbour);
                }
            }
        }
        
        pathLength = 0;
        return null;
    }

    public void SetWalkable(GridPosition gridPosition, bool walkable = true)
    {
        _gridSystem.GetGridObject(gridPosition).IsWalkable = walkable;
    }

    public bool IsWalkable(GridPosition gridPosition)
    {
        return _gridSystem.GetGridObject(gridPosition).IsWalkable;
    }

    public bool HasPath(GridPosition startPos, GridPosition endPos)
    {
        return TryFindPath(startPos, endPos, out _);
    }

    public int GetPathLength(GridPosition startPos, GridPosition endPos)
    {
        FindPath(startPos, endPos, out int length);
        return length;
    }

    private PathNode GetNode(GridPosition pos)
    {
        return _gridSystem.GetGridObject(pos);
    }

    private List<PathNode> GetNeigbours(PathNode node)
    {
        List<PathNode> neigbours = new();

        GridPosition pos = node.GridPosition;
<<<<<<< HEAD
        bool oddRow = pos.Z % 2 == 1;
        GridPosition[] neigboursPositions =
        {
            pos + UP,
            pos + RIGHT,
            pos + DOWN,
            pos + LEFT,
            pos + UP + (oddRow? RIGHT : LEFT),
            pos + DOWN + (oddRow? RIGHT : LEFT)
=======
        GridPosition[] neigboursPositions =
        {
            pos + UP,
            pos + UP + RIGHT,
            pos + RIGHT,
            pos + RIGHT + DOWN,
            pos + DOWN,
            pos + DOWN + LEFT,
            pos + LEFT,
            pos + LEFT + UP
>>>>>>> master
        };
        foreach (GridPosition p in neigboursPositions)
            if (_gridSystem.IsValidGridPosition(p))
                neigbours.Add(GetNode(p));

        return neigbours;
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new();
        path.Add(endNode);
        PathNode current = endNode;
        while (current.CameFromPathNode != null)
        {
            path.Add(current.CameFromPathNode);
            current = current.CameFromPathNode;
        }
        path.Reverse();
        return path.Select(x => x.GridPosition).ToList();
    }   
}
