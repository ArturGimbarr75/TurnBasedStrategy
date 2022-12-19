using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    [Header("Positions")]
    [SerializeField] private Vector3 _mainPosition;
    [SerializeField] private Vector3 _buttonsPosition;

    [Header("Play")]
    [SerializeField] private AnimationCurve _moveToButtonsCurve;
    [SerializeField] private float _moveToButtonsDuration;

    [Header("Buttons")]
    [SerializeField] private AnimationCurve _moveToMainCurve;
    [SerializeField] private float _moveToMainDuration;

    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private AnimationCurve _moveCurve;
    private float _duration;
    private float _elapsedDuration;
    private Transform _camera;

    private void Start()
    {
        _camera = Camera.main.transform;
    }

    private void Update()
    {
        if (_elapsedDuration >= _duration)
            return;

        _elapsedDuration = Mathf.Min(_elapsedDuration + Time.deltaTime, _duration);
        _camera.position = Vector3.Lerp
        (
            _startPosition,
            _targetPosition,
            _moveCurve.Evaluate(_elapsedDuration / _duration)
        );
    }

    public void MoveToMain()
    {
        _startPosition = _buttonsPosition;
        _targetPosition = _mainPosition;
        _moveCurve = _moveToMainCurve;
        _duration = _moveToMainDuration;
        _elapsedDuration = 0;
    }

    public void MoveToButtons()
    {
        _startPosition = _mainPosition;
        _targetPosition = _buttonsPosition;
        _moveCurve = _moveToButtonsCurve;
        _duration = _moveToButtonsDuration;
        _elapsedDuration = 0;
    }
}
