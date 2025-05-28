using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private GameSceneUI gameSceneUI;
    [SerializeField] private PlayerGrabber grabber;

    private IInteractable _currentTarget;
    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        // 잡고 있는 상태면 UI 안 뜨게 막기
        if (grabber != null && grabber.IsHolding)
        {
            _currentTarget = null;
            gameSceneUI.GetInteractData(); // UI 비활성화
            return;
        }

        if (TryGetInteractable(out IInteractable interactable, out RaycastHit hit))
        {
            if (interactable.CanShowUI())
            {
                _currentTarget = interactable;
                string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
                gameSceneUI.GetInteractData(layerName); // UI 활성화
            }
            else
            {
                _currentTarget = null;
                gameSceneUI.GetInteractData(); // UI 비활성화
            }
        }
        else
        {
            _currentTarget = null;
            gameSceneUI.GetInteractData(); // UI 비활성화
        }
    }

    public void OnUse(InputAction.CallbackContext context)
    {
        // E 키 눌렀고, 잡고 있는 상태가 아닐 때만 상호작용 허용
        if (context.performed && !grabber.IsHolding)
        {
            if (TryGetInteractable(out IInteractable interactable, out _))
            {
                interactable.Interact();
            }
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
