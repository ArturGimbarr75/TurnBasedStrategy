using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }
    public int TurnNumber { get; private set; }
    public bool IsPlayerTurn { get; private set; } = true;

    public event EventHandler OnTurnChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"There's more than one {typeof(TurnSystem).FullName} " + transform + " - " + Instance);
            return;
        }
        Instance = this;
    }

    public void NextTurn()
    {
        TurnNumber++;
        IsPlayerTurn = !IsPlayerTurn;
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }
}
