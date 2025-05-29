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

    private Rigidbody _rigidbody;
    private GroundChecker _groundChecker;
    private Animator _animator;

    [SerializeField] private bool isJumping = false;
    private bool _wasTouchingWallLastFrame = false;

    private void Start()
    {
        settingData = SettingManager.Instance.Current;
        Cursor.lockState = CursorLockMode.Locked;

        _rigidbody = GetComponent<Rigidbody>();
        _groundChecker = GetComponent<GroundChecker>();
        _animator = GetComponent<Animator>();
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
        var crouch = GetComponent<PlayerCrouch>();
        if (crouch != null)
            crouchMultiplier = crouch.SpeedMultiplier;

        Vector3 desiredVelocity = moveDir * maxSpeed * crouchMultiplier;
        Vector3 currentHorizontalVelocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
        Vector3 velocityChange = desiredVelocity - currentHorizontalVelocity;

        if (!_groundChecker.IsGrounded)
        {
            if (IsTouchingWallMultiDirection(_curMovementInput, out Vector3 wallNormal))
            {
                velocityChange = Vector3.zero;

                if (!_wasTouchingWallLastFrame)
                {
                    _rigidbody.AddForce(wallNormal * 1.5f, ForceMode.Impulse);
                    _wasTouchingWallLastFrame = true;
                }

                Vector3 vel = _rigidbody.velocity;
                _rigidbody.velocity = new Vector3(0f, vel.y, 0f);
            }
            else
            {
                velocityChange *= 0.5f;
                _wasTouchingWallLastFrame = false;
            }
        }
        else
        {
            _wasTouchingWallLastFrame = false;
        }

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

    private bool IsTouchingWallMultiDirection(Vector2 moveInput, out Vector3 wallNormal)
    {
        wallNormal = Vector3.zero;

        Vector3 origin = transform.position + Vector3.up * 0.5f;
        List<Vector3> checkDirs = new List<Vector3>();

        if (moveInput.y > 0.1f) checkDirs.Add(cameraContainer.forward);
        if (moveInput.y < -0.1f) checkDirs.Add(-cameraContainer.forward);
        if (moveInput.x > 0.1f) checkDirs.Add(cameraContainer.right);
        if (moveInput.x < -0.1f) checkDirs.Add(-cameraContainer.right);

        Vector3 lookDir = cameraContainer.forward;
        lookDir.y = 0f;
        lookDir.Normalize();
        if (!checkDirs.Contains(lookDir))
            checkDirs.Add(lookDir);

        float sphereRadius = 0.3f;
        float checkDistance = 0.6f;

        foreach (var dir in checkDirs)
        {
            if (Physics.SphereCast(origin, sphereRadius, dir, out RaycastHit hit, checkDistance))
            {
                wallNormal = hit.normal;
                return true;
            }
        }

        return false;
    }

    private void UpdateAnimatorBlend()
    {
        if (_animator == null) return;

        Vector3 horizontalVelocity = _rigidbody.velocity;
        horizontalVelocity.y = 0f;

        float speed = horizontalVelocity.magnitude / maxSpeed;
        _animator.SetFloat("Blend", speed, 0.1f, Time.deltaTime); // 부드러운 블렌딩
    }
}
