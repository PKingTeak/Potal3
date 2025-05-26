using SW;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelecter : MonoBehaviour
{
    [SerializeField]
    private List<StageData> maps;
    [SerializeField]
    private List<GameObject> mapEntryPrefabs; //맵에 설치할 오브젝트들이  



    private void Start()
    {
        SettingMap();
    }


    public void SettingMap()
    {
        foreach (var map in maps)
        {
            for (int i = 0; i < mapEntryPrefabs.Count; i++)
            {
                GameObject entryGO = Instantiate(mapEntryPrefabs[i]);
             
            }
        }
    }





}

