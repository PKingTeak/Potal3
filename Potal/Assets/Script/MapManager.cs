using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public List<MapData> mapDataList; //일단 수동으로 넣고 나중에 Json으로 로드

    //오브젝트 위치 회전 크기 저장



    public void SeletMap(string mapName)
    {
        MapData data = mapDataList.Find(m => m.mapName == mapName);
        //이제 생성해줄 클래스를 만들어야짐



    }


}
