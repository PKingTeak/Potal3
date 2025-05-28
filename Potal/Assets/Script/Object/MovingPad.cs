using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPad : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3[] moveDestination;

    private Vector3 destination;
    private int currentIndex;
    
    private Rigidbody _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        destination = moveDestination[currentIndex];
    }

    private void FixedUpdate()
    {
        MoveToDestination();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody rigid))
        { 
            other.transform.SetParent(this.transform);
        }
    }
    
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody rigid))
        {
            other.transform.SetParent(null);
        }
    }

    private void MoveToDestination()
    {
        Vector3 nextPosition = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.fixedDeltaTime);
        
        transform.position = nextPosition;

        if (Vector3.Distance(transform.position, destination) < 0.01f)
        {
            currentIndex = (currentIndex + 1) % moveDestination.Length;
            destination = moveDestination[currentIndex];
        }
    }
}
