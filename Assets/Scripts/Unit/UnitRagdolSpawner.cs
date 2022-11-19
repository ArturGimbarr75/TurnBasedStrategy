using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdolSpawner : MonoBehaviour
{
    [SerializeField] private Transform _ragdollPrefab;
    [SerializeField] private Transform _originalRootBone;

    private HealthSystem _healthSystem;

    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.OnDead += OnDead;
    }

    private void OnDead(object sender, DeadEventArgs args)
    {
        Instantiate(_ragdollPrefab, transform.position, transform.rotation)
            .GetComponent<UnitRagdoll>().Setup(_originalRootBone, args.Damage, args.HitSource);
    }

    private void OnDisable()
    {
        _healthSystem.OnDead -= OnDead;
    }
}
