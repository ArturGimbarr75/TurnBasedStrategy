using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _actionPointsText;
    [SerializeField] private Unit _unit;
    [SerializeField] private HealthSystem _healthSystem;
    [SerializeField] private Image _healthBarImage;
    [SerializeField] private Gradient _healthBarGradient;

    private void Start()
    {
        Unit.OnAnyActionPointsChanged += UpdateActionPointsText;
        _healthSystem.OnHealthChanged += OnHealthChanged;
        UpdateActionPointsText();
        OnHealthChanged(null, new() { Health = 1, MaxHealth = 1 });
    }

    private void OnHealthChanged(object sender, HealthEventArgs args)
    {
        _healthBarImage.color = _healthBarGradient.Evaluate(args.NormalizedHealth);
        _healthBarImage.fillAmount = args.NormalizedHealth;
    }

    private void UpdateActionPointsText(object sender = null, EventArgs args = null)
    {
        _actionPointsText.text = _unit.GetActionPoints().ToString();
    }

    private void OnDisable()
    {
        Unit.OnAnyActionPointsChanged -= UpdateActionPointsText;
        _healthSystem.OnHealthChanged -= OnHealthChanged;
    }
}
