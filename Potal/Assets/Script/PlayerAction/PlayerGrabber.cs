using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrabber : MonoBehaviour
{
    [Header("Grab")]
    [SerializeField] private Transform cameraTransform;       
    [SerializeField] private Transform grabPoint;             
    [SerializeField] private float grabRange = 3f;            
    [SerializeField] private LayerMask grabLayer;             
    [SerializeField] private float maxGrabDistance = 3.5f;    

    private InteractableGrabbable _held;                      
    private bool _isGrabbing = false;                         
    
    // 외부에서 현재 잡고 있는지 확인용
    public bool IsHolding => _held != null;

    public void OnGrab(InputAction.CallbackContext context)
    {
        // E 키 눌렀을 때
        if (context.performed)
        {
            // 토글 처리: 잡고 있으면 놓고, 아니면 잡기 시도
            if (_isGrabbing)
                Release();
            else
                TryGrab();

            _isGrabbing = !_isGrabbing;
        }
    }

    private void Update()
    {
        // 잡은 상태에서 거리가 너무 멀어지면 놓기
        if (_held != null)
        {
            float distance = Vector3.Distance(_held.transform.position, grabPoint.position);
            if (distance > maxGrabDistance)
            {
                Release();
                _isGrabbing = false;
            }
        }
    }

    private void TryGrab()
    {
        if (_held != null) return;

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, grabRange, grabLayer))
        {
            if (hit.collider.TryGetComponent(out InteractableGrabbable grabbable))
            {
                grabbable.StartGrab(grabPoint);
                _held = grabbable;
            }
        }
    }

    private void Release()
    {
        if (_held != null)
        {
            _held.StopGrab();
            _held = null;
        }
    }
}