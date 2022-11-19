using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{
    public override string ActionName => "Interact";

    [SerializeField] private bool _diamondFormDistance;
    [SerializeField, Min(-1)] private int _maxInteractDistance;

    public override EnemyAiAction GetEnemyAiAction(GridPosition gridPosition)
    {
        return new EnemyAiAction()
        {
            GridPosition = gridPosition,
            ActionValue = 0
        };
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {
        List<GridPosition> gridPositions = new List<GridPosition>();
        GridPosition unitGridPosition = Unit.GetGridPosition();

        int startX = _maxInteractDistance != -1? unitGridPosition.X - _maxInteractDistance : 0;
        int startZ = _maxInteractDistance != -1? unitGridPosition.Z - _maxInteractDistance : 0;
        int endX = _maxInteractDistance != -1
            ?unitGridPosition.X + _maxInteractDistance
            : LevelGrid.Instance.Width - 1;
        int endZ = _maxInteractDistance != -1
            ? unitGridPosition.X + _maxInteractDistance
            : LevelGrid.Instance.Height - 1;

        for (int x = startX; x <= endX; x++)
            for (int z = startZ; z <= endZ; z++)
            {
                GridPosition testGridPos = new GridPosition(x, z);

                if (_diamondFormDistance && testGridPos.Distance(unitGridPosition) > _maxInteractDistance)
                    continue;

                if (   !LevelGrid.Instance.IsValidGridPosition(testGridPos)
                    || !LevelGrid.Instance.TryGetDoorAtGridPosition(testGridPos, out _))
                    continue;

                gridPositions.Add(testGridPos);
            }

        return gridPositions;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        if (LevelGrid.Instance.TryGetDoorAtGridPosition(gridPosition, out IInteractable door))
        {  
            base.TakeAction(gridPosition, onActionComplete);
            door.Interact(ActionComplete);
        }
    }
}
