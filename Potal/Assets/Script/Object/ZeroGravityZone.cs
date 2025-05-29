using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroGravityZone : MonoBehaviour
{
    [SerializeField] private float power;

    private Collider zoneCollider;
    private float maxY;
    
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        zoneCollider = GetComponent<Collider>();
        maxY = zoneCollider.bounds.max.y;

        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        meshRenderer.material.color = Color.gray;
    }

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
        float currentY = rb.position.y;
        float remainHeight = maxY - currentY;

        float clampedPower = Mathf.Clamp(power, 0f, remainHeight * 2f);
        
        Vector3 randomForce = new Vector3(
            UnityEngine.Random.Range(-2f, 2f),
            clampedPower,
            UnityEngine.Random.Range(-2f, 2f)
        );
        rb.AddForce(randomForce, ForceMode.VelocityChange);
    }
}
