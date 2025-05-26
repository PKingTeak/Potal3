using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapDataToJson : MonoBehaviour
{

    public MapData data;
    

    public void ParseToJson()
    {
        
        Vector3 startpos = data.playerStartPos.ToVector3();
        Vector3 clearPos = data.playerStartPos.ToVector3();

        string json = JsonUtility.ToJson(data, true);
        string path = Application.dataPath + "/Resources/mapData.json";
        File.WriteAllText(path, json);


        Debug.Log("맵 데이터를 JSON으로 저장 완료!\n" + json);



    }


    private void Start()
    {
        ParseToJson();
    }
        

}
