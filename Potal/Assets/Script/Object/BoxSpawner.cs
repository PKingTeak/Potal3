using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour, IIdentifiable
{
    [SerializeField] private int id;
    public int Id => id;
    
    public void SetId(int id)
    {
        this.id = id;
    }
    
    
}
