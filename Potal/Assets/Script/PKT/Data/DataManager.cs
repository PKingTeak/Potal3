using SW;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;


public class DataManager 
{
        
    List<StageData> datas = new List<StageData>();
    Dictionary<int, StageData> dataDict = new Dictionary<int, StageData>();

    public void JsonToData()
    {

       string json =  Resources.Load<TextAsset>("Json/StageData/Stage02").text;



        //뭘해겠어? 텍스트를 이제 구분지어서 리스트로 담아야겠지??
        //하나만 가져올꺼야
        StageData data =JsonUtility.FromJson<StageData>(json);
        //값을 가져옴
        datas.Add(data);
        for (int i = 0; i < datas.Count; i++)
        {
            dataDict.Add(i,datas[i]); //캐싱
        
        }

    }

    public StageData GetStageData(int key)
    {
        if (dataDict[key] != null)
        {
            return dataDict[key];
        }
        else 
        {
            return null;
        }
    }
  


        
}
