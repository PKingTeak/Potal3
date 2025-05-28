using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private GameSceneUI gameSceneUI;

    private IInteractable _currentTarget;

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (TryGetInteractable(out IInteractable interactable, out RaycastHit hit))
        {
            if (!interactable.CanShowUI())
            {
                ClearUI();
                return;
            }

            _currentTarget = interactable;
            string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
            gameSceneUI.GetInteractData(layerName);
        }
        else
        {
            ClearUI();
        }
    }

    private void ClearUI()
    {
        if (_currentTarget != null)
        {
            _currentTarget = null;
            gameSceneUI.GetInteractData();
        }
    }

    public void OnUse(InputAction.CallbackContext context)
    {
        if (context.performed && TryGetInteractable(out IInteractable interactable, out _))
        {
            interactable.Interact();
        }
    }

    private bool TryGetInteractable(out IInteractable interactable, out RaycastHit hit)
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(ray, out hit, interactRange, interactLayer))
        {
            return hit.collider.TryGetComponent(out interactable);
        }

        interactable = null;
        return false;
    }
}