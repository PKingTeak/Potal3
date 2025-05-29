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

        // 클론의 위치와 회전값을 본체 위치로 설정
        Vector3 newPos = clone.transform.position;
        Quaternion newRot = clone.transform.rotation;

        // 기울어진 상태로 나오는 거 방지
        Vector3 euler = newRot.eulerAngles;
        euler.x = 0f;
        euler.z = 0f;
        newRot = Quaternion.Euler(euler);

        // 본체 위치와 회전 적용
        transform.SetPositionAndRotation(newPos, newRot);

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Debug.Log("포탈들어갈떄 속도: " + rb.velocity);
            Vector3 relativeVel = portal.InverseTransformDirection(rb.velocity);
            Vector3 newVelocity = linkedPortal.TransformDirection(relativeVel);
            rb.velocity = -newVelocity * 1.2f;
            Debug.Log("포탈나갈때 속도: " + rb.velocity);
        // StartCoroutine(ApplyVelocityAfterDelay(rb, -newVelocity * 1.2f));
        }
    }

    // private IEnumerator ApplyVelocityAfterDelay(Rigidbody rb, Vector3 newVelocity)
    // {
    //     yield return new WaitForFixedUpdate(); // 한 프레임 쉬고 적용
    //     rb.velocity = newVelocity;
    // }

    public void UpdateCloneTransform(Transform portal, Transform linkedPortal)
    {
        // 포탈과 traveller의 상대 거리 계산
        Vector3 relativePos = portal.InverseTransformPoint(transform.position);
        // 포탈 기준 traveller와 반대방향, 포탈에서 나오는 느낌을 주게 함
        relativePos = new Vector3(-relativePos.x, relativePos.y, -relativePos.z);
        Vector3 newPos = linkedPortal.TransformPoint(relativePos);

        // 포탈과 traveller의 상대 회전 계산
        Quaternion relativeRot = Quaternion.Inverse(portal.rotation) * transform.rotation;
        Quaternion newRot = linkedPortal.rotation * relativeRot;

        // 회전 반전
        newRot *= Quaternion.Euler(0, 180f, 0); // 반전으로 포탈에서 나오는 방향으로 보이게 함

        // 클론 위치와 회전 적용
        clone.transform.SetPositionAndRotation(newPos, newRot);
    }
}
