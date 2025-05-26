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
    [SerializeField]
    private Transform contentParent;


    private void Start()
    {
       
    }


    public void SettingMap()
    {
        foreach (var map in maps)
        {
            for (int i = 0; i < mapEntryPrefabs.Count; i++)
            {
                GameObject entryGO = Instantiate(mapEntryPrefabs[i], contentParent);
                MapEntryUI entryUI = entryGO.GetComponent<MapEntryUI>();
                //UI등록해주기 

                if (entryUI != null)
                {
                    entryUI.Initialize(map);
                }
                else
                {
                    Debug.LogWarning("mapEntryPrefab에 MapEntryUI 컴포넌트가 없습니다.");
                }
            }
        }
    }





}

