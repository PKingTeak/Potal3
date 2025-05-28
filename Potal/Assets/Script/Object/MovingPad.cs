using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPad : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3[] moveDestination;

    private Vector3 destination;
    private Vector3 deltaPosition;
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
        MoveRidingObjects();
    }

    HashSet<Rigidbody> rigidObjects = new HashSet<Rigidbody>();
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody rigid))
        {
            rigidObjects.Add(rigid);
        }
    }
    
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody rigid))
        {
            rigidObjects.Remove(rigid);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody rigid))
        {
            rigid.MovePosition(rigid.position + deltaPosition);
        }
    }

    private void MoveToDestination()
    {
        Vector3 nextPosition = Vector3.MoveTowards(_rigid.position, destination, moveSpeed * Time.fixedDeltaTime);
        
        deltaPosition = nextPosition - _rigid.position;

        _rigid.position = nextPosition;

        if (Vector3.Distance(_rigid.position, destination) < 0.01f)
        {
            currentIndex = (currentIndex + 1) % moveDestination.Length;
            destination = moveDestination[currentIndex];
        }
    }

    private void MoveRidingObjects()
    {
        foreach (var rigid in rigidObjects)
        {
            if (rigid != null)
                rigid.MovePosition(rigid.position + deltaPosition);
        }
    }
}
