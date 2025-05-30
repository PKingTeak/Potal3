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
    private PlayerGrabber _grabber;
    private string _lastLayerName = null;

    private void Awake()
    {
        _grabber = GetComponent<PlayerGrabber>();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_grabber != null && _grabber.IsHolding)
        {
            if (_lastLayerName != null)
            {
                _lastLayerName = null;
                gameSceneUI.GetInteractData();
            }
            _currentTarget = null;
            return;
        }

        if (TryGetInteractable(out IInteractable interactable, out RaycastHit hit))
        {
            if (interactable.CanShowUI())
            {
                string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
                if (_lastLayerName != layerName)
                {
                    _lastLayerName = layerName;
                    gameSceneUI.GetInteractData(layerName);
                }
                _currentTarget = interactable;
                return;
            }
        }

        if (_lastLayerName != null)
        {
            _lastLayerName = null;
            gameSceneUI.GetInteractData();
        }

        _currentTarget = null;
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

    public void ForceUIRefresh()
    {
        _lastLayerName = null;
        _currentTarget = null;
        UpdateUI();
    }
}
