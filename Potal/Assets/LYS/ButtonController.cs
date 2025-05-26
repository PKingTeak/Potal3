using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public event Action OnPressed;
    public event Action OnReleased;

    private Rigidbody current;
    
    private void OnCollisionEnter(Collision other)
    {
        if (current == null && other.transform.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            current = rb;
            OnPressed?.Invoke();
        }
    }
    
    private void OnCollisionExit(Collision other)
    {
        if (other.transform.TryGetComponent<Rigidbody>(out Rigidbody rb) && rb == current)
        {
            OnReleased?.Invoke();
            current = null;
        }
    }
}
