using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private LayerMask portalSurfaceLayer;

    [SerializeField]
    private GameObject bluePortal;

    [SerializeField]
    private GameObject orangePortal;

    public void OnBluePortal(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ShootPortal(bluePortal);
        }
    }

    public void OnOrangePortal(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ShootPortal(orangePortal);
        }
    }

    void ShootPortal(GameObject portal)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, portalSurfaceLayer))
        {
            portal.transform.position = hit.point + hit.normal * 0.01f;
            portal.transform.rotation = Quaternion.LookRotation(hit.normal);

            if (!portal.activeSelf)
                portal.SetActive(true);
        }
    }
}
