using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFire : MonoBehaviour
{
    [Header("Portal")]
    [SerializeField] private GameObject redPortalPrefab;
    [SerializeField] private GameObject bluePortalPrefab;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Camera playerCamera;

    private GameObject _currentRedPortal;
    private GameObject _currentBluePortal;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.Instance;
	}

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlacePortal(redPortalPrefab, ref _currentRedPortal);
            audioManager.SFXSourcePortalShoot.Play();
		}
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlacePortal(bluePortalPrefab, ref _currentBluePortal);
			audioManager.SFXSourcePortalShoot.Play();
		}
    }

    private void PlacePortal(GameObject portalPrefab, ref GameObject currentPortal)
    {
        Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, wallLayer))
        {
            audioManager.SFXSourcePortalHit.Play();
            Vector3 hitPoint = hit.point;
            Vector3 normal = hit.normal;

            // 기존 포탈 제거
            if (currentPortal != null)
            {
                Destroy(currentPortal);
            }

            // 포탈 생성
            Quaternion rotation = Quaternion.LookRotation(-normal);
            Vector3 offset = hitPoint + normal * 0.01f;

            currentPortal = Instantiate(portalPrefab, offset, rotation);
        }
    }
}