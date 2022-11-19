using System;
using UnityEngine;

public class DeadEventArgs : EventArgs
{
    public Vector3? HitSource;
    public float Damage;
}
