using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance { get; private set; }

    private CinemachineImpulseSource _impulseSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"There's more than one {typeof(ScreenShake).FullName} " + transform + " - " + Instance);
            return;
        }
        Instance = this;
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(float intensity = 1)
    {
        _impulseSource.GenerateImpulse(intensity);
    }
}
