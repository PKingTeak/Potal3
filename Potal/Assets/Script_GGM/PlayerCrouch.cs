using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CapsuleCollider))]
public class PlayerCrouch : MonoBehaviour
{
    [Header("Crouch Settings")]
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float standHeight = 2f;
    [SerializeField] private float crouchSpeedMultiplier = 0.5f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float cameraCrouchOffset = -0.5f;

    private CapsuleCollider _collider;
    private float _originalCameraY;
    private bool _isCrouching = false;
    private bool _isCeilingBlocked = false;

    public bool IsCrouching => _isCrouching;
    public float SpeedMultiplier => _isCrouching ? crouchSpeedMultiplier : 1f;

    private void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
        _originalCameraY = cameraTransform.localPosition.y;
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SetCrouch(true);  // 누르자마자 앉기
        }
        else if (context.canceled)
        {
            TryUnCrouch();    // 뗐을 때 일어서기 시도
        }
    }

    private void SetCrouch(bool state)
    {
        _isCrouching = state;
        _collider.height = state ? crouchHeight : standHeight;

        Vector3 camPos = cameraTransform.localPosition;
        camPos.y = _originalCameraY + (state ? cameraCrouchOffset : 0f);
        cameraTransform.localPosition = camPos;
    }

    private void TryUnCrouch()
    {
        if (_isCeilingBlocked)
            return;

        SetCrouch(false);
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            // 머리 위쪽에서 누르고 있는 표면이 있다면
            if (Vector3.Dot(contact.normal, Vector3.down) > 0.5f)
            {
                _isCeilingBlocked = true;
                return;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // 충돌 종료 시 막힘 해제
        _isCeilingBlocked = false;
    }
}
