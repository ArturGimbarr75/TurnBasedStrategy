using UnityEngine;

public interface IHitable
{
    void Hit(float damage, Vector3? hitSource = null);
}
