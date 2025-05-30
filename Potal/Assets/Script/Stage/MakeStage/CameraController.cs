using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float scrollSpeed = 5f;
    [SerializeField]
    private float mouseSensitivity = 2.5f;
    [SerializeField]
    private Camera _camera;

    private float yaw;
    private float pitch;

    private Vector3 cameraOriginPosition;
    private Vector3 cameraOriginRotation;

    private void Awake()
    {
        _camera.transform.position = new Vector3(-80, 30, -80);
        _camera.transform.rotation = Quaternion.Euler(15, 45, 0);
        cameraOriginPosition = _camera.transform.position;
        Vector3 euler = _camera.transform.eulerAngles;
        cameraOriginRotation = euler;
        pitch = euler.x;
        yaw = euler.y;
    }
    private void Start()
    {
    }

    private void Update()
    {

    }

    private void LateUpdate()
    {
        if (_camera == null) return;
        ProcessCameraMove();
        ProcessCameraRotation();
        ProcessResetCameraPosition();
    }

    private void ProcessCameraMove()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        Vector3 inputDir = new Vector3(horizontal, 0f, vertical);
        if (inputDir.sqrMagnitude > 1f)
        {
            inputDir.Normalize();
        }

        Vector3 up = _camera.transform.up;
        Vector3 right = _camera.transform.right;
        Vector3 forward = _camera.transform.forward; ;

        Vector3 moveDir = up * inputDir.z + right * inputDir.x;
        Vector3 verticalMove =
            forward * scroll * scrollSpeed;

        Vector3 move = (moveDir * moveSpeed + verticalMove) * Time.deltaTime;
        _camera.transform.position += move;
    }

    void ProcessCameraRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            yaw += mouseX * mouseSensitivity;
            pitch -= mouseY * mouseSensitivity;

            _camera.transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }

    void ProcessResetCameraPosition()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            _camera.transform.position = cameraOriginPosition;
            _camera.transform.rotation = Quaternion.Euler(cameraOriginRotation);
            pitch = cameraOriginRotation.x;
            yaw = cameraOriginRotation.y;
        }
    }
}
