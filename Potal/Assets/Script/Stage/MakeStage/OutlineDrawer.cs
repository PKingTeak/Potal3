using UnityEngine;
using System.Collections.Generic;

public class OutlineDrawer : MonoBehaviour
{
    public GameObject targetObject;
    public Color outlineColor = Color.green;
    public float lineWidth;

    private Camera _camera;
    private Bounds combinedBounds;
    private bool isVisible = false;

    public void SetTarget(GameObject obj)
    {
        targetObject = obj;
    }

    private void Awake()
    {
        _camera = Camera.main;
        targetObject = null;
    }

    private void Update()
    {
        if (targetObject != null)
        {
            UpdateBounds();
            isVisible = IsVisibleToCamera(combinedBounds, _camera);
        }
    }

    private void OnRenderObject()
    {
        if (!isVisible || targetObject == null)
            return;

        DrawBoundsOutline(combinedBounds, outlineColor);
    }

    private void UpdateBounds()
    {
        Renderer[] renderers = targetObject.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return;

        combinedBounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            combinedBounds.Encapsulate(renderers[i].bounds);
        }
    }

    private bool IsVisibleToCamera(Bounds bounds, Camera cam)
    {
        Vector3[] points = new Vector3[8];
        Vector3 center = bounds.center;
        Vector3 extents = bounds.extents;

        points[0] = center + new Vector3(+extents.x, +extents.y, +extents.z);
        points[1] = center + new Vector3(+extents.x, +extents.y, -extents.z);
        points[2] = center + new Vector3(+extents.x, -extents.y, +extents.z);
        points[3] = center + new Vector3(+extents.x, -extents.y, -extents.z);
        points[4] = center + new Vector3(-extents.x, +extents.y, +extents.z);
        points[5] = center + new Vector3(-extents.x, +extents.y, -extents.z);
        points[6] = center + new Vector3(-extents.x, -extents.y, +extents.z);
        points[7] = center + new Vector3(-extents.x, -extents.y, -extents.z);

        foreach (var point in points)
        {
            Vector3 viewportPoint = cam.WorldToViewportPoint(point);
            if (viewportPoint.z > 0 &&
                viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                viewportPoint.y >= 0 && viewportPoint.y <= 1)
            {
                return true;
            }
        }
        return false;
    }

    private void DrawBoundsOutline(Bounds bounds, Color color)
    {
        Vector3 c = bounds.center;
        Vector3 e = bounds.extents;

        Vector3[] corners = new Vector3[8]
        {
        c + new Vector3(+e.x, +e.y, +e.z),
        c + new Vector3(+e.x, +e.y, -e.z),
        c + new Vector3(+e.x, -e.y, +e.z),
        c + new Vector3(+e.x, -e.y, -e.z),
        c + new Vector3(-e.x, +e.y, +e.z),
        c + new Vector3(-e.x, +e.y, -e.z),
        c + new Vector3(-e.x, -e.y, +e.z),
        c + new Vector3(-e.x, -e.y, -e.z)
        };

        int[][] edges = new int[][]
        {
        new[] {0, 1}, new[] {0, 2}, new[] {0, 4},
        new[] {1, 3}, new[] {1, 5},
        new[] {2, 3}, new[] {2, 6},
        new[] {3, 7},
        new[] {4, 5}, new[] {4, 6},
        new[] {5, 7},
        new[] {6, 7}
        };

        Material lineMat = GetLineMaterial();
        lineMat.SetPass(0);

        GL.PushMatrix();
        GL.LoadProjectionMatrix(_camera.projectionMatrix);
        GL.modelview = _camera.worldToCameraMatrix;

        int repeat = 5;
        float offset = lineWidth * 0.5f;

        for (int i = 0; i < repeat; i++)
        {
            float x = Mathf.Cos(2 * Mathf.PI * i / repeat) * offset;
            float y = Mathf.Sin(2 * Mathf.PI * i / repeat) * offset;
            Vector3 screenOffset = new Vector3(x, y, 0);

            GL.Begin(GL.LINES);
            GL.Color(color);

            foreach (var edge in edges)
            {
                Vector3 p1 = _camera.WorldToScreenPoint(corners[edge[0]]) + screenOffset;
                Vector3 p2 = _camera.WorldToScreenPoint(corners[edge[1]]) + screenOffset;
                Vector3 wp1 = _camera.ScreenToWorldPoint(new Vector3(p1.x, p1.y, p1.z));
                Vector3 wp2 = _camera.ScreenToWorldPoint(new Vector3(p2.x, p2.y, p2.z));

                GL.Vertex(wp1);
                GL.Vertex(wp2);
            }

            GL.End();
        }

        GL.PopMatrix();
    }


    private Material lineMaterial;
    private Material GetLineMaterial()
    {
        if (lineMaterial == null)
        {
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };

            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            lineMaterial.SetInt("_ZWrite", 0);
            lineMaterial.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
        }
        return lineMaterial;
    }
}
