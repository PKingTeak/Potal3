using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadFloor : MonoBehaviour
{
    private MeshRenderer[] meshRenderers;
    private void Start()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in meshRenderers)
        {
            renderer.material.color = Color.red;
        }
    }
}
