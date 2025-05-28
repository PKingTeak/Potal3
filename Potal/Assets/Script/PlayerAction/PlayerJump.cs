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
    private PlayerMovement _movement;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _groundChecker = GetComponent<GroundChecker>();
        _crouch = GetComponent<PlayerCrouch>();
        _movement = GetComponent<PlayerMovement>();
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

        Vector3 newVelocity = _rigidbody.velocity;
        newVelocity.y = 0f;
        _rigidbody.velocity = newVelocity;

        _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

        AudioManager.Instance.SFXSourceJump.Play();

        if (_movement != null)
            _movement.SetJumping(0.2f); // 점프 후 Move() 일시 중지
    }

    public void Jump(float padJumpPower)
    {
        Vector3 newVelocity = _rigidbody.velocity;
        newVelocity.y = 0f;
        _rigidbody.velocity = newVelocity;

        _rigidbody.AddForce(Vector3.up * (jumpPower + padJumpPower), ForceMode.Impulse);

        if (_movement != null)
            _movement.SetJumping(0.2f);
    }
}