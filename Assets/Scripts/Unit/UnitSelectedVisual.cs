using System;
using UnityEngine;
using UAS = UnitActionSystem;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit _unit;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        UAS.Instance.OnSelectedUnitChanged += UpdateVisual;
        UpdateVisual();
    }

    private void UpdateVisual(object sender = null, EventArgs args = null)
    {
        _meshRenderer.enabled = UAS.Instance.SelectedUnit.transform == _unit.transform;
    }

    private void OnDisable()
    {
        UAS.Instance.OnSelectedUnitChanged -= UpdateVisual;
    }
}
