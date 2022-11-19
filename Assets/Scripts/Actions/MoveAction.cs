using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static UnityEngine.Mathf;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    public override string ActionName => "Move";

    [SerializeField, Min(0)] private float _movementSpeed;
    [SerializeField, Range(0.01f, 1)] private float _rotationDuration;
    [SerializeField, Min(0)] private int _maxMoveDistance = 4;

    private List<Vector3> _pathToTarget;
    private int _currentPosIndex;
    private Vector3 _targetPos;
    private Quaternion _startRotation;
    private Quaternion _targetRotation;
    private float _rotationLerp = 1;

    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        if (!_isActive)
            return;

        if (transform.position == _targetPos && _rotationLerp > _rotationDuration)
        {
            _currentPosIndex++;            
            if (_currentPosIndex >= _pathToTarget.Count)
            {
                ActionComplete();
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                return; 
            }
            SetValues(_currentPosIndex);
        }

        Vector3 moveDirection = (_targetPos - transform.position).normalized;
        float translationDistance = Min(Time.deltaTime * _movementSpeed,
            Vector3.Distance(transform.position, _targetPos));
        transform.position += translationDistance * moveDirection;
        _rotationLerp += Time.deltaTime;
        transform.rotation = Quaternion.Lerp(_startRotation, _targetRotation, _rotationLerp / _rotationDuration);
    }

    public override void TakeAction(GridPosition targetPosition, Action onActionComplete = null)
    {
        _currentPosIndex = 1;
        _pathToTarget = PathFinding.Instance
            .FindPath(Unit.GetGridPosition(), targetPosition).ToListVector3();
        SetValues(_currentPosIndex);
        OnStartMoving?.Invoke(this, EventArgs.Empty);
        base.TakeAction(targetPosition, onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {
        List<GridPosition> gridPositions = new List<GridPosition>();
        GridPosition unitGridPosition = Unit.GetGridPosition();
        for (int x = -_maxMoveDistance; x < _maxMoveDistance; x++)
            for (int z = -_maxMoveDistance; z < _maxMoveDistance; z++)
            {
                GridPosition offsetGridPos = new GridPosition(x, z);
                GridPosition testGridPos = unitGridPosition + offsetGridPos;

                if (   !LevelGrid.Instance.IsValidGridPosition(testGridPos)
                    || LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPos)
                    || unitGridPosition == testGridPos
                    || !PathFinding.Instance.IsWalkable(testGridPos)
                    || !PathFinding.Instance.HasPath(unitGridPosition, testGridPos)
                    || PathFinding.Instance.GetPathLength(unitGridPosition, testGridPos) > _maxMoveDistance)
                    continue;

                gridPositions.Add(testGridPos);
            }
        return gridPositions;
    }

    public override EnemyAiAction GetEnemyAiAction(GridPosition gridPosition)
    {
        if (Unit.TryGetAction(out ShootAction shootAction))
        {
            const int VALUE_MULTIPLIER = 10;
            int targetCount = shootAction.GetTargetCountAtPosition(gridPosition);
            return new()
            {
                GridPosition = gridPosition,
                ActionValue = targetCount * VALUE_MULTIPLIER
            };
        }
        else
            return new()
            {
                GridPosition = gridPosition,
                ActionValue = 100
            };
    }

    private void SetValues(int index)
    {
        _targetPos = _pathToTarget[index];
        _startRotation = transform.rotation;
        _targetRotation = Quaternion.LookRotation(_pathToTarget[index] - transform.position);
        _rotationLerp = 0;
    }
}
