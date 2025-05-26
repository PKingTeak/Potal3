using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrabber : MonoBehaviour
{
    [Header("Grab")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform grabPoint;
    [SerializeField] private float grabRange = 3f;
    [SerializeField] private LayerMask grabLayer;

    private InteractableGrabbable _held;

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            TryGrab();
        }
        else if (context.canceled)
        {
            Release();
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