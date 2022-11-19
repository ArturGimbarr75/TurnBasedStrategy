using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using static UnityEngine.Mathf;

public class ShootAction : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;
    public static event EventHandler<OnShootEventArgs> OnAnyShoot;
    
    [field: SerializeField] public Transform ActionCameraPosition { get; private set; }
    [field: SerializeField] public Transform ShootStartPosition { get; private set; }
    [field: SerializeField, Min(1)] public int MaxShootDistance { get; private set; } = 7;

    [SerializeField] private LayerMask _obstaclesLayerMask;

    [Header("Params")]
    [SerializeField, Min(0)] private float _aimingDuration = 1;
    [SerializeField, Min(0)] private float _shootingDuration = 0.1f;
    [SerializeField, Min(0)] private float _cooloffDuration = 1;
    [SerializeField, Min(0)] private float _damage = 10;

    public override string ActionName => "Shoot";

    private State _state;
    private float _stateTimer;
    private bool _canShoot;
    private Unit _targetUnit;
    private Quaternion _startRotation, _targetRotation;

    private void Update()
    {
        if (!_isActive)
            return;

        _stateTimer -= Time.deltaTime;
        switch (_state)
        {
            case State.Aiming:
                transform.rotation = Quaternion.Lerp(_targetRotation, _startRotation, _stateTimer / _aimingDuration);
                break;

            case State.Shooting:
                if (_canShoot)
                {
                    Shoot();
                    _canShoot = false;
                }
                break;

            case State.Cooloff:
                break;
        }
        
        if (_stateTimer <= 0)
            NextState();
    }

    private void Shoot()
    {
        OnShoot?.Invoke(this, new() { TargetUnit = _targetUnit, ShootingUnit = Unit });
        OnAnyShoot?.Invoke(this, new() { TargetUnit = _targetUnit, ShootingUnit = Unit });
        _targetUnit.Hit(_damage, transform.position);
    }

    private void NextState()
    {
        switch (_state)
        {
            case State.Aiming:
                _state = State.Shooting;
                _stateTimer = _shootingDuration;
                break;

            case State.Shooting:
                _state = State.Cooloff;
                _stateTimer = _cooloffDuration;
                break;

            case State.Cooloff:
                ActionComplete();
                break;
        }
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {
        return GetValidActionGridPositions(Unit.GetGridPosition());
    }

    public List<GridPosition> GetValidActionGridPositions(GridPosition unitGridPosition)
    {
        List<GridPosition> gridPositions = new List<GridPosition>();
        for (int x = -MaxShootDistance; x < MaxShootDistance; x++)
            for (int z = -MaxShootDistance; z < MaxShootDistance; z++)
            {
                GridPosition offsetGridPos = new GridPosition(x, z);
                GridPosition testGridPos = unitGridPosition + offsetGridPos;
                int testDistance = Abs(x) + Abs(z);

                if (testDistance > MaxShootDistance
                    || !LevelGrid.Instance.IsValidGridPosition(testGridPos)
                    || !LevelGrid.Instance.TryGetUnitOnGridPosition(testGridPos, out Unit targetUnit)
                    || targetUnit.IsEnemy == Unit.IsEnemy
                    || !CanHitTarget(testGridPos))
                    continue;

                gridPositions.Add(testGridPos);
            }
        return gridPositions;
    }

    public override void TakeAction(GridPosition targetPosition, Action onActionComplete)
    {
        _canShoot = true;
        LevelGrid.Instance.TryGetUnitOnGridPosition(targetPosition, out Unit targetUnit);
        _targetUnit = targetUnit;
        _state = State.Aiming;
        _stateTimer = _aimingDuration;
        _startRotation = transform.rotation;
        _targetRotation = Quaternion.LookRotation(_targetUnit.transform.position - transform.position);
        base.TakeAction(targetPosition, onActionComplete);
    }

    public override EnemyAiAction GetEnemyAiAction(GridPosition gridPosition)
    {
        Unit target = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new()
        {
            GridPosition = gridPosition,
            ActionValue = RoundToInt(1 - target.GetLeftHealth() /HealthSystem.MAX_ALLOWED_HEALTH) * 100
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositions(gridPosition).Count;
    }

    private bool CanHitTarget(GridPosition target)
    {
#if UNITY_EDITOR
        Vector3 offset = Vector3.up * ShootStartPosition.position.y;
        Debug.DrawLine(target.GetWorldPosition() + offset, Unit.GetWorldPosition() + offset, Color.red, 10); 
#endif
        return !Physics.Raycast
        (
            ShootStartPosition.position,
            (target.GetWorldPosition() - Unit.GetWorldPosition()).normalized,
            Vector3.Distance(target.GetWorldPosition(), Unit.GetWorldPosition()),
            _obstaclesLayerMask
        );
    }

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff
    }
}
