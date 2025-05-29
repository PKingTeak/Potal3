using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float maxSpeed = 5f;                  // 최대 이동 속도
    [SerializeField] private float airControlMultiplier = 0.3f;    // 공중 제어 감쇠 계수
    private Vector2 _curMovementInput;                             // 현재 이동 입력(WASD)

    [Header("Look Settings")]
    [SerializeField] private Transform cameraContainer;            // 카메라 회전 기준이 되는 오브젝트
    [SerializeField] private float minXLook = -80f;                // 상하 회전 제한(최소)
    [SerializeField] private float maxXLook = 80f;                 // 상하 회전 제한(최대)
    [SerializeField] private SettingData settingData;              // 설정 데이터 (민감도 등)
    private float _camCurXRot;                                     // 현재 X축 회전값
    private Vector2 _mouseDelta;                                   // 마우스 입력값
    [SerializeField] private bool canLook = true;                  // 카메라 회전 가능 여부

    // 컴포넌트
    private Rigidbody _rigidbody;
    private GroundChecker _groundChecker;

    // 상태 변수
    [SerializeField] private bool isJumping = false;               // 점프 중 여부
    private bool _wasTouchingWallLastFrame = false;                // 이전 프레임에 벽에 닿았는지

    private void Start()
    {
        settingData = SettingManager.Instance.Current;
        Cursor.lockState = CursorLockMode.Locked;

        _rigidbody = GetComponent<Rigidbody>();
        _groundChecker = GetComponent<GroundChecker>();
    }

    private void FixedUpdate()
    {
        if (!isJumping)
            Move();             // 이동 처리

        SmoothLanding();        // 착지 시 충격 완화
    }

    private void LateUpdate()
    {
        if (canLook)
            CameraLook();       // 카메라 회전 처리

        // 위치 보정 (떨림 방지)
        cameraContainer.localPosition = Vector3.zero;
    }

    private void Move()
    {
        // 카메라 기준 방향 계산
        Vector3 camForward = cameraContainer.forward;
        Vector3 camRight = cameraContainer.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // 이동 방향 계산
        Vector3 moveDir = (camForward * _curMovementInput.y + camRight * _curMovementInput.x).normalized;

        // 지면/공중 감쇠
        float groundMultiplier = _groundChecker != null && _groundChecker.IsGrounded ? 1f : airControlMultiplier;

        // 웅크리기 보정
        float crouchMultiplier = 1f;
        var crouch = GetComponent<PlayerCrouch>();
        if (crouch != null)
            crouchMultiplier = crouch.SpeedMultiplier;

        // 목표 속도 계산
        Vector3 desiredVelocity = moveDir * maxSpeed * groundMultiplier * crouchMultiplier;

        // 현재 수평 속도
        Vector3 currentHorizontalVelocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
        Vector3 velocityChange = desiredVelocity - currentHorizontalVelocity;

        if (!_groundChecker.IsGrounded)
        {
            // 공중일 때 벽 충돌 감지
            if (IsTouchingWallMultiDirection(_curMovementInput, out Vector3 wallNormal))
            {
                velocityChange = Vector3.zero;

                if (!_wasTouchingWallLastFrame)
                {
                    // 처음 벽에 닿을 때만 튕겨나가듯 살짝 힘을 줌
                    _rigidbody.AddForce(wallNormal * 1.5f, ForceMode.Impulse);
                    _wasTouchingWallLastFrame = true;
                }

                // 벽에 붙은 상태에서는 수평 이동 제거
                Vector3 vel = _rigidbody.velocity;
                _rigidbody.velocity = new Vector3(0f, vel.y, 0f);
            }
            else
            {
                // 벽에 안 닿았으면 공중 이동 감쇠
                velocityChange *= 0.5f;
                _wasTouchingWallLastFrame = false;
            }
        }
        else
        {
            _wasTouchingWallLastFrame = false;
        }

        // 최종 이동력 적용
        _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    private void SmoothLanding()
    {
        // 착지 순간 수직 속도를 부드럽게 감쇠해 충격 완화
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
        // 마우스 입력으로 카메라 상하 회전
        _camCurXRot += _mouseDelta.y * settingData.lookSensitivity;
        _camCurXRot = Mathf.Clamp(_camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-_camCurXRot, 0f, 0f);

        // 좌우 회전 (캐릭터 자체 회전)
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

        // 입력 방향 기반 벽 감지
        if (moveInput.y > 0.1f) checkDirs.Add(cameraContainer.forward);
        if (moveInput.y < -0.1f) checkDirs.Add(-cameraContainer.forward);
        if (moveInput.x > 0.1f) checkDirs.Add(cameraContainer.right);
        if (moveInput.x < -0.1f) checkDirs.Add(-cameraContainer.right);

        // 추가: 시선 방향 감지 (이동이 없어도 벽 감지 가능)
        Vector3 lookDir = cameraContainer.forward;
        lookDir.y = 0f;
        lookDir.Normalize();
        if (!checkDirs.Contains(lookDir))
            checkDirs.Add(lookDir);

        // 각 방향에 대해 SphereCast 수행
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
}


// 이동, 점프 최적화, 내용이 너무 어려워서 챗지피티 내용 참고하였습니다. - GGM