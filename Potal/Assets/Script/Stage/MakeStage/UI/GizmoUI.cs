using UnityEngine;

public class GizmoUI : MonoBehaviour
{
    public enum GizmoHandleType
    {
        Position = 0,
        Rotation = 1,
        Scale = 2,
        Total = 3
    }

    [SerializeField] private GameObject positionGroup;
    [SerializeField] private GameObject rotationGroup;
    [SerializeField] private GameObject scaleGroup;
    [SerializeField] private Transform rootTransform;
    private GameObject targetObject;
    private readonly GameObject[] handleGroups = new GameObject[(int)GizmoHandleType.Total];

    private void Awake()
    {
        handleGroups[(int)GizmoHandleType.Position] = positionGroup;
        handleGroups[(int)GizmoHandleType.Rotation] = rotationGroup;
        handleGroups[(int)GizmoHandleType.Scale] = scaleGroup;
    }

    public void SetTarget(GameObject target)
    {
        targetObject = target;
        UpdateRootTransform();
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
    }


    private void UpdateRootTransform()
    {
        if (targetObject == null) return;

        rootTransform.position = targetObject.transform.position;
        rootTransform.rotation = targetObject.transform.rotation;
    }

    public void ShowOnly(GizmoHandleType type)
    {
        for (int i = 0; i < (int)GizmoHandleType.Total; i++)
        {
            handleGroups[i]?.SetActive(i == (int)type);
        }
    }

    public void SetHandleActive(GizmoHandleType type, bool active)
    {
        handleGroups[(int)type]?.SetActive(active);
    }

    public void HideAllHandles()
    {
        for (int i = 0; i < (int)GizmoHandleType.Total; i++)
        {
            handleGroups[i]?.SetActive(false);
        }
    }
}
