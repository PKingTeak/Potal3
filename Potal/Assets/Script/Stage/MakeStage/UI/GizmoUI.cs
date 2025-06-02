using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public enum GizmoHandleType
{
    Position = 0,
    Rotation = 1,
    Scale = 2,
    Total = 3
}
public class GizmoUI : MonoBehaviour
{
    [SerializeField] private GameObject positionHandle;
    [SerializeField] private GameObject rotationHandle;
    [SerializeField] private GameObject scaleHandle;
    [SerializeField] private Transform rootTransform;
    private GameObject targetObject;
    private GameObject[] handles;
    private GizmoHandleType selectedHandleType;

    private void Awake()
    {
        handles = new GameObject[(int)GizmoHandleType.Total];
        handles[(int)GizmoHandleType.Position] = positionHandle;
        handles[(int)GizmoHandleType.Rotation] = rotationHandle;
        handles[(int)GizmoHandleType.Scale] = scaleHandle;
    }

    private void Start()
    {
        selectedHandleType = GizmoHandleType.Total;
    }

    public void SetTarget(GameObject target)
    {
        targetObject = target;
        UpdateRootTransform();
        if (targetObject == null)
        {
            HideAllHandles();
            return;
        }
        if (selectedHandleType != GizmoHandleType.Total)
        {
            SetRendererAndColliderEnabled(handles[(int)selectedHandleType], true);
        }
    }

    public GameObject GetTarget()
    {
        return targetObject;
    }

    public void MoveTarget(Vector3 worldDirection, float amount)
    {
        if (targetObject == null) return;

        targetObject.transform.position += worldDirection * amount;
        rootTransform.position = targetObject.transform.position;
        rootTransform.rotation = targetObject.transform.rotation;

        UpdateRootTransform();
    }

    public void ScaleTarget(Axis axis, float amount)
    {
        if (targetObject == null) return;

        Vector3 scale = targetObject.transform.localScale;

        scale.x += axis == Axis.X ? amount : 0;
        scale.x = Mathf.Max(scale.x, 0f);
        scale.y += axis == Axis.Y ? amount : 0;
        scale.y = Mathf.Max(scale.y, 0f);
        scale.z += axis == Axis.Z ? amount : 0;
        scale.z = Mathf.Max(scale.z, 0f);
        targetObject.transform.localScale = scale;

        UpdateRootTransform();
    }

    public void RotateTarget(Axis axis, float amount)
    {
        if (targetObject == null) return;

        Vector3 euler = targetObject.transform.eulerAngles;

        euler.x += axis == Axis.X ? amount : 0;
        euler.y += axis == Axis.Y ? amount : 0;
        euler.z += axis == Axis.Z ? amount : 0;
        targetObject.transform.eulerAngles = euler;

        UpdateRootTransform();
    }

    private void UpdateRootTransform()
    {
        if (targetObject == null) return;
        rootTransform.position = targetObject.transform.position;
        rootTransform.rotation = targetObject.transform.rotation;
    }

    public void ShowOnly(GizmoHandleType type)
    {
        selectedHandleType = type;
        if (targetObject == null) return;
        for (int i = 0; i < (int)GizmoHandleType.Total; i++)
        {
            SetRendererAndColliderEnabled(handles[i], i == (int)type);
        }
    }

    public void HideAllHandles()
    {
        for (int i = 0; i < (int)GizmoHandleType.Total; i++)
        {
            SetRendererAndColliderEnabled(handles[i], false);
        }
    }

    public static void SetRendererAndColliderEnabled(GameObject obj, bool enabled)
    {
        if (obj == null) return;

        var renderer = obj.GetComponent<Renderer>();
        if (renderer != null) renderer.enabled = enabled;

        var collider = obj.GetComponent<Collider>();
        if (collider != null) collider.enabled = enabled;

        foreach (Transform child in obj.transform)
        {
            SetRendererAndColliderEnabled(child.gameObject, enabled);
        }
    }

}
