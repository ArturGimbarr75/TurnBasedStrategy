using System;
using UnityEngine;

public class DestructableCrate : MonoBehaviour, IHitable
{
    public static event EventHandler OnAnyDestroyed;

    public GridPosition GridPosition { get; private set; }

    [SerializeField] private Transform _crateDestroyPrefab;
    [SerializeField, Range(0.01f, 1)] private float _explosionForceMultiplier;

    private void Start()
    {
        GridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public void Hit(float damage, Vector3? hitSource)
    {
        const float ADDITIONAL_RANGE = 5;
        Transform root = Instantiate(_crateDestroyPrefab, transform.position, transform.rotation);
        hitSource ??= transform.position;
        ApplyExplosionToChildren(root, damage * _explosionForceMultiplier,
            hitSource.Value, Vector3.Distance(hitSource.Value, transform.position) + ADDITIONAL_RANGE);
        Destroy(gameObject);
        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
    }

    private void ApplyExplosionToChildren(Transform root, float explosionForce,
        Vector3 explosionPosition, float explosionRange)
    {
        const float UPWARDS_MODIFIER = 1;
        foreach (Transform child in root)
        {
            if (child.TryGetComponent(out Rigidbody childRigidbody))
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange,
                    UPWARDS_MODIFIER, ForceMode.Impulse);
            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
