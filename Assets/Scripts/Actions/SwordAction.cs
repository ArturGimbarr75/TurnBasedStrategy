using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwordAction : BaseAction
{
    public static event EventHandler OnHit;
    
    public event EventHandler OnActionStarted;

    public override string ActionName => "Sword";

    [SerializeField, Min(0.01f)] private float _beforeHitDuration = 0.7f;
    [SerializeField, Min(0.01f)] private float _afterHitDuration = 0.5f;
    [SerializeField, Min(0)] private float _damage = 100;

    private State _state;
    private float _stateTimer;
    private Unit _targetUnit;
    private Quaternion _startRotation, _targetRotation;

    public const int MAX_HIT_DISTANCE = 1;

    private void Update()
    {
        if (!_isActive)
            return;

        _stateTimer -= Time.deltaTime;
        switch (_state)
        {
            case State.SwingingSwordBeforeHit:
                transform.rotation = Quaternion.Lerp(_targetRotation, _startRotation, _stateTimer / _beforeHitDuration);
                break;

            case State.SwingingSwordAfterHit:
                break;
        }

        if (_stateTimer <= 0)
            NextState();
    }

    private void NextState()
    {
        switch (_state)
        {
            case State.SwingingSwordBeforeHit:
                _state = State.SwingingSwordAfterHit;
                _stateTimer = _afterHitDuration;
                _targetUnit.Hit(_damage, transform.position);
                OnHit?.Invoke(this, EventArgs.Empty);
                break;

            case State.SwingingSwordAfterHit:
                ActionComplete();
                break;
        }
    }

    public override EnemyAiAction GetEnemyAiAction(GridPosition gridPosition)
    {
        return new EnemyAiAction
        {
            GridPosition = gridPosition,
            ActionValue = 200
        };
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {
        List<GridPosition> gridPositions = new List<GridPosition>();
        GridPosition unitGridPosition = Unit.GetGridPosition();
        for (int x = -MAX_HIT_DISTANCE; x <= MAX_HIT_DISTANCE; x++)
            for (int z = -MAX_HIT_DISTANCE; z <= MAX_HIT_DISTANCE; z++)
            {
                GridPosition offsetGridPos = new GridPosition(x, z);
                GridPosition testGridPos = unitGridPosition + offsetGridPos;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPos)
                    || !LevelGrid.Instance.TryGetUnitOnGridPosition(testGridPos, out Unit targetUnit)
                    || targetUnit.IsEnemy == Unit.IsEnemy)
                    continue;

                gridPositions.Add(testGridPos);
            }
        return gridPositions;
    }

    public override void TakeAction(GridPosition targetPosition, Action onActionComplete)
    {
        LevelGrid.Instance.TryGetUnitOnGridPosition(targetPosition, out Unit targetUnit);
        _targetUnit = targetUnit;
        _state = State.SwingingSwordBeforeHit;
        _stateTimer = _beforeHitDuration;
        _startRotation = transform.rotation;
        _targetRotation = Quaternion.LookRotation(_targetUnit.transform.position - transform.position);
        base.TakeAction(targetPosition, onActionComplete);
        OnActionStarted?.Invoke(this, EventArgs.Empty);
    }

    private enum State
    {
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit
    }
}
