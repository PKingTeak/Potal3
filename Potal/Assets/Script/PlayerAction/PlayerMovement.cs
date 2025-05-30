using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed = 5f;
    private Vector2 _curMovementInput;

    [Header("Look")]
    [SerializeField] private Transform cameraContainer;
    [SerializeField] private float minXLook = -80f;
    [SerializeField] private float maxXLook = 80f;
    [SerializeField] private SettingData settingData;
    private float _camCurXRot;
    private Vector2 _mouseDelta;
    [SerializeField] private bool canLook = true;

    private PlayerCrouch _playerCrouch;
    private Rigidbody _rigidbody;
    private GroundChecker _groundChecker;
    private Animator _animator;

    [SerializeField] private bool isJumping = false;

    private void Start()
    {
        settingData = SettingManager.Instance.Current;
        Cursor.lockState = CursorLockMode.Locked;

        _rigidbody = GetComponent<Rigidbody>();
        _groundChecker = GetComponent<GroundChecker>();
        _animator = GetComponent<Animator>();
        _playerCrouch = GetComponent<PlayerCrouch>();
    }

    private void FixedUpdate()
    {
        if (isJumping)
            return;

        Move();
        SmoothLanding();
        UpdateAnimatorBlend();
    }

    private void LateUpdate()
    {
        if (canLook)
            CameraLook();
    }

    private void Move()
    {
        Vector3 camForward = cameraContainer.forward;
        Vector3 camRight = cameraContainer.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * _curMovementInput.y + camRight * _curMovementInput.x).normalized;

        float crouchMultiplier = 1f;
        
        if (_playerCrouch != null)
            crouchMultiplier = _playerCrouch.SpeedMultiplier;

        Vector3 desiredVelocity = moveDir * (maxSpeed * crouchMultiplier);
        Vector3 currentHorizontalVelocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
        Vector3 velocityChange = desiredVelocity - currentHorizontalVelocity;

        _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    private void SmoothLanding()
    {
        if (_groundChecker != null && _groundChecker.IsGrounded)
        {
            Vector3 velocity = _rigidbody.velocity;
            if (velocity.y < -2f)
            {
                _rigidbody.velocity = new Vector3(velocity.x, -1f, velocity.z);
            }
        }
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

    public void SetJumping(float duration)
    {
        StartCoroutine(JumpDuration(duration));
    }

    private IEnumerator JumpDuration(float duration)
    {
        isJumping = true;
        yield return new WaitForSeconds(duration);
        isJumping = false;
    }

    private void UpdateAnimatorBlend()
    {
        if (_animator == null) return;

        Vector3 horizontalVelocity = _rigidbody.velocity;
        horizontalVelocity.y = 0f;

        float speed = horizontalVelocity.magnitude / maxSpeed;
        _animator.SetFloat("Blend", speed, 0.1f, Time.deltaTime);
    }
}
