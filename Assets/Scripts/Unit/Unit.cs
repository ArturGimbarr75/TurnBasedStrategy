using System;
using UnityEngine;

public class Unit : MonoBehaviour, IHitable
{
    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    public bool IsEnemy => _isEnemy;

    [SerializeField] private bool _isEnemy;
    [SerializeField, Min(0)] private int _maxActionPoints = 2;

    private MoveAction _moveAction;
    private SpinAction _spinAction;
    private BaseAction[] _actions;
    private HealthSystem _healthSystem;
    private GridPosition _gridPosition;
    private int _actionPoints = 2;

    private void Awake()
    {
        _moveAction = GetComponent<MoveAction>();
        _spinAction = GetComponent<SpinAction>();
        _actions = GetComponents<BaseAction>();
        _healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
        TurnSystem.Instance.OnTurnChanged += OnTurnChanged;
        _healthSystem.OnDead += OnDead;
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != _gridPosition)
        {
            GridPosition oldGridPosition = _gridPosition;
            _gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMoveGridPosition(oldGridPosition, newGridPosition, this);    
        }
    }

    private void OnDead(object sender, EventArgs args)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(_gridPosition, this);
        Destroy(gameObject);
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    private void OnTurnChanged(object sender, EventArgs args)
    {
        if (IsEnemy ^ !TurnSystem.Instance.IsPlayerTurn)
        {
            _actionPoints = _maxActionPoints;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SpendActionPoints(int amount)
    {
        _actionPoints -= amount;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void OnDisable()
    {
        TurnSystem.Instance.OnTurnChanged -= OnTurnChanged;
        _healthSystem.OnDead -= OnDead;
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction act in _actions)
            if (act is T a)
                return a;
        return default;
    }

    public bool TryGetAction<T>(out T action) where T : BaseAction
    {
        action = default;
        foreach (BaseAction act in _actions)
            if (act is T a)
            {
                action = a;
                return true;
            }
        return false;
    }

    public BaseAction[] GetActions() => _actions;

    public GridPosition GetGridPosition() => _gridPosition;
    
    public Vector3 GetWorldPosition() => LevelGrid.Instance.GetWorldPosition(_gridPosition);

    public int GetActionPoints() => _actionPoints;

    public bool CanSpendActionPointsToTakeAction(BaseAction action)
        => _actionPoints >= action.ActionPointsCost;

    public bool TrySpendActionPointsToTakeAction(BaseAction action)
    {
        if (!CanSpendActionPointsToTakeAction(action))
            return false;
        SpendActionPoints(action.ActionPointsCost);
        return true;
    }

    public void Hit(float damage, Vector3? hitSource = null)
    {
        _healthSystem.Hit(damage, hitSource);
    }

    public float GetLeftHealth()
    {
        return _healthSystem.Health;
    }
}
