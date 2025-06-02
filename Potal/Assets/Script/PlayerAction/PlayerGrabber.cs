using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrabber : MonoBehaviour
{
    [Header("Grab")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform grabPoint;
    [SerializeField] private float grabRange = 3f;
    [SerializeField] private float holdDistance = 1.5f;
    [SerializeField] private float maxGrabDistance = 3.5f;
    [SerializeField] private LayerMask grabLayer;

    private InteractableGrabbable _held;

    public bool IsHolding => _held != null;

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (IsHolding)
                Release();
            else
                TryGrab();
        }
    }

    private void Update()
    {
        if (IsHolding)
        {
            float distance = Vector3.Distance(_held.transform.position, grabPoint.position);
            if (distance > maxGrabDistance)
            {
                Release();

                if (TryGetComponent(out PlayerInteractor interactor))
                    interactor.ForceUIRefresh();
            }
        }
    }

    private void TryGrab()
    {
        if (IsHolding) return;

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, grabRange, grabLayer))
        {
            if (hit.collider.TryGetComponent(out InteractableGrabbable grabbable))
            {
                grabbable.StartGrab(grabPoint, holdDistance); // 거리 전달
                _held = grabbable;
            }
        }
    }

    private void Release()
    {
        if (!IsHolding) return;

        _held.StopGrab();
        _held = null;
    }
}