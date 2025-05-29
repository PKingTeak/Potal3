using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour, IIdentifiable
{
    [SerializeField] private int id;
    public int Id => id;

    [SerializeField] private GameObject spawnPivot;
    [SerializeField] private GameObject spawnPrefab;
    public void SetId(int id)
    {
        this.id = id;
    }

    public void Spawn()
    {
        GameObject go = Instantiate(spawnPrefab, null);
        go.transform.position = spawnPivot.transform.position;
    }
}
