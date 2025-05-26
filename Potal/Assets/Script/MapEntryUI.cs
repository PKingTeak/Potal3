using SW;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MapEntryUI : MonoBehaviour
{
    [Header("UIReference ")]
    public TextMeshProUGUI mapNameText;
    public TextMeshProUGUI startPointText;
    public TextMeshProUGUI clearPointText;
    public Button selectButton;

   // [SerializeField]
    //private StageData mapData;

    [SerializeField]
    private MapSelecter curMap;
    

    public void Initialize(StageData data)
    {
       //mapData =  data;

        mapNameText.text = data.name;
        if (data.PrefabEntries != null && data.PrefabEntries.Count >= 2)
        {
            Vector3 startPos = data.PrefabEntries[0].position;
            Vector3 clearPos = data.PrefabEntries[1].position;

            //화면에 표시 일단 어디가 시작지점인지 끝점인지
            startPointText.text = string.Format("시작: {0:F1}, {1:F1}, {2:F1}", startPos.x, startPos.y, startPos.z);
            clearPointText.text = string.Format("클리어: {0:F1}, {1:F1}, {2:F1}", clearPos.x, clearPos.y, clearPos.z);
            //테스트용
        }
        else
        {
            startPointText.text = null;
            clearPointText.text = null;
        }



        
    }


    public void OnSelectedClicked()
    {

        curMap.SettingMap();

        
    }
   
}
