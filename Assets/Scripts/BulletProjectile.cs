using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField, Min(10)] private float _speed = 20;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private Transform _bulletHitVfxPrefab;

    private Vector3 _targetPosition;

    public void Setup(Vector3 targetPosition)
        => _targetPosition = targetPosition;

    void Update()
    {
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(_targetPosition, transform.position);
        transform.position += moveDirection * _speed * Time.deltaTime;

        if (distance < Vector3.Distance(_targetPosition, transform.position))
        {
            transform.position = _targetPosition;
            _trailRenderer.transform.parent = null;
            Destroy(gameObject);
            Instantiate(_bulletHitVfxPrefab, _targetPosition, Quaternion.identity);
        }
    }
}
