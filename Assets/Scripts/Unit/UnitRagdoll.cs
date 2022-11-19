using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform _ragdollRootBone;
    [SerializeField, Range(0.01f, 1)] private float _explosionForceMultiplier = 1;

    private float _explosionRange;

    public void Setup(Transform originalRootBone, float damage, Vector3? hitSource = null)
    {
        MatchAllChildTransforms(originalRootBone, _ragdollRootBone);
        hitSource ??= transform.position;
        _explosionRange = Vector3.Distance(hitSource.Value, transform.position) * 2;
        ApplyExplosionToRagdoll(_ragdollRootBone, _explosionForceMultiplier * damage, hitSource.Value, _explosionRange);
    }

    private void MatchAllChildTransforms(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;

                MatchAllChildTransforms(child, cloneChild);
            }
        }
    }

    private void ApplyExplosionToRagdoll(Transform root, float explosionForce,
        Vector3 explosionPosition, float explosionRange)
    {
        const float UPWARDS_MODIFIER = 1;
        foreach (Transform child in root)
        {
            if (child.TryGetComponent(out Rigidbody childRigidbody))
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange,
                    UPWARDS_MODIFIER, ForceMode.Impulse);
            ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
