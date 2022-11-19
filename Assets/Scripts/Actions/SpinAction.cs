using System;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    public override string ActionName => "Spin";

    [SerializeField, Range(0.1f, 3)] private float _duration;
    private Vector3 _beginRot;
    private Vector3 _endRot;
    private float _lerpValue;

    private void Update()
    {
        if (!_isActive)
            return;

        _lerpValue += Time.deltaTime;
        transform.rotation = Quaternion.Euler(Vector3.Slerp(_beginRot, _endRot, _lerpValue / _duration));

        if (_lerpValue > _duration)
        {
            ActionComplete();
            _onActionComplete = null;
        }
    }

    public override void TakeAction(GridPosition targetPosition, Action onActionComplete = null)
    {
        _beginRot = transform.rotation.eulerAngles;
        _endRot = _beginRot;
        _endRot.y += 360;
        _lerpValue = 0;
        base.TakeAction(targetPosition, onActionComplete);
    }

    public override EnemyAiAction GetEnemyAiAction(GridPosition gridPosition)
    {
        return new()
        {
            GridPosition = gridPosition,
            ActionValue = 0
        };
    }

    public override List<GridPosition> GetValidActionGridPositions()
        => new List<GridPosition>() { Unit.GetGridPosition() };
}
