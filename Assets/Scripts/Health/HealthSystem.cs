using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IHitable
{
    public event EventHandler<DeadEventArgs> OnDead;
    public event EventHandler<HealthEventArgs> OnHealthChanged;

    [field: SerializeField, Min(0.1f)] public float Health { get; private set; } = 100;
    public float MaxHealth { get; private set; }
    public float NormalizedHealth => Health / MaxHealth;

    public const float MAX_ALLOWED_HEALTH = 3000;

    private void Awake()
    {
        MaxHealth = Mathf.Min(Health, MAX_ALLOWED_HEALTH);
    }

    public void Hit(float damage, Vector3? hitSource = null)
    {
        Health -= damage;

        if (Health < 0)
            Health = 0;

        OnHealthChanged?.Invoke(this, new()
        {
            Health = Health,
            MaxHealth = MaxHealth,
            HitSource = hitSource,
            Damage = damage
        });

        if (Health == 0)
            OnDead?.Invoke(this, new() { HitSource = hitSource, Damage = damage });
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        if (Health > MAX_ALLOWED_HEALTH)
            Health = MAX_ALLOWED_HEALTH;
    }

#endif
}
