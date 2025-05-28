using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("Portal Settings")]
    public Transform viewPortal;
    public Portal linkedPortal;
    public MeshRenderer screen;
    public Camera portalCamera;

    private float maxDistance = 10f;

    private List<PortalTraveller> travellers = new List<PortalTraveller>();

    [SerializeField]
    private Transform player;

    void Start()
    {
        portalCamera.enabled = true;
    }

    void LateUpdate()
    {
        UpdatePortalCamera();
    }

    void UpdatePortalCamera()
    {
        float distance = Vector3.Distance(player.position, viewPortal.position); // 플레이어와 포탈 사기 계산

        float targetFOV = Mathf.Lerp(60f, 100f, distance / maxDistance);

        portalCamera.fieldOfView = targetFOV; // 포탈 카메라 fieldOfView 적용
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PortalTraveller>(out var traveller))
        {
            Debug.Log("포탈에 뭔가 들어온게 감지");
            OnTravellerEnterPortal(traveller);
            foreach (var t in travellers)
            {
                Debug.Log("현재 들어온 것들" + t.name);
            }
        }
    }

    public void OnTravellerEnterPortal(PortalTraveller traveller)
    {
        if (!travellers.Contains(traveller))
        {
            travellers.Add(traveller);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("test"))
            return;

        for (int i = 0; i < travellers.Count; i++)
        {
            PortalTraveller traveller = travellers[i];
            Vector3 offset = traveller.transform.position - transform.position; // 이동 오브젝트 위치 계산
            float dot = Vector3.Dot(transform.forward, offset); // 오브젝트가 앞인지 뒤인지 판별
            // Debug.Log(dot); // dot확인 디버그
            if (dot < 0.5f)
            {
                traveller.Teleport(transform, traveller.transform, linkedPortal.transform);
                linkedPortal.OnTravellerEnterPortal(traveller);
                travellers.RemoveAt(i);
                i--;
            }
        }

        // if (dot < 0.5f)
        // {
        //     // 현재 포탈 기준으로 플레이어 위치 로컬 좌표로 변환
        //     Vector3 relativePos = transform.InverseTransformPoint(player.position);
        //     // 반대쪽 포탈 공간으로 위치로 이동, 0.6정도 더 이동
        //     Vector3 newPos =
        //         linkedPortal.transform.TransformPoint(relativePos)
        //         + linkedPortal.transform.forward * 0.4f;

        //     // 현재 포탈 기준으로 플레이어의 회전값 계산
        //     Quaternion relativeRot = Quaternion.Inverse(transform.rotation) * player.rotation;
        //     // 이동한 포탈 기준으로 플레이어의 회전값 계산
        //     Quaternion newRot = linkedPortal.transform.rotation * relativeRot;

        //     // 방향 보정(포탈 뒤에서 나오니까)
        //     newRot *= Quaternion.Euler(0, 180f, 0);

        //     // 플레이어 위치값과 회전값 적용
        //     player.SetPositionAndRotation(newPos, newRot);
        // }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        // 플레이어가 포탈에서 나갔을 때 다시 포탈 사용가능하게 설정F
    }
}
