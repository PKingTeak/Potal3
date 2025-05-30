using UnityEngine;

public class GizmoRenderer : MonoBehaviour
{
    public Color gizmoColor = Color.yellow;
    public Vector3 boxSize = Vector3.one;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}