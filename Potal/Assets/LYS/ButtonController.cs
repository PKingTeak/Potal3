using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private Rigidbody currentRigidbody;
    public GameObject door;
    
    private void OnCollisionEnter(Collision other)
    {
        if (currentRigidbody == null && other.transform.TryGetComponent<Rigidbody>(out Rigidbody rigid))
        {
            currentRigidbody = rigid;
            Debug.Log($"Button Click: {rigid.name}");
            door.GetComponent<Animator>().SetBool("IsOpen", true);
        }
    }
    
    private void OnCollisionExit(Collision other)
    {
        Rigidbody _what;
        if (other.transform.TryGetComponent<Rigidbody>(out Rigidbody exitingRigid) && exitingRigid == currentRigidbody)
        {
            Debug.Log($"Button Released by: {exitingRigid.name}");
            door.GetComponent<Animator>().SetBool("IsOpen", false);
            currentRigidbody = null;
        }
    }
    
    
}
