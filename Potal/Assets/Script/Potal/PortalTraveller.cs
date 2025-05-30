using System.Collections;
using UnityEngine;

public class PortalTraveller : MonoBehaviour
{
    public GameObject clonePrefab; // 포탈 통고할때 생성되는 클론 프리팹
    public GameObject clone; // Traveller의 클론

    public void Teleport(Transform portal, Transform linkedPortal)
    {
        if (clone == null)
            return;

        // 클론의 위치와 회전값을 본체 위치로 설정, 약간 튀어나가게 함
        Vector3 newPos = clone.transform.position + linkedPortal.transform.forward * 0.5f;
        Quaternion newRot = clone.transform.rotation;

        transform.SetPositionAndRotation(newPos, newRot);

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Debug.Log("포탈들어갈떄 속도: " + rb.velocity);
            Vector3 relativeVel = portal.InverseTransformDirection(rb.velocity);
            // z축만 반전
            relativeVel.z = -relativeVel.z;

            Vector3 newVelocity = linkedPortal.TransformDirection(relativeVel);
            newVelocity *= 1.2f;

            // 최대 속도 제한
            if (newVelocity.magnitude > 20f)
                newVelocity = newVelocity.normalized * 20f;

            rb.velocity = newVelocity;
            Debug.Log("포탈나갈때 속도: " + rb.velocity);
        }
        // x, z 축 기울어져 있을때 실행
        Vector3 euler = transform.rotation.eulerAngles;
        if (Mathf.Abs(euler.x) > 1f || Mathf.Abs(euler.z) > 1f)
        {
            StartCoroutine(SmoothRotation(linkedPortal));
        }
    }

    IEnumerator SmoothRotation(Transform linkedPortal)
    {
        yield return new WaitForSeconds(0.3f); // 안정화 시간

        Quaternion startRot = transform.rotation;

        Vector3 forward = linkedPortal.forward;
        forward.y = 0f;
        // 천장이나 바닥에 있는 경우
        if (forward == Vector3.zero)
            forward = linkedPortal.up; // 포탈이 바닥/천장일 경우 대체

        // 해당 방향 바라보도록 회전 설정
        Quaternion targetRot = Quaternion.LookRotation(forward.normalized, Vector3.up);

        float duration = 0.5f;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        transform.rotation = targetRot;
    }

    public void UpdateCloneTransform(Transform portal, Transform linkedPortal)
    {
        // 포탈과 traveller의 상대 거리 계산
        Vector3 relativePos = portal.InverseTransformPoint(transform.position);
        // 포탈 기준 traveller와 반대방향, 포탈에서 나오는 느낌을 주게 함
        relativePos = new Vector3(-relativePos.x, relativePos.y, -relativePos.z);
        Vector3 newPos = linkedPortal.TransformPoint(relativePos);

        Quaternion newRot;

        // 벨로시티 y가 클 경우 포탈 forward를 기준으로 회전
        Rigidbody rb = GetComponent<Rigidbody>();
        if (Mathf.Abs(rb.velocity.y) > 0.1f)
        {
            Vector3 forward = linkedPortal.forward;
            forward.y = 0;
            // 천장이나 바닥에 있는 경우
            if (forward == Vector3.zero)
                forward = linkedPortal.up;

            // 해당 방향 바라보도록 회전 설정
            newRot = Quaternion.LookRotation(forward.normalized, Vector3.up);
        }
        else
        {
            // 상대 회전 적용 후 반전)
            Quaternion relativeRot = Quaternion.Inverse(portal.rotation) * transform.rotation;
            newRot = linkedPortal.rotation * relativeRot;
            newRot *= Quaternion.Euler(0, 180f, 0);
        }

        // 클론 위치와 회전 적용
        clone.transform.SetPositionAndRotation(newPos, newRot);
    }
}
