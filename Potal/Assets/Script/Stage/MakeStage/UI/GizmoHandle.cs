using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;

public class GizmoHandle : MonoBehaviour
{
    Camera _camera;
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private Color highlightColor;
    [SerializeField]
    private GizmoUI gizmoUI;
    [SerializeField]
    private Axis axis;
    private GameObject selectedHandle;
    private Vector3 lastMousePos;

    private void Awake()
    {
        if (_camera == null) _camera = Camera.main;
        Renderer renderer = this.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material material = new Material(renderer.sharedMaterial);
            material.color = highlightColor;
            renderer.material = material;
        }
    }

    private void Update()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << LayerMask.NameToLayer("GizmoHandle");

            if (Physics.Raycast(ray, out RaycastHit hit, 10000f, layerMask))
            {
                selectedHandle = hit.collider.gameObject;
                lastMousePos = Input.mousePosition;
            }
        }

        if (Input.GetMouseButton(0) && selectedHandle == gameObject)
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            lastMousePos = Input.mousePosition;

            Transform targetTransform = gizmoUI.GetTarget()?.transform;
            if (targetTransform == null) return;

            Vector3 worldDir = axis switch
            {
                Axis.X => targetTransform.right,
                Axis.Y => targetTransform.up,
                Axis.Z => targetTransform.forward,
                _ => Vector3.zero
            };

            Vector3 screenPos = _camera.WorldToScreenPoint(targetTransform.position);
            Vector3 screenDir = _camera.WorldToScreenPoint(targetTransform.position + worldDir);
            Vector2 screenAxis = (screenDir - screenPos).normalized;

            float signedAmount = Vector2.Dot((Vector2)delta, screenAxis);
            gizmoUI.MoveTarget(worldDir, signedAmount * moveSpeed * Time.deltaTime);
        }



        if (Input.GetMouseButtonUp(0))
        {
            selectedHandle = null;
        }
    }
}
