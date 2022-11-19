using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _selected;

    private BaseAction _baseAction;

    public void SetAction(BaseAction action)
    {
        _baseAction = action;
        _text.text = action.ActionName;
        _button.onClick.AddListener(() => UnitActionSystem.Instance.SelectedAction = action);
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selected = UnitActionSystem.Instance.SelectedAction;
        _selected.SetActive(selected == _baseAction);
    }
}
