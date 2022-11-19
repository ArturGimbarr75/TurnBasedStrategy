using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PathFindingGridDebugObject : GridDebugObject
{
    [SerializeField] private TextMeshPro _gCostText;
    [SerializeField] private TextMeshPro _hCostText;
    [SerializeField] private TextMeshPro _fCostText;
    [SerializeField] private SpriteRenderer _isWalkableSprite;
    
    private PathNode _pathNode;

    public override void SetGridObject(object gridObject)
    {
        _pathNode = gridObject as PathNode;
        base.SetGridObject(gridObject);
    }

    protected override void Start()
    {
        base.Start();
        _gCostText.text = $"G: {_pathNode.GCost}";
        _hCostText.text = $"H: {_pathNode.HCost}";
        _fCostText.text = $"F: {_pathNode.FCost}";
        _isWalkableSprite.color = _pathNode.IsWalkable ? Color.green : Color.red;
    }
}
