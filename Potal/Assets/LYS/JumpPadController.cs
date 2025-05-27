using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadController : MonoBehaviour
{
    [SerializeField] private float _jumpPower;
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.TryGetComponent(out Rigidbody rigid))
        {
            Jump(rigid,  _jumpPower);
        }
    }
    
    private void Jump(Rigidbody rigid, float jumpPower)
    {
        rigid.velocity = new Vector3(rigid.velocity.x, 0f, rigid.velocity.z);
        rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }
}
