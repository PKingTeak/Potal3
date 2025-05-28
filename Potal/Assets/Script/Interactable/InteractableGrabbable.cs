using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InteractableGrabbable : MonoBehaviour, IInteractable
{
    private bool _isHeld = false;
    private Rigidbody _rb;
    private Transform _holder;

    [Header("Grab Settings")]
    [SerializeField] private float followSpeed = 20f;
    [SerializeField] private float releasePushStrength = 0.5f;
    [SerializeField] private float grabDistance = 1.5f;

    [Header("Layer Change Settings")]
    [SerializeField] private LayerMask heldLayer;     // 잡은 상태에서의 레이어 (점프 불가)
    [SerializeField] private LayerMask defaultLayer;  // 원래 레이어 (점프 가능)

    private Quaternion _rotationOffset;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _rb.freezeRotation = true;

        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    public void StartGrab(Transform holder)
    {
        _holder = holder;
        _isHeld = true;

        _rotationOffset = Quaternion.Inverse(_holder.rotation) * transform.rotation;

        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _rb.angularVelocity = Vector3.zero;

        // 점프 방지 레이어로 변경
        gameObject.layer = LayerMaskToIndex(heldLayer);
    }

    public void StopGrab()
    {
        _isHeld = false;
        _rb.constraints = RigidbodyConstraints.None;

        // 원래 레이어로 복구
        gameObject.layer = LayerMaskToIndex(defaultLayer);

        if (_holder != null)
        {
            Vector3 pushDir = (_holder.forward + _holder.up * 0.5f).normalized;
            _rb.velocity += pushDir * releasePushStrength;
        }

        _holder = null;
    }

    private void FixedUpdate()
    {
        if (_isHeld && _holder != null)
        {
            Vector3 targetPos = _holder.position + _holder.forward * grabDistance;
            Quaternion targetRot = _holder.rotation * _rotationOffset;

            Vector3 dir = (targetPos - transform.position);
            _rb.velocity = dir * followSpeed;
            _rb.angularVelocity = Vector3.zero;

            Quaternion smoothedRot = Quaternion.Lerp(_rb.rotation, targetRot, Time.fixedDeltaTime * followSpeed);
            _rb.MoveRotation(smoothedRot);
        }
    }

    public bool IsHeld => _isHeld;

    public void Interact() { }

    public bool CanShowUI() => !_isHeld;

    // LayerMask에서 단일 레이어 index 추출
    private int LayerMaskToIndex(LayerMask mask)
    {
        int value = mask.value;
        for (int i = 0; i < 32; i++)
        {
            if ((value & (1 << i)) != 0)
                return i;
        }
        return 0;
    }
}
