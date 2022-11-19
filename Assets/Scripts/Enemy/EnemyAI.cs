using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField, Min(0)] private float _turnDuration = 1;

    private float _timer;
    private State _state;

    private void Awake()
    {
        _state = State.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += OnTurnChanged;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn)
            return;

        switch (_state)
        {
            case State.WaitingForEnemyTurn:
                break;

            case State.TaikingTurn:  
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    _state = State.Busy;
                    if (TryTakeEnemyAiAction(SetStateTakingTurn))
                        _state = State.Busy;
                    else
                        TurnSystem.Instance.NextTurn();
                }
                break;

            case State.Busy:
                break;
        }        
    }

    private void OnTurnChanged(object sender, EventArgs args)
    {
        if (TurnSystem.Instance.IsPlayerTurn)
            return;

        _state = State.TaikingTurn;
        _timer = _turnDuration;
    }

    private bool TryTakeEnemyAiAction(Action onEnemyAiActionComplete)
    {
        foreach (Unit enemyUnit in UnitManager.Instance.EnemyUnits)
        {
            if (TryTakeEnemyAiAction(enemyUnit, onEnemyAiActionComplete))
                return true;
        }

        return false;
    }

    private bool TryTakeEnemyAiAction(Unit unit, Action onEnemyAiActionComplete)
    {
        EnemyAiAction bestAiAction = null;
        BaseAction bestAction = null;
        foreach (BaseAction action in unit.GetActions())
        {
            if (!unit.CanSpendActionPointsToTakeAction(action))
                continue;

            EnemyAiAction testAction = action.GetBestEnemyAiAction();
            if (bestAiAction == null || testAction != null && testAction.ActionValue > bestAiAction.ActionValue)
            {
                bestAiAction = action.GetBestEnemyAiAction();
                bestAction = action;
            }   
        }

        if (bestAiAction != null && unit.TrySpendActionPointsToTakeAction(bestAction))
        {
            bestAction.TakeAction(bestAiAction.GridPosition, onEnemyAiActionComplete);
            return true;
        }
        else
            return false;
    }

    private void SetStateTakingTurn()
    {
        _timer = _turnDuration;
        _state = State.TaikingTurn;
    }

    enum State
    {
        WaitingForEnemyTurn,
        TaikingTurn,
        Busy
    }
}
