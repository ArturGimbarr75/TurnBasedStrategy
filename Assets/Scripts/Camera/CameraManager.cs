using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject _actionCamera;

    private void Start()
    {
        BaseAction.OnAnyActionStarted += OnAnyActionStarted;
        BaseAction.OnAnyActionComplited += OnAnyActionComplited;

        HideActionCamera();
    }

    private void OnAnyActionStarted(object sender, EventArgs args)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                _actionCamera.transform.parent = shootAction.ActionCameraPosition;
                _actionCamera.transform.localPosition = Vector3.zero;
                _actionCamera.transform.localRotation = Quaternion.identity;
                ShowActionCamera();
                break;
        }    
    }

    private void OnAnyActionComplited(object sender, EventArgs args)
    {
        switch (sender)
        {
            case ShootAction:
                _actionCamera.transform.parent = null;
                HideActionCamera();
                break;
        }
    }

    private void ShowActionCamera()
    {
        _actionCamera.SetActive(true);
    }

    private void HideActionCamera()
    {
        _actionCamera.SetActive(false);
    }

    private void OnDisable()
    {
        BaseAction.OnAnyActionStarted -= OnAnyActionStarted;
    }
}
