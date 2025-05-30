using System.Collections;
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
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("PortalClone"))
                return;

            if (((1 << hit.collider.gameObject.layer) & wallLayer.value) == 0)
                return;

            audioManager.SFXSourcePortalHit.Play();

            RepositionAndActivate(portal, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }

    private void RepositionAndActivate(GameObject portal, Vector3 pos, Quaternion rot)
    {
        Animator animator = portal.GetComponent<Animator>();

        if (portal.activeSelf)
        {
            animator.ResetTrigger("Off");
            animator.SetTrigger("Off");

            StartCoroutine(MoveAfterDisappear(portal, pos + rot * Vector3.forward * 0.01f, rot));
        }
        else
        {
            portal.transform.SetPositionAndRotation(pos + rot * Vector3.forward * 0.01f, rot);
            portal.SetActive(true);
        }
    }

    private IEnumerator MoveAfterDisappear(GameObject portal, Vector3 newPos, Quaternion newRot)
    {
        yield return new WaitForSeconds(0.25f);

        portal.SetActive(false);
        portal.transform.SetPositionAndRotation(newPos, newRot);
        portal.SetActive(true);
    }
}
