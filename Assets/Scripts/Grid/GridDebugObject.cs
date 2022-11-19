using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro _textMeshPro;
    private object _gridObject;

    public virtual void SetGridObject(object gridObject) => _gridObject = gridObject;

    protected virtual void Start()
    {
        _textMeshPro.text = $"{_gridObject:2L}";
    }
}
