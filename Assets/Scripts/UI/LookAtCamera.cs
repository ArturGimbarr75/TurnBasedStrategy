using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool _invert = true;

    private Transform _camera;

    void Start()
    {
        _camera = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (_invert)
            transform.LookAt(transform.position + (_camera.position - transform.position) * -1);
        else
            transform.LookAt(_camera);
    }
}
