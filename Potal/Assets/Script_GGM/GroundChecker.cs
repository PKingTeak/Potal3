using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool IsGrounded { get; private set; }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                // "지면"과 닿았는지 판정
                IsGrounded = true;
                return;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        IsGrounded = false;
    }
}