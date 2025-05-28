using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerButton : MonoBehaviour, IIdentifiable
{
    [SerializeField] private int id;
    public int Id => id;
    
    public event Action OnPressed;
    
    public void SetId(int id)
    {
        this.id = id;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            OnPressed?.Invoke();
        }
    }
}
