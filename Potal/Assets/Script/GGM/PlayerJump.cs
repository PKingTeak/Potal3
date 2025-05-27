using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerJump : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] private float jumpPower = 7f;

    private Rigidbody _rigidbody;
    private GroundChecker _groundChecker;
    private PlayerCrouch _crouch;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _groundChecker = GetComponent<GroundChecker>();
        _crouch = GetComponent<PlayerCrouch>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            TryJump();
        }
    }

    private void TryJump()
    {
        if (_groundChecker == null || !_groundChecker.IsGrounded)
            return;
        
        if (_crouch != null && _crouch.IsCrouching)
            return;

        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
        _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }
}