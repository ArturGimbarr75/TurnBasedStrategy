using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private Material[] _materials;

    private int _index = 0;
    private MeshRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();
        _renderer.material = _materials[_index];

        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractable(gridPosition, this);
        PathFinding.Instance.SetWalkable(gridPosition, false);
    }

    public void Interact(Action onInteractionComplete)
    {
        _index++;
        if (_index >= _materials.Length)
            _index = 0;
        _renderer.material = _materials[_index];
        onInteractionComplete?.Invoke();
    }
}
