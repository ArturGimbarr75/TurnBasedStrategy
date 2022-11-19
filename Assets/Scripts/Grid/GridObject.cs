using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : IFormattable
{
    private List<Unit> _units;
    private IInteractable _interactable;
    private GridSystem<GridObject> _gridSystem;
    private GridPosition _gridPosition;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        _gridSystem = gridSystem;
        _gridPosition = gridPosition;
        _units = new List<Unit>();
    }

    public override string ToString() => _gridPosition.ToString() + "\n" + string.Join(",\n", _units);

    public string ToString(string format, IFormatProvider formatProvider)
        => _gridPosition.ToString(format, formatProvider) + "\n" + string.Join(",\n", _units);

    public void AddUnit(Unit unit)
    {
        _units.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        _units.Remove(unit);
    }

    public bool TryGetUnit(out Unit unit)
    {
        unit = null;
        if (HasAnyUnit())
        {
            unit = GetUnitList()[0];
            return true;
        }
        return false;
    }

    public List<Unit> GetUnitList()
    {
        return _units;
    }

    public bool HasAnyUnit()
    {
        return _units.Count > 0;
    }

    public bool TryGetInteraction(out IInteractable interactable)
    {
        interactable = _interactable;
        return interactable != null;
    }

    public bool TryGetInteraction<T>(out T interactable) where T : IInteractable
    {
        if (_interactable is T inter)
        {
            interactable = inter;
            return true;
        }
        interactable = default;
        return false;
    }

    public void SetInteractable(IInteractable interactable)
    {
        _interactable = interactable;
    }
}
