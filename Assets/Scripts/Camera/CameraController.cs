using Cinemachine;
using UnityEngine;

using static UnityEngine.Mathf;

public class CameraController : MonoBehaviour
{
    [Header("Params")]
    [SerializeField, Range(0.1f, 10)] private float _movementSpeed;
    [SerializeField, Range(0.1f, 180)] private float _rotationSpeed;
    [SerializeField, Range(0.01f, 1)] private float _zoomSpeed;

    [Header("Ancors")]
    [SerializeField] private Vector3 _upAncor;
    [SerializeField] private Vector3 _downAncor;

    [Space(2)]
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    private CinemachineTransposer _cinemachineTransposer;
    private float _zoomLerp = 1;

    private void Start()
    {
        _cinemachineTransposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Update()
    {
        Vector2 inputVector = InputManager.Instance.GetCameraMoveVector();
        float rotation = InputManager.Instance.GetCameraRotateAmount() * _rotationSpeed;
        _zoomLerp += InputManager.Instance.GetCameraZoomAmount() * _zoomSpeed;
        _zoomLerp = Clamp(_zoomLerp, 0, 1);

        if (inputVector == Vector2.zero && rotation == 0 && InputManager.Instance.GetCameraZoomAmount() == 0)
            return;

        Vector3 moveVector = transform.forward * inputVector.y + transform.right * inputVector.x;
        transform.position += moveVector * Time.deltaTime * _movementSpeed;
        transform.Rotate(Vector3.up * rotation * Time.deltaTime);
        _cinemachineTransposer.m_FollowOffset = Vector3.Lerp(_downAncor, _upAncor, _zoomLerp);
    }
}
