using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadController : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.TryGetComponent(out Rigidbody rb))
        {
            //Rigidbody를 써서 cube도 보낼 수 있게 할 것인지?
            //아니면 PlayerController를 써서 플레이어만 뛸 수 있게 할 것인지?
            //Jump 로직~
        }
    }
}
