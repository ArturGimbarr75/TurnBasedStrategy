using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static UnityEngine.Mathf;

public class GridSystem<TGridObject>
{
	private int _width;
	private int _height;
	private float _cellSize;
    private TGridObject[,] _gridObjects;
	
	public GridSystem(int width, int height, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject, float cellSize = 1)
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
        => new Vector3(gridPosition.X, 0, gridPosition.Z) * _cellSize;
	
	public GridPosition GetGridPosition(Vector3 worldPosition) => new GridPosition
		(
			RoundToInt(worldPosition.x / _cellSize),
            RoundToInt(worldPosition.z / _cellSize)
		);

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
