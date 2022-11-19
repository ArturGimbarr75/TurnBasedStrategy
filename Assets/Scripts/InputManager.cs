#define NEW_INPUT_SYSTEM

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private PlayerInputActions _inputActions;

    private void Awake()
    {
        Instance = this;

        _inputActions = new PlayerInputActions();
        _inputActions.Player.Enable();
    }

    public Vector2 GetMouseScreenPosition()
    {
#if NEW_INPUT_SYSTEM
        return Mouse.current.position.ReadValue();
#else
        return Input.mousePosition;
#endif
    }

    public bool IsMouseButtonDownThisFrame()
    {
#if NEW_INPUT_SYSTEM
        return _inputActions.Player.Click.WasPressedThisFrame();
#else
        return Input.GetMouseButtonDown(0);
#endif
    }

    public Vector2 GetCameraMoveVector()
    {
#if NEW_INPUT_SYSTEM
        return _inputActions.Player.CameraMovement.ReadValue<Vector2>();
#else
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
#endif
    }

    public float GetCameraRotateAmount()
    {
#if NEW_INPUT_SYSTEM
        return _inputActions.Player.CameraRotate.ReadValue<float>();
#else
        float rotation = 0;
        if (Input.GetKey(KeyCode.Q))
            rotation += 1;
        if (Input.GetKey(KeyCode.E))
            rotation -= 1;
        return rotation;
#endif
    }

    public float GetCameraZoomAmount()
    {
#if NEW_INPUT_SYSTEM
        return _inputActions.Player.CameraZoom.ReadValue<float>();
#else
        return -Input.mouseScrollDelta.y;
#endif
    }
}
