using UnityEngine;

public class PortalTraveller : MonoBehaviour
{
    public GameObject clonePrefab; // 포탈 통고할때 생성되는 클론 프리팹
    public GameObject clone; // Traveller의 클론

    // 포탈을 통과했을 때 호출되는 메서드
    public void Teleport(Transform portal, Transform linkedPortal)
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        // 진입 방향이 앞인지 판단
        bool isForwardEntry = rb.velocity.z > 0f;

        // 포탈의 방향 체크
        bool isEntryPortalUp = Vector3.Dot(portal.forward.normalized, Vector3.up) > 0.9f;
        bool isExitPortalSide =
            Mathf.Abs(Vector3.Dot(linkedPortal.forward.normalized, Vector3.up)) < 0.1f;

        if (clone == null)
            return;

        // 이동한  클론으로부터 이동 위치와 회전값 설정 받아옴
        Vector3 newPos = clone.transform.position + linkedPortal.forward * 0.5f;
        Quaternion newRot = clone.transform.rotation;

        if (rb != null)
        {
            Vector3 relVel = portal.InverseTransformDirection(rb.velocity);
            relVel = new Vector3(-relVel.x, relVel.y, -relVel.z);
            rb.velocity = linkedPortal.TransformDirection(relVel);
        }

        Vector3 euler = newRot.eulerAngles;

        // 기울어져 있을 경우
        if (Mathf.Abs(euler.x) > 1f || Mathf.Abs(euler.z) > 1f)
        {
            Vector3 forward = Vector3
                .ProjectOnPlane(newRot * Vector3.forward, Vector3.up)
                .normalized;

            if (forward == Vector3.zero)
                forward = linkedPortal.forward;
                
            float y = newRot.eulerAngles.y;

            newRot = Quaternion.Euler(0f, y, 0f);

            // 바닥포탈에서 벽면 포탈로 이동할 경우
            if (isEntryPortalUp && isExitPortalSide)
            {
                if (isForwardEntry)
                {
                    newRot = Quaternion.LookRotation(linkedPortal.forward, Vector3.up);
                }
                else
                {
                    newRot = Quaternion.LookRotation(-linkedPortal.forward, Vector3.up);
                }
            }
        }
        transform.SetPositionAndRotation(newPos, newRot);
    }

    // 클론의 위치와 회전을 갱신
    public void UpdateCloneTransform(Transform portal, Transform linkedPortal)
    {
        // 클론 위치 설정
        Vector3 relativePos = portal.InverseTransformPoint(transform.position);
        relativePos = new Vector3(-relativePos.x, relativePos.y, -relativePos.z);
        Vector3 newPos = linkedPortal.TransformPoint(relativePos);

        // 클론회전 설정
        Quaternion relativeRot = Quaternion.Inverse(portal.rotation) * transform.rotation;
        Quaternion newRot = linkedPortal.rotation * relativeRot;

        newRot = Quaternion.AngleAxis(180f, linkedPortal.up) * newRot;

        clone.transform.SetPositionAndRotation(newPos, newRot);
    }
}
