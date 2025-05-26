using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InteractableGrabbable : MonoBehaviour
{
    private bool _isHeld = false;
    private Rigidbody _rb;
    private Transform _holder;

    [Header("Auto Release Settings")]
    [SerializeField] private float maxGrabDistance = 3.5f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    public void StartGrab(Transform holder)
    {
        _holder = holder;
        _isHeld = true;
    }

    public void StopGrab()
    {
        _isHeld = false;
        _holder = null;
    }

    private void FixedUpdate()
    {
        if (_isHeld && _holder != null)
        {
            // 거리 검사 (자동 놓기)
            float distance = Vector3.Distance(transform.position, _holder.position);
            if (distance > maxGrabDistance)
            {
                StopGrab();
                return;
            }

            // 부드럽게 따라오기
            Vector3 dir = (_holder.position - transform.position);
            _rb.velocity = dir * 20f;

            _rb.angularVelocity = Vector3.zero;
        }
    }

    public bool IsHeld => _isHeld;
}