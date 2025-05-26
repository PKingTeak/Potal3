using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YS_PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody _rigid;
    Vector3 moveDirection;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isGrapCube)
            {
                isGrapCube = false;
                grappedCube.transform.SetParent(null);
                grappedCube.GetComponent<Rigidbody>().isKinematic = false;
                grappedCube = null;
            }
            else
            {
                if (PerformRaycast())
                {
                    isGrapCube = true;
                    
                    grappedCube.transform.SetParent(cubePivot);
                    grappedCube.transform.localPosition = Vector3.zero;
                    grappedCube.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        moveDirection = new Vector3(horizontal, 0, vertical);
        
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    [Header("Interaction")] 
    [SerializeField] private float cubeCheckDistance;
    [SerializeField] private LayerMask cubeLayerMask;
    [SerializeField] private bool isGrapCube;
    [SerializeField] private Transform cubePivot;
    private GameObject grappedCube;
    
    private bool PerformRaycast()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, cubeCheckDistance,
                cubeLayerMask))
        {
            grappedCube = hit.transform.gameObject;
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isGrapCube ? Color.green : Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up, transform.forward * cubeCheckDistance);
    }
}
