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
        float lookUpDot = Vector3.Dot(transform.forward.normalized, Vector3.up);

        Vector3 euler = transform.rotation.eulerAngles;

        if (lookUpDot > 0.9f)
        {
            Vector3 forward = Vector3
                .ProjectOnPlane(transform.forward, Vector3.up)
                .normalized;

            if (forward == Vector3.zero)
                forward = Vector3.forward;

            Quaternion uprightRot = Quaternion.LookRotation(forward, Vector3.up);
            transform.rotation = uprightRot;

            euler.x = 0f;
            euler.z = 0f;

            transform.rotation = Quaternion.Euler(euler);
        }
        else if (lookUpDot < -0.9f)
        {
            Vector3 forward = Vector3
                .ProjectOnPlane(-transform.forward, Vector3.up)
                .normalized;
            if (forward == Vector3.zero)
                forward = Vector3.forward;

            Quaternion uprightRot = Quaternion.LookRotation(forward, Vector3.up);
            transform.rotation = uprightRot;
            euler.x = 0f;
            euler.z = 0f;

            transform.rotation = Quaternion.Euler(euler);
        }
        // x, z 축 기울어져 있을때 실행
        if (Mathf.Abs(euler.x) > 1f || Mathf.Abs(euler.z) > 1f)
        {
            StartCoroutine(SmoothRotation(linkedPortal));
        }
    }

    public void UpdateCloneTransform(Transform portal, Transform linkedPortal)
    {
        Vector3 relativePos = portal.InverseTransformPoint(transform.position);
        relativePos = new Vector3(-relativePos.x, relativePos.y, -relativePos.z);
        Vector3 newPos = linkedPortal.TransformPoint(relativePos);

        Quaternion newRot;

        if (Vector3.Dot(linkedPortal.forward.normalized, Vector3.down) < 0.99f)
        {
            Vector3 relativeDir = portal.InverseTransformDirection(transform.forward);
            relativeDir = new Vector3(-relativeDir.x, relativeDir.y, -relativeDir.z);
            Vector3 exitDir = linkedPortal.TransformDirection(relativeDir);

            newRot = Quaternion.LookRotation(exitDir, linkedPortal.up);
        }
        else
        {
            newRot = transform.rotation;
        }

        clone.transform.SetPositionAndRotation(newPos, newRot);
    }

    IEnumerator SmoothRotation(Transform linkedPortal)
    {
        yield return new WaitForSeconds(0.1f);

        Quaternion startRot = transform.rotation;

        Vector3 forward = linkedPortal.forward;
        forward.y = 0f;
        // 천장이나 바닥에 있는 경우
        if (forward == Vector3.zero)
            forward = linkedPortal.up;

        Quaternion targetRot = Quaternion.LookRotation(forward.normalized, Vector3.up);

        float duration = 0.3f;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        transform.rotation = targetRot;
    }
}
