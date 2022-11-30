using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    public bool Hiden { set => _meshRenderer.enabled = !value; }
<<<<<<< HEAD

=======
>>>>>>> master
    [SerializeField] private MeshRenderer _meshRenderer;

    public void SetMaterial(Material material)
        => _meshRenderer.material = material;    
}
