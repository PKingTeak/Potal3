using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagonalJumpPad : MonoBehaviour
{
    [SerializeField] private float jumpAngle = 45f;
    [SerializeField] private float jumpSpeed = 10f;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.TryGetComponent(out Rigidbody rigid))
        {
            DiagonalJump(rigid);
        }
    }
    
    private void DiagonalJump(Rigidbody rigid)
    {
        Vector3 jumpVelocity = GetJumpVelocity(jumpAngle, jumpSpeed);
        rigid.velocity = jumpVelocity;
        
        //플레이어 move()의 rigid 연결 때문에 예외 처리 수행
        if (rigid.TryGetComponent(out PlayerMovement movement))
        {
            //체공 시간 만큼 player의 move() 중단
            float flightTime = CalculateFlightTime(jumpVelocity.y);
            movement.SetJumping(flightTime);
        }
    }

    //포물선 위치/방향을 고려한 속도 계산 (포물선 공식 이용)
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

    //체공 시간 계산
    private float CalculateFlightTime(float verticalSpeed)
    {
        float gravity = Mathf.Abs(Physics.gravity.y);
        float totalTime = 2f * verticalSpeed / gravity;
        
        return totalTime;
    }
    
    // private void OnDrawGizmos()
    // {
    //     Vector3 pos = transform.position;
    //     Vector3 velocity = GetJumpVelocity(jumpAngle, jumpSpeed);
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
