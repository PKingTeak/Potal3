using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vector3Data
{
    public float x;
    public float y;
    public float z;


    public Vector3Data(Vector3 vec)
    {
        x = vec.x;
        y = vec.y;
        z = vec.z;
    }

    public Vector3 ToVector3() => new Vector3(x, y, z);
}


[System.Serializable]
public class MapData 
{
    public string mapName;
    public Vector3Data playerStartPos;
    public Vector3Data clearPos;

    public MapData(string name, Vector3 startPos, Vector3 EndPos)
    {
        mapName = name;
        playerStartPos =  new Vector3Data(startPos);
        clearPos = new Vector3Data(EndPos);
    }

}


