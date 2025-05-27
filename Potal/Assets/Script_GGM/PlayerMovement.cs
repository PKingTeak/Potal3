using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float airControlMultiplier = 0.3f; // 공중 제어력 감소 비율
    private Vector2 _curMovementInput;

    [Header("Look")]
    [SerializeField] private Transform cameraContainer;
    [SerializeField] private float minXLook = -80f;
    [SerializeField] private float maxXLook = 80f;
    //[SerializeField] private float lookSensitivity = 1f;
    [SerializeField] private SettingData settingData;
	private float _camCurXRot;
    private Vector2 _mouseDelta;
    [SerializeField] private bool canLook = true;

    private Rigidbody _rigidbody;
    private GroundChecker _groundChecker;

    private void Start()
    {
        settingData = SettingData.Instance;
        Cursor.lockState = CursorLockMode.Locked;
        _rigidbody = GetComponent<Rigidbody>();
        _groundChecker = GetComponent<GroundChecker>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
            CameraLook();
    }

    private void Move()
    {
        // 이동 방향 계산 (카메라 기준)
        Vector3 camForward = cameraContainer.forward;
        Vector3 camRight = cameraContainer.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * _curMovementInput.y + camRight * _curMovementInput.x).normalized;

        // 공중 제어력 보정
        float jumpMultiplier = _groundChecker != null && _groundChecker.IsGrounded ? 1f : airControlMultiplier;

        // 앉기 속도 보정
        float crouchMultiplier = 1f;
        var crouch = GetComponent<PlayerCrouch>();
        if (crouch != null)
            crouchMultiplier = crouch.SpeedMultiplier;

        // 최종 속도 계산
        Vector3 targetVelocity = moveDir * maxSpeed * jumpMultiplier * crouchMultiplier;

        // 적용
        _rigidbody.velocity = new Vector3(targetVelocity.x, _rigidbody.velocity.y, targetVelocity.z);
    }


    private void CameraLook()
    {
        _camCurXRot += _mouseDelta.y * settingData.lookSensitivity;
        _camCurXRot = Mathf.Clamp(_camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-_camCurXRot, 0f, 0f);

        transform.eulerAngles += new Vector3(0f, _mouseDelta.x * settingData.lookSensitivity, 0f);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
            _curMovementInput = context.ReadValue<Vector2>();
        else if (context.canceled)
            _curMovementInput = Vector2.zero;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _mouseDelta = context.ReadValue<Vector2>();
    }
}
