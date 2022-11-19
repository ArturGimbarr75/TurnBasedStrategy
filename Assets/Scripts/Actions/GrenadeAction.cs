using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static UnityEngine.Mathf;

public class GrenadeAction : BaseAction
{
    public override string ActionName => "Granade";

    [field: SerializeField, Min(1)] public int MaxThrowDistance { get; private set; }

    [SerializeField] private Transform _grenadeProjectilePrefab;

    private void Update()
    {
        if (!_isActive)
            return;
        ActionComplete();
    }

    public override EnemyAiAction GetEnemyAiAction(GridPosition gridPosition)
    {
        return new EnemyAiAction()
        {
            ActionValue = 0,
            GridPosition = gridPosition
        };
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {
        List<GridPosition> gridPositions = new List<GridPosition>();
        GridPosition unitGridPosition = Unit.GetGridPosition();
        for (int x = -MaxThrowDistance; x <= MaxThrowDistance; x++)
            for (int z = -MaxThrowDistance; z <= MaxThrowDistance; z++)
            {
                GridPosition offsetGridPos = new GridPosition(x, z);
                GridPosition testGridPos = unitGridPosition + offsetGridPos;
                int testDistance = Abs(x) + Abs(z);

                if (testDistance > MaxThrowDistance
                    || !LevelGrid.Instance.IsValidGridPosition(testGridPos))
                    continue;

                gridPositions.Add(testGridPos);
            }
        return gridPositions;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Transform grenade = Instantiate(_grenadeProjectilePrefab, Unit.GetWorldPosition(), Quaternion.identity);
        GrenadeProjectile grenadeProjectile = grenade.GetComponent<GrenadeProjectile>();
        grenadeProjectile.Setup(gridPosition, ActionComplete);
        base.TakeAction(gridPosition, onActionComplete);
    }
}
