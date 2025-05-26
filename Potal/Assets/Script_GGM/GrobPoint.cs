using UnityEngine;

public class GrabPoint : MonoBehaviour
{
    [Header("Grab")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float holdDistance = 2f;
    [SerializeField] private float smoothSpeed = 10f;

    private Vector3 _velocity;

    private void LateUpdate()
    {
        Vector3 targetPos = cameraTransform.position + cameraTransform.forward * holdDistance;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, 1f / smoothSpeed);
        transform.rotation = cameraTransform.rotation;
    }
}