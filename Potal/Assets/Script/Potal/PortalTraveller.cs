using UnityEngine;

public class PortalTraveller : MonoBehaviour
{
    public void Teleport(Transform fromPortal, Transform traveller, Transform toPortal)
    {
        Vector3 relativePos = fromPortal.InverseTransformPoint(traveller.position);
        Vector3 newPos = toPortal.TransformPoint(relativePos) + toPortal.forward * 0.4f;

        Quaternion relativeRot = Quaternion.Inverse(fromPortal.rotation) * traveller.rotation;
        Quaternion newRot = toPortal.transform.rotation * relativeRot;
        newRot *= Quaternion.Euler(0, 180f, 0);

        Vector3 euler = newRot.eulerAngles;
        euler.x = 0f;
        euler.z = 0f;
        newRot = Quaternion.Euler(euler);

        traveller.SetPositionAndRotation(newPos, newRot);
    }
}
