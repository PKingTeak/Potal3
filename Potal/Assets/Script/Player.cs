using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private LayerMask groundLayerMask;

    [SerializeField]
    private float staminaCostOnJump;
    public float moveSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;

    [Header("Look")]
    [SerializeField]
    private Transform cameraContainer;

    [SerializeField]
    private float minXLook;

    [SerializeField]
    private float maxXLook;

    [SerializeField]
    private float lookSensitivity;
    private bool canLook = true;
    private float camCurXRot;
    private Vector2 mouseDelta;

    private Rigidbody rb;

    [SerializeField]
    private LayerMask portalSurfaceLayer;

    [SerializeField]
    private GameObject bluePortal;

    [SerializeField]
    private GameObject orangePortal;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 고정
    }

    void Update()
    {
        Move(); // 일반 이동
    }

    void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rb.velocity.y; // y값 유지

        rb.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    // public void OnBluePortal(InputAction.CallbackContext context)
    // {
    //     if (context.phase == InputActionPhase.Performed)
    //     {
    //         ShootPortal(bluePortal);
    //     }
    // }

    // public void OnOrangePortal(InputAction.CallbackContext context)
    // {
    //     if (context.phase == InputActionPhase.Performed)
    //     {
    //         ShootPortal(orangePortal);
    //     }
    // }

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
