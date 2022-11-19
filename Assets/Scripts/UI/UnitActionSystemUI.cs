using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using static UnitActionSystem;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform _actionButtonPrefab;
    [SerializeField] private Transform _buttonsContainer;
    [SerializeField] private TextMeshProUGUI _actionPointsText;

    private List<ActionButtonUI> _actionButtons;

    private void Start()
    {
        _actionButtons = new List<ActionButtonUI>();
        Instance.OnSelectedUnitChanged += OnSelectedUnitChanged;
        Instance.OnSelectedActionChanged += UpdateSelectedVisual;
        Instance.OnActionStarted += OnActionStarted;
        TurnSystem.Instance.OnTurnChanged += OnTurnChanged;
        Unit.OnAnyActionPointsChanged += OnAnyActionPointsChanged;
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void OnTurnChanged(object sender, EventArgs args)
    {
        UpdateActionPoints();
    }

    private void CreateUnitActionButtons()
    {
        foreach (Transform button in _buttonsContainer)
            Destroy(button.gameObject);

        _actionButtons.Clear();

        Unit selectedUnit = Instance.SelectedUnit;
        foreach (BaseAction action in selectedUnit.GetActions())
        {
            ActionButtonUI button = Instantiate(_actionButtonPrefab, _buttonsContainer).GetComponent<ActionButtonUI>();
            button.SetAction(action);
            _actionButtons.Add(button);
        }
    }

    private void OnSelectedUnitChanged(object sender, EventArgs args)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }

    private void OnActionStarted(object sender, EventArgs args)
    {
        UpdateActionPoints();
    }

    private void OnAnyActionPointsChanged(object sender, EventArgs args)
    {
        UpdateActionPoints();
    }

    private void UpdateSelectedVisual(object sender = null, EventArgs args = null)
    {
        UpdateActionPoints();
        foreach (ActionButtonUI buttonUI in _actionButtons)
            buttonUI.UpdateSelectedVisual();
    }

    private void UpdateActionPoints()
    {
        int points = Instance.SelectedUnit.GetActionPoints();
        _actionPointsText.text = $"Action points: {points}";
    }

    private void OnDisable()
    {
        Instance.OnSelectedUnitChanged -= OnSelectedUnitChanged;
        Instance.OnSelectedActionChanged -= UpdateSelectedVisual;
        Instance.OnActionStarted -= OnActionStarted;
        TurnSystem.Instance.OnTurnChanged -= OnTurnChanged;
        Unit.OnAnyActionPointsChanged -= OnAnyActionPointsChanged;
    }
}
