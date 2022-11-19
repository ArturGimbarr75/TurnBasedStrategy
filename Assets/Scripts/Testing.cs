using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GridPosition mouseGridPos = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            GridPosition start = new();

            IEnumerable<Vector3> path = PathFinding.Instance.FindPath(start, mouseGridPos)
                .Select(x => LevelGrid.Instance.GetWorldPosition(x));

            Vector3 previours = path.First();
            foreach (Vector3 p in path)
            {
                Debug.Log(p);
                Debug.DrawLine(previours, p, Color.black, 50);
                previours = p;
            }
        }
    }
}
