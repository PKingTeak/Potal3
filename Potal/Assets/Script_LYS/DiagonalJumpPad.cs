using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagonalJumpPad : MonoBehaviour
{
    [SerializeField] private float launchAngle = 45f;
    [SerializeField] private float launchSpeed = 10f;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.TryGetComponent(out Rigidbody rigid))
        {
            DiagonalJump(rigid);
        }
    }
    
    private void DiagonalJump(Rigidbody rigid)
    {
        Vector3 launchVelocity = GetJumpVelocity(launchAngle, launchSpeed);
        rigid.velocity = launchVelocity;

        if (rigid.TryGetComponent(out PlayerMovement movement))
        {
            float flightTime = CalculateFlightTime(launchVelocity.y);
            movement.SetJumping(flightTime);
        }
    }

    private Vector3 GetJumpVelocity(float angleDegree, float speed)
    {
        float angleRad = angleDegree * Mathf.Deg2Rad;

        //Horizontal Forward Direction
        Vector3 forward = transform.forward;
        forward.y = 0;
        forward.Normalize();
        
        float horizontalSpeed = Mathf.Cos(angleRad) * speed;
        float verticalSpeed = Mathf.Sin(angleRad) * speed;

        Vector3 velocity = forward * horizontalSpeed;
        velocity.y = verticalSpeed;
        
        return velocity;
    }

    private float CalculateFlightTime(float verticalSpeed)
    {
        float gravity = Mathf.Abs(Physics.gravity.y);
        float totalTime = 2f * verticalSpeed / gravity;
        
        return totalTime;
    }
    
    // private void OnDrawGizmos()
    // {
    //     Vector3 pos = transform.position;
    //     Vector3 velocity = GetJumpVelocity(launchAngle, launchSpeed);
    //
    //     Gizmos.color = Color.green;
    //     for (int i = 0; i < 100; i++)
    //     {
    //         Gizmos.DrawSphere(pos, 0.1f);
    //         velocity += Physics.gravity * Time.fixedDeltaTime;
    //         pos += velocity * Time.fixedDeltaTime;
    //     }
    // }
}
