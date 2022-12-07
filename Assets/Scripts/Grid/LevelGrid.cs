using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    public event EventHandler OnAnyUnitMoveGridPosition;

    [field: SerializeField, Min(1)] public int Width { get; private set; }
    [field: SerializeField, Min(1)] public int Height { get; private set; }
    [field: SerializeField, Min(0.1f)] public float CellSize { get; private set; }

    [Header("Debug")]
    [SerializeField] private Transform _textCellPrefab;

    private GridSystemHex<GridObject> _gridSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"There's more than one {typeof(LevelGrid).FullName} " + transform + " - " + Instance);
            return;
        }
        Instance = this;
        _gridSystem = new GridSystemHex<GridObject>(Width, Height, (go, gp) => new(go, gp), CellSize);
        //_gridSystem.CreateDebugObjects(_textCellPrefab, transform);
    }

    private void Start()
    {
        PathFinding.Instance.Setup(Width, Height, CellSize);
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
        => _gridSystem.GetGridObject(gridPosition).AddUnit(unit);

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
        => _gridSystem?.GetGridObject(gridPosition).GetUnitList();

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
        => GetUnitListAtGridPosition(gridPosition).FirstOrDefault();

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
        => _gridSystem.GetGridObject(gridPosition).RemoveUnit(unit);

    public GridPosition GetGridPosition(Vector3 worldPosition)
        => _gridSystem.GetGridPosition(worldPosition);
    
    public Vector3 GetWorldPosition(GridPosition gridPosition)
        => _gridSystem.GetWorldPosition(gridPosition);

    public (int width, int height) GetGridSize() => _gridSystem.GetGridSize();

    public void UnitMoveGridPosition(GridPosition from, GridPosition to, Unit unit)
    {
        RemoveUnitAtGridPosition(from, unit);
        AddUnitAtGridPosition(to, unit);
        OnAnyUnitMoveGridPosition?.Invoke(this, EventArgs.Empty);
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
        => _gridSystem.IsValidGridPosition(gridPosition);

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
        => _gridSystem.GetGridObject(gridPosition).HasAnyUnit();

    public bool TryGetUnitOnGridPosition(GridPosition gridPosition, out Unit unit)
        => _gridSystem.GetGridObject(gridPosition).TryGetUnit(out unit);

    public bool TryGetDoorAtGridPosition(GridPosition gridPosition, out IInteractable interactable)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.TryGetInteraction(out interactable);
    }

    public bool TryGetDoorAtGridPosition<T>(GridPosition gridPosition, out T interactable) where T : IInteractable
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.TryGetInteraction(out interactable);
    }

    public void SetInteractable(GridPosition gridPosition, IInteractable interactable)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.SetInteractable(interactable);
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 offset = new Vector3(0.5f, 0, 0.5f) * CellSize;
        Vector3 zone = new Vector3(Width, 0, Height * GridSystemHex<GridObject>.HEX_VERTIVAL_OFFSET_MULTIPLIER) * CellSize;
        zone += new Vector3(1.25f, 0, 0.75f) * GridSystemHex<GridObject>.HEX_VERTIVAL_OFFSET_MULTIPLIER;
        Gizmos.DrawWireCube(zone / 2 - offset, zone);
    }

#endif
}
