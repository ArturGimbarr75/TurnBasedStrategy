using System;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyGrenadeExploded;

    [Header("Throw")]
    [SerializeField] private AnimationCurve _throwParabola;
    [SerializeField, Min(0)] private float _maxHeight;

    [Header("VFX")]
    [SerializeField] private Transform _explodeVfxPrefab;
    [SerializeField] private Vector3 _explodeVfxOffset;
    [SerializeField] private TrailRenderer _trailRenderer;

    [Header("Params")]
    [SerializeField, Min(0.1f)] private float _moveSpeed;
    [SerializeField, Min(0f)] private float _damage;
    [SerializeField, Min(0.1f)] private float _damageRadiusInCells;
    [SerializeField] private AnimationCurve _damageMultiplierCurve;

    private Vector3 _currentPosition;
    private Vector3 _targetPosition;
    private Vector3 _moveDir;
    private float _distance;
    private float _totalDistance;
    private Action _onGrenadeExplodes;

    private void Update()
    {
        float translation = _moveSpeed * Time.deltaTime;
        _distance -= translation;
        _currentPosition += _moveDir * translation;
        _currentPosition.y = _throwParabola.Evaluate(1 - _distance / _totalDistance) * _totalDistance / _maxHeight; // TODO: Fix throwing
        transform.position = _currentPosition;
        

        if (_distance <= 0)
        {
            float damageDistance = _damageRadiusInCells * LevelGrid.Instance.CellSize;
            Collider[] colliders = Physics.OverlapSphere(_targetPosition, damageDistance);
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out IHitable hitable))
                {
                    if (hitable is MonoBehaviour targetUnit)
                    {
                        var distanceToUnit = Vector3.Distance(targetUnit.transform.position, _targetPosition);
                        if (distanceToUnit < damageDistance)
                        {
                            float curveTime = distanceToUnit / damageDistance;
                            hitable.Hit(_damage * _damageMultiplierCurve.Evaluate(curveTime), _targetPosition);
                        }
                    }
                }
            }
            Destroy(gameObject);
            _onGrenadeExplodes?.Invoke();
            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
            _trailRenderer.transform.parent = null;
            Instantiate(_explodeVfxPrefab, _targetPosition + _explodeVfxOffset, Quaternion.identity);
        }
    }

    public void Setup(GridPosition gridPosition, Action onGrenadeExplodes)
    {
        _targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        _moveDir = (_targetPosition - transform.position).normalized;
        _distance = Vector3.Distance(_targetPosition, transform.position);
        _totalDistance = _distance;
        _onGrenadeExplodes = onGrenadeExplodes;
        _currentPosition = transform.position;
    }
}
