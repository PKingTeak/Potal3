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

    
    private Vector3 _positionOffset;
    private Quaternion _rotationOffset;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _rb.freezeRotation = true;
    }

    public void StartGrab(Transform holder)
    {
        _holder = holder;
        _isHeld = true;
        
        _positionOffset = Quaternion.Inverse(_holder.rotation) * (transform.position - _holder.position);
        _rotationOffset = Quaternion.Inverse(_holder.rotation) * transform.rotation;

        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _rb.angularVelocity = Vector3.zero;
    }

    public void StopGrab()
    {
        _isHeld = false;
        _rb.constraints = RigidbodyConstraints.None;

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
            // 목표 위치 및 회전 계산
            Vector3 targetPos = _holder.position + _holder.rotation * _positionOffset;
            Quaternion targetRot = _holder.rotation * _rotationOffset;

            // 부드럽게 이동
            Vector3 dir = (targetPos - transform.position);
            _rb.velocity = dir * followSpeed;

            _rb.angularVelocity = Vector3.zero;

            Quaternion smoothedRot = Quaternion.Lerp(_rb.rotation, targetRot, Time.fixedDeltaTime * followSpeed);
            _rb.MoveRotation(smoothedRot);
        }
    }

    public bool IsHeld => _isHeld;

    public void Interact()
    {
        Debug.Log("잡을 수 있는 오브젝트와 상호작용 시도됨");
    }

    public bool CanShowUI()
    {
        return !_isHeld;
    }
}
