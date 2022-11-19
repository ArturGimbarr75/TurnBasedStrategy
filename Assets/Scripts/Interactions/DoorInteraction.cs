using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _isOpened;

    [Header("Doors")]
    [SerializeField] private Transform _leftDoor;
    [SerializeField] private Transform _rightDoor;

    [Header("Scale")]
    [SerializeField] private Vector3 _openedLeftScale;
    [SerializeField] private Vector3 _closedLeftScale;
    [SerializeField] private Vector3 _openedRightScale;
    [SerializeField] private Vector3 _closedRightScale;

    [Header("Animation")]
    [SerializeField] private AnimationCurve _lerpCurve;
    [SerializeField, Range(0.1f, 2)] private float _animationDuration;

    private Action _onInteractionComplete;
    private GridPosition _gridPosition;
    private float _duration;

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractable(_gridPosition, this);
        PathFinding.Instance.SetWalkable(_gridPosition, _isOpened);
    }

    private void Update()
    {
        if (_duration == _animationDuration)
            return;

        _duration += Time.deltaTime;

        if (_isOpened)
        {
            _leftDoor.localScale = Vector3.Lerp(_closedLeftScale, _openedLeftScale, _lerpCurve.Evaluate(_duration / _animationDuration));
            _rightDoor.localScale = Vector3.Lerp(_closedRightScale, _openedRightScale, _lerpCurve.Evaluate(_duration / _animationDuration));
        }
        else
        {
            _leftDoor.localScale = Vector3.Lerp(_openedLeftScale, _closedLeftScale, _lerpCurve.Evaluate(_duration / _animationDuration));
            _rightDoor.localScale = Vector3.Lerp(_openedRightScale, _closedRightScale, _lerpCurve.Evaluate(_duration / _animationDuration));
        }

        if (_duration > _animationDuration)
        {
            _duration = _animationDuration;
            _onInteractionComplete?.Invoke();
        }
    }

    public void Interact(Action onInteractionComplete)
    {
        _isOpened = !_isOpened;
        PathFinding.Instance.SetWalkable(_gridPosition, _isOpened);
        _duration = 0;
        _onInteractionComplete = onInteractionComplete;
    }
}
