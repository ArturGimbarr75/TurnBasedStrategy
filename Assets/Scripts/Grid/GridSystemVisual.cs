using System;
using System.Collections.Generic;
using UnityEngine;
using LG = LevelGrid;

using static UnityEngine.Mathf;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private Transform _gridSystemVisualSinglePrefab;
    [SerializeField] private List<GridVisualTypeMaterial> _gridVisualTypeMaterials;

    public static GridSystemVisual Instance { get; private set; }

    private GridSystemVisualSingle[,] _gridVisuals;
    private GridSystemVisualSingle _lastSelectedGridSystemVisualSingle;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"There's more than one {typeof(LG).FullName} " + transform + " - " + Instance);
            return;
        }
        Instance = this;
    }

    private void Start()
    {     
        (int width, int height) = LG.Instance.GetGridSize();
        _gridVisuals = new GridSystemVisualSingle[width, height];
        for (int x = 0; x < width; x++)
            for (int z = 0; z < height; z++)
            {
                GridPosition position = new GridPosition(x, z);
                _gridVisuals[x, z] = Instantiate(_gridSystemVisualSinglePrefab,
                    LG.Instance.GetWorldPosition(position), Quaternion.identity)
                    .GetComponent<GridSystemVisualSingle>();
            }

        UnitActionSystem.Instance.OnSelectedActionChanged += UpdateGridVisual;
        LG.Instance.OnAnyUnitMoveGridPosition += UpdateGridVisual;
        UpdateGridVisual();
    }

    public void HideAllGridPositions()
    {
        (int width, int height) = LG.Instance.GetGridSize();
        for (int x = 0; x < width; x++)
            for (int z = 0; z < height; z++)
                _gridVisuals[x, z].Hiden = true;
    }

    public void ShowGridPositionList(List<GridPosition> positions, GridVisualType type)
    {
        foreach (GridPosition p in positions)
        {
            _gridVisuals[p.X, p.Z].Hiden = false;
            _gridVisuals[p.X, p.Z].SetMaterial(GetGridVisualType(type));
        }
    }

    private void ShowGridPositionSquare(GridPosition gridPosition, int range, GridVisualType type)
    {
        List<GridPosition> gridPositions = new List<GridPosition>();
        for (int x = -range; x <= range; x++)
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPos = gridPosition + new GridPosition(x, z);
                if (!LG.Instance.IsValidGridPosition(testGridPos))
                    continue;
                gridPositions.Add(testGridPos);
            }
        ShowGridPositionList(gridPositions, type);
    }

    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType type)
    {
        List<GridPosition> gridPositions = new List<GridPosition>();   
        for (int x = -range; x <= range; x++)
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPos = gridPosition + new GridPosition(x, z);
                int testDistance = Abs(x) + Abs(z);
                if (testDistance > range || !LG.Instance.IsValidGridPosition(testGridPos))
                    continue;
                gridPositions.Add(testGridPos);
            }
        ShowGridPositionList(gridPositions, type);
    }

    private void UpdateGridVisual(object sender = null, EventArgs args = null)
    {
        HideAllGridPositions();
        BaseAction selectedAction = UnitActionSystem.Instance.SelectedAction;
        Unit selectedUnit = UnitActionSystem.Instance.SelectedUnit;

        GridVisualType type = selectedAction switch 
        {
            MoveAction => GridVisualType.White,
            SpinAction => GridVisualType.Blue,
            ShootAction or SwordAction => GridVisualType.Red,
            GrenadeAction => GridVisualType.Yellow,
            InteractAction => GridVisualType.Blue,
            _ => GridVisualType.White
        };

        if (selectedAction is ShootAction shootAction)
            ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.MaxShootDistance, GridVisualType.RedSoft);
        if (selectedAction is SwordAction)
            ShowGridPositionSquare(selectedUnit.GetGridPosition(), SwordAction.MAX_HIT_DISTANCE, GridVisualType.RedSoft);

        ShowGridPositionList(selectedAction.GetValidActionGridPositions(), type);
    }

    private Material GetGridVisualType(GridVisualType type)
    {
        foreach (GridVisualTypeMaterial el in _gridVisualTypeMaterials)
            if (el.GridVisualType == type)
                return el.Material;

        Debug.LogError($"Could not find GridVisualTypeMaterial for GridVisualType {type}");
        return null;
    }

    private void OnDisable()
    {
        UnitActionSystem.Instance.OnSelectedActionChanged -= UpdateGridVisual;
        LG.Instance.OnAnyUnitMoveGridPosition -= UpdateGridVisual;
    }

    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType GridVisualType;
        public Material Material;
    }

    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        RedSoft,
        Yellow
    }
}
