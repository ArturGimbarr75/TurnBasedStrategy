using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingUpdater : MonoBehaviour
{
    private void Start()
    {
        DestructableCrate.OnAnyDestroyed += OnAnyDestroyed;
    }

    private void OnAnyDestroyed(object sender, EventArgs args)
    {
        if (sender is DestructableCrate destructableCrate)
            PathFinding.Instance.SetWalkable(destructableCrate.GridPosition);
    }

    private void OnDisable()
    {
        DestructableCrate.OnAnyDestroyed -= OnAnyDestroyed;
    }
}
