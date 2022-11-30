using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static UnityEngine.Mathf;

public class GridSystemHex<TGridObject>
{
	private int _width;
	private int _height;
	private float _cellSize;
    private TGridObject[,] _gridObjects;

    private const float HEX_VERTIVAL_OFFSET_MULTIPLIER = 0.75f;
	
	public GridSystemHex(int width, int height, Func<GridSystemHex<TGridObject>, GridPosition, TGridObject> createGridObject, float cellSize = 1)
	{
		_width = width;
		_height = height;
		_cellSize = cellSize;
        _gridObjects = new TGridObject[width, height];

		for (int x = 0; x < _width; x++)
			for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                _gridObjects[x, z] = createGridObject(this, gridPosition);
            }
	}

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    { 
        return new Vector3(gridPosition.X, 0, 0) * _cellSize +
               new Vector3(0, 0, gridPosition.Z) * _cellSize * HEX_VERTIVAL_OFFSET_MULTIPLIER +
               (gridPosition.Z % 2 == 1
                    ? new Vector3(1, 0, 0) * _cellSize * 0.5f
                    : Vector3.zero);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        GridPosition roughXZ = new GridPosition
        (
            RoundToInt(worldPosition.x / _cellSize),
            RoundToInt(worldPosition.z / _cellSize / HEX_VERTIVAL_OFFSET_MULTIPLIER)
        );

        bool oddRow = roughXZ.Z % 2 == 1;
        List<GridPosition> neigbours = new List<GridPosition>()
        { 
            roughXZ + new GridPosition(-1, 0),
            roughXZ + new GridPosition(1, 0),
            roughXZ + new GridPosition(0, 1),
            roughXZ + new GridPosition(0, -1),
            roughXZ + new GridPosition(oddRow? 1 : -1, 1),
            roughXZ + new GridPosition(oddRow? 1 : -1, -1),
        };
        GridPosition closest = roughXZ;
        foreach (GridPosition neigbour in neigbours)
            if (Vector3.Distance(worldPosition, neigbour.GetWorldPosition()) <
                Vector3.Distance(worldPosition, closest.GetWorldPosition()))
                closest = neigbour;
        return closest;
    }

    public void CreateDebugObjects(Transform debugPrefab, Transform parrent = null)
    {
        for (int x = 0; x < _width; x++)
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform debugTransform =
                    UnityEngine.Object.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity, parrent);
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
    }

    public TGridObject GetGridObject(GridPosition gridPosition)
        => _gridObjects[gridPosition.X, gridPosition.Z];

    public bool IsValidGridPosition(GridPosition gridPosition)
        =>  gridPosition.X >= 0 &&
            gridPosition.Z >= 0 &&
            gridPosition.X < _width &&
            gridPosition.Z < _height;

    public (int width, int height) GetGridSize() => (_width, _height);
}
