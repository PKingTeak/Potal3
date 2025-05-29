using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFire : MonoBehaviour
{
    [Header("Portal")]
    [SerializeField]
    private GameObject redPortalPrefab;

    [SerializeField]
    private GameObject bluePortalPrefab;

    [SerializeField]
    private LayerMask wallLayer;

    [SerializeField]
    private Camera playerCamera;

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
            PlacePortal(redPortalPrefab);
        }
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlacePortal(bluePortalPrefab);
        }
    }

    private void PlacePortal(GameObject portal)
    {
        // Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, wallLayer))
        {
            audioManager.SFXSourcePortalHit.Play();

            portal.transform.position = hit.point + hit.normal * 0.01f;
            portal.transform.rotation = Quaternion.LookRotation(hit.normal);

            if (!portal.activeSelf)
                portal.SetActive(true);
        }
    }
}
