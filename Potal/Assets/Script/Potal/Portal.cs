using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("Portal Settings")]
    [SerializeField]
    private Portal linkedPortal;

    [SerializeField]
    private Camera portalCamera;

    private float maxDistance = 10f;

    private List<PortalTraveller> travellers = new List<PortalTraveller>();

    [SerializeField]
    private Transform player;

    private void Update()
    {
        if (travellers.Count > 0)
        {
            CheckTravellers(); //
        }
    }

    private void LateUpdate()
    {
        UpdatePortalCamera();
    }

    private void UpdatePortalCamera()
    {
        // 플레이어와 포탈 사기 계산
        float distance = Vector3.Distance(player.position, transform.position);

        float targetFOV = Mathf.Lerp(60f, 100f, distance / maxDistance);

        // 포탈 카메라 fieldOfView 적용
        portalCamera.fieldOfView = targetFOV;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PortalTraveller>(out var traveller))
        {
            OnTravellerEnterPortal(traveller);
        }
    }

    // 포탈뒤에 있는 벽들의 Collider 모두 반환
    private List<Collider> GetWallBehindPortal()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, -transform.forward, 0.5f);

        List<Collider> colliders = new List<Collider>();

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != null)
                colliders.Add(hit.collider);
        }

        return colliders;
    }

    // traveller가 포탈에 들어왔을 때 처리
    private void OnTravellerEnterPortal(PortalTraveller traveller)
    {
        // 리스트에 없으면 추가
        if (!travellers.Contains(traveller))
            travellers.Add(traveller);

        // 클론 없으면 생성, 있으면 활성화만
        if (traveller.clone == null)
        {
            traveller.clone = Instantiate(traveller.clonePrefab);
            traveller.clone.name = traveller.name + "_Clone";
            traveller.clone.SetActive(true);
        }
        traveller.clone.SetActive(true);

        // 클론 배치
        traveller.UpdateCloneTransform(transform, linkedPortal.transform);
    }

    // 포탈 안에 있는 traveller 위치 감지, 이동 처리
    private void CheckTravellers()
    {
        for (int i = 0; i < travellers.Count; i++)
        {
            PortalTraveller traveller = travellers[i];

            // 클론 위치 계속 갱신
            if (traveller.clone != null && traveller.clone.activeSelf)
            {
                traveller.UpdateCloneTransform(transform, linkedPortal.transform);
            }

            Vector3 offset = traveller.transform.position - transform.position; // traveller 위치 계산
            float dot = Vector3.Dot(transform.forward, offset); // traveller가 앞인지 뒤인지 판별

            // 벽과 traveller의 충돌 제거
            SetWallCollision(traveller, true);
            if (dot < 0f)
            {
                // 본체를 클론 위치로 이동시키고, 상대 포탈에 클론 배치
                traveller.Teleport();
                // 포탈 도착 지점에 이동했을때 시작지점에 클론 배치
                linkedPortal.OnTravellerEnterPortal(traveller);
                if (traveller.clone != null)
                    traveller.clone.SetActive(false);
            }
        }
    }

    // traveller와 포탈 뒤쪽 벽 충돌 설정
    private void SetWallCollision(PortalTraveller traveller, bool ignore)
    {
        Collider travellerCol = traveller.GetComponent<Collider>();
        List<Collider> wallCols = GetWallBehindPortal();

        if (wallCols != null && travellerCol != null)
        {
            foreach (Collider wallCol in wallCols)
            {
                Physics.IgnoreCollision(travellerCol, wallCol, ignore);
            }
        }
    }

    // 포탈 벗어났을 시
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PortalTraveller>(out var traveller))
        {
            if (traveller.clone != null)
                traveller.clone.SetActive(false);

            travellers.Remove(traveller);

            if (travellers.Count == 0)
            {
                SetWallCollision(traveller, false);
            }
        }
    }
}
