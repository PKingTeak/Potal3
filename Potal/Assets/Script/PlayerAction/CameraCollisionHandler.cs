using UnityEngine;

public class FirstPersonCameraCollision : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;      // 플레이어 머리 기준
    [SerializeField] private Transform cameraTransform;      // 실제 카메라 위치 (자식 오브젝트)

    [Header("Settings")]
    [SerializeField] private float cameraDistance = 0.5f;     // 기본 거리
    [SerializeField] private float minDistance = 0.1f;        // 최소 거리
    [SerializeField] private float collisionBuffer = 0.05f;   // 벽 여유
    [SerializeField] private float sphereRadius = 0.2f;
    [SerializeField] private LayerMask collisionMask;

    private void LateUpdate()
    {
        Vector3 origin = playerTransform.position + Vector3.up * 1.0f; // 머리 위치 기준
        Vector3 camDir = (cameraTransform.position - origin).normalized;

        float desiredDistance = cameraDistance;
        float adjustedDistance = desiredDistance;

        // 뒤로 cast 해서 충돌 체크
        if (Physics.SphereCast(origin, sphereRadius, camDir, out RaycastHit hit, desiredDistance + collisionBuffer, collisionMask))
        {
            adjustedDistance = Mathf.Clamp(hit.distance - collisionBuffer, minDistance, desiredDistance);
        }

        // 카메라를 머리 기준 뒤쪽으로 배치
        cameraTransform.position = origin + camDir * adjustedDistance;
    }
}