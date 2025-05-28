using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroGravityZone : MonoBehaviour
{
    [SerializeField] private float power;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.useGravity = false;
            rb.drag = 3f;
            rb.angularDrag = 1f;
            AddRandomForce(rb);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.useGravity = true;
            rb.drag = 0f;
            rb.angularDrag = 0.05f;
        }
    }
    
    void AddRandomForce(Rigidbody rb)
    {
        Vector3 randomForce = new Vector3(
            UnityEngine.Random.Range(-2f, 2f),
            power,
            UnityEngine.Random.Range(-2f, 2f)
        );
        rb.AddForce(randomForce, ForceMode.VelocityChange);
    }
}
