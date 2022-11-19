using System;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    [SerializeField, Min(0)] private float _shootIntensity = 1;
    [SerializeField, Min(0)] private float _swordHitIntensity = 3;
    [SerializeField, Min(0)] private float _explodeIntensity = 5;

    private void Start()
    {
        ShootAction.OnAnyShoot += OnAnyShoot;
        GrenadeProjectile.OnAnyGrenadeExploded += OnAnyExploded;
        SwordAction.OnHit += OnSwordHit;
    }

    private void OnSwordHit(object sender, EventArgs args)
    {
        ScreenShake.Instance.Shake(_swordHitIntensity);
    }

    private void OnAnyShoot(object sender, EventArgs args)
    {
        ScreenShake.Instance.Shake(_shootIntensity);
    }

    private void OnAnyExploded(object sender, EventArgs args)
    {
        ScreenShake.Instance.Shake(_explodeIntensity);
    }

    private void OnDisable()
    {
        ShootAction.OnAnyShoot -= OnAnyShoot;
        GrenadeProjectile.OnAnyGrenadeExploded -= OnAnyExploded;
        SwordAction.OnHit -= OnSwordHit;
    }
}
