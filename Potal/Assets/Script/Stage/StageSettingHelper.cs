using SW;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSettingHelper : MonoBehaviour
{

    public static event Action onCompleted;


    public StageDataManager dataManager;

    private void Awake()
    {
        dataManager = new StageDataManager();
        dataManager.JsonToData();
                      
    }


    public void SettingMap(StageData data)
    { 
        foreach(var map in data.PrefabEntries)
        {
            GameObject go = Resources.Load<GameObject>($"Prefabs/RealStagePrefab/{map.prefabPath}");
            GameObject entryGo = Instantiate(go);
            go.transform.position = map.position;
        }

        onCompleted?.Invoke();
    }

    public void FindmapIndex(int stage)
    {
        SettingMap( dataManager.GetStageData(stage));
    }

   
    
}
