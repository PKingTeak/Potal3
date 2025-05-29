using SW;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSettingHelper : MonoBehaviour
{

    public static event Action onCompleted;


    public StageDataLoader dataLoader;

    private int buttonid;
    private int doorid;

    private void Awake()
    {
        dataLoader = new StageDataLoader();
        dataLoader.JsonToData();
        
    }


    public void SettingMap(StageData data)
    { 
        foreach(var map in data.PrefabEntries)
        {
            GameObject go = Resources.Load<GameObject>($"Prefabs/RealStagePrefab/{map.prefabPath}");
            Vector3 goPosition = map.position;
            Quaternion goRotaion = Quaternion.Euler( map.rotation);

            GameObject entryGo = Instantiate(go,goPosition, goRotaion);
            if (map.prefabPath == "Button")
            {
                entryGo.GetComponent<DoorButton>().SetId(buttonid);
                //짝지어주는거 조금 고민해야될듯
                //이건 데이터에 부여해야 될듯하다. 
                buttonid++;
            }
            else if (map.prefabPath == "ButtonDoor")
            {

                entryGo.GetComponent<Door>().SetId(doorid);
                doorid++;
            }
            
            //버튼 연결은 추후에 해야한다. 모든 오브젝트가 달리고 버튼을 셋팅해줘야한다. 
        }


        onCompleted?.Invoke();
    }

    public void FindmapIndex(int stage)
    {
        SettingMap(dataLoader.GetStageData(stage));
    }

   
    
}
