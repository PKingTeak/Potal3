using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpPower = 7f;

    private Rigidbody _rigidbody;
    private GroundChecker _groundChecker;
    private PlayerCrouch _crouch;
    private PlayerMovement _movement;
    private Animator _animator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _groundChecker = GetComponent<GroundChecker>();
        _crouch = GetComponent<PlayerCrouch>();
        _movement = GetComponent<PlayerMovement>();
        _animator = GetComponent<Animator>();
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

        _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

        AudioManager.Instance?.SFXSourceJump?.Play();

        if (_movement != null)
            _movement.SetJumping(0.2f);

        if (_animator != null)
            _animator.SetTrigger("Jump");
    }

    public void Jump(float padJumpPower)
    {
        _rigidbody.AddForce(Vector3.up * (jumpPower + padJumpPower), ForceMode.Impulse);

        if (_movement != null)
            _movement.SetJumping(0.2f);

        if (_animator != null)
            _animator.SetTrigger("Jump");
    }
}