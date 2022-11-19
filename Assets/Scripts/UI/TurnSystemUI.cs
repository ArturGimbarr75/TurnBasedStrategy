using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using static TurnSystem;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button _endTurnButton;
    [SerializeField] private TextMeshProUGUI _turnNumberText;
    [SerializeField] private GameObject _enemyVisual;

    private void Start()
    {
        _endTurnButton.onClick.AddListener(() => Instance.NextTurn());
        Instance.OnTurnChanged += UpdateTurnText;
        Instance.OnTurnChanged += UpdateEnemyTurnVisual;
        Instance.OnTurnChanged += UpdateEndTurnButton;

        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButton();
    }

    private void UpdateTurnText(object sender = null, EventArgs args = null)
    {
        _turnNumberText.text = $"TURN {Instance.TurnNumber}";
    }

    private void UpdateEnemyTurnVisual(object sender = null, EventArgs args = null)
    {
        _enemyVisual.SetActive(!Instance.IsPlayerTurn);
    }

    private void UpdateEndTurnButton(object sender = null, EventArgs args = null)
    {
        _endTurnButton.gameObject.SetActive(Instance.IsPlayerTurn);
    }

    private void OnDisable()
    {
        Instance.OnTurnChanged -= UpdateTurnText;
        Instance.OnTurnChanged -= UpdateEnemyTurnVisual;
        Instance.OnTurnChanged -= UpdateEndTurnButton;
    }
}
