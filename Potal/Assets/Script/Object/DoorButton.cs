using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class DoorButton : MonoBehaviour, IIdentifiable
{
    [SerializeField] private int id;
    public int Id => id;
    public event Action OnPressed;
    public event Action OnReleased;

    private Rigidbody current;

    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void SetId(int id)
    {
        this.id = id;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (current == null && other.transform.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            AudioManager.Instance.SFXSourceButtonDown.Play();
            current = rb;
            OnPressed?.Invoke();
            meshRenderer.material.color = Color.red;
        }
    }
    
    private void OnCollisionExit(Collision other)
    {
        if (other.transform.TryGetComponent<Rigidbody>(out Rigidbody rb) && rb == current)
        {
            OnReleased?.Invoke();
            current = null;
            meshRenderer.material.color = Color.white;
        }
    }
}
