using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InteractableGrabbable : MonoBehaviour, IInteractable
{
    private bool _isHeld = false;
    private Rigidbody _rb;
    private Transform _holder;

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
            Vector3 dir = (_holder.position - transform.position);
            _rb.velocity = dir * 20f;
            _rb.angularVelocity = Vector3.zero;
        }
    }

    public bool IsHeld => _isHeld;

    public void Interact()
    {
        Debug.Log("잡을 수 있는 오브젝트와 상호작용 시도됨");
    }
}