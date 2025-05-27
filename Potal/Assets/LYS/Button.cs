using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Button : MonoBehaviour, IIdentifiable
{
    [SerializeField] private string id;
    public string Id => id;
    public event Action OnPressed;
    public event Action OnReleased;

    private Rigidbody current;

    private void Awake()
    {
        id = ExtractInstanceIndex(gameObject.name);
    }

    private string ExtractInstanceIndex(string id)
    {
        Match match = Regex.Match(name, @"\((\d+)\)");
        if (match.Success)
            return match.Groups[1].Value;
        return "0";
    }
    
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
