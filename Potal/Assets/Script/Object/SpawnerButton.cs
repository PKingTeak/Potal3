using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerButton : MonoBehaviour, IIdentifiable
{
    [SerializeField] private int id;
    public int Id => id;
    
    public event Action OnPressed;

    [SerializeField] private bool isPressed;

    private MeshRenderer meshRenderer;
    
    private void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        isPressed = false;
    }

    public void SetId(int id)
    {
        this.id = id;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (!isPressed && other.transform.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            OnPressed?.Invoke();
            isPressed = true;
            meshRenderer.material.color = Color.red;
        }
    }
}
