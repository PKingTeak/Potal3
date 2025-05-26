using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace SW
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float scrollSpeed = 5f;
        [SerializeField] private float mouseSensitivity = 2.5f;

        private float yaw;
        private float pitch;
        private new Camera camera;

        private Vector3 cameraOriginPosition;
        private Vector3 cameraOriginRotation;

        private void Awake()
        {
        }
        private void Start()
        {
            camera = Camera.main;
            camera.transform.position = new Vector3(0, 140, 0);
            cameraOriginPosition = camera.transform.position;
            Vector3 euler = camera.transform.eulerAngles;
            cameraOriginRotation = euler;
            pitch = euler.x;
            yaw = euler.y;
        }

        private void Update()
        {

        }

        private void LateUpdate()
        {
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

            Vector3 up = camera.transform.up;
            Vector3 right = camera.transform.right;
            Vector3 forward = camera.transform.forward;
            up.Normalize();
            right.Normalize();

            Vector3 moveDir = up * inputDir.z + right * inputDir.x;
            Vector3 verticalMove = forward * scroll * scrollSpeed;

            Vector3 move = (moveDir * moveSpeed + verticalMove) * Time.deltaTime;
            camera.transform.position += move;
        }

        void ProcessCameraRotation()
        {
            if (Input.GetMouseButton(1))
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                yaw += mouseX * mouseSensitivity;
                pitch -= mouseY * mouseSensitivity;

                camera.transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
            }
        }

        void ProcessResetCameraPosition()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                camera.transform.position = cameraOriginPosition;
                camera.transform.rotation = Quaternion.Euler(cameraOriginRotation);
            }
        }
    }
}
