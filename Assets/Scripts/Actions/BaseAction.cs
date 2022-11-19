using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionComplited;

    [field: SerializeField] public int ActionPointsCost { get; private set; }

    public abstract string ActionName { get; }
    public Unit Unit { get; private set; }

    protected bool _isActive;
    protected Action _onActionComplete;

    protected virtual void Awake()
    {
        Unit = GetComponent<Unit>();
    }

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
        => GetValidActionGridPositions().Contains(gridPosition);

    public virtual void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        _onActionComplete = onActionComplete;
        _isActive = true;
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    public abstract List<GridPosition> GetValidActionGridPositions();

    protected void ActionComplete()
    {
        _isActive = false;
        _onActionComplete?.Invoke();
        OnAnyActionComplited?.Invoke(this, EventArgs.Empty);
    }

    public EnemyAiAction GetBestEnemyAiAction()
    {
        List<EnemyAiAction> actions = new List<EnemyAiAction>();
        List<GridPosition> validActionGridPositions = GetValidActionGridPositions();

        foreach (GridPosition gridPosition in validActionGridPositions)
        {
            EnemyAiAction action = GetEnemyAiAction(gridPosition);
            actions.Add(action);
        }

        actions.Sort((a, b) => b.ActionValue - a.ActionValue);
        return actions.FirstOrDefault();
    }

    public abstract EnemyAiAction GetEnemyAiAction(GridPosition gridPosition);
}
