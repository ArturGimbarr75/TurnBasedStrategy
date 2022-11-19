using System;

public class HealthEventArgs : DeadEventArgs
{
    public float Health;
    public float MaxHealth;
    public float NormalizedHealth => Health / MaxHealth;
}
