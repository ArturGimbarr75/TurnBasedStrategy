using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    public List<Unit> Units => new(_units);
    public List<Unit> FriendlyUnits => new(_friendlyUnits);
    public List<Unit> EnemyUnits => new(_enemyUnits);

    private List<Unit> _units;
    private List<Unit> _friendlyUnits;
    private List<Unit> _enemyUnits;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"There's more than one {typeof(UnitManager).FullName} " + transform + " - " + Instance);
            return;
        }
        Instance = this;

        _units = new List<Unit>();
        _friendlyUnits = new List<Unit>();
        _enemyUnits = new List<Unit>();
    }

    private void Start()
    {
        Unit.OnAnyUnitSpawned += OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += OnAnyUnitDead;
    }

    private void OnAnyUnitSpawned(object sender, EventArgs args)
    {
        Unit unit = sender as Unit;

        _units.Add(unit);
        if (unit.IsEnemy)
            _enemyUnits.Add(unit);
        else
            _friendlyUnits.Add(unit);
    }

    private void OnAnyUnitDead(object sender, EventArgs args)
    {
        Unit unit = sender as Unit;

        _units.Remove(unit);
        if (unit.IsEnemy)
            _enemyUnits.Remove(unit);
        else
            _friendlyUnits.Remove(unit);
    }
}
