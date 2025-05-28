using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType
{
    Time,
    Jump,
    Point
}

public class Mission
{
    public MissionType type;
	public int value;
}

public class MissionData : MonoBehaviour
{
    public List<Mission> missions;
    public string SetMissionData(int index)
    {
        string str = "";

        switch (missions[index].type)
        {
            case MissionType.Time:
                str = $"시간 {missions[index].value} 안에 클리어"; 
            break;

            case MissionType.Jump:
				str = $"점프 {missions[index].value} 번 이내에 클리어";
			break;

            case MissionType.Point:
				str = $"보석 {missions[index].value} 개 획득하고 클리어";
				break;
		}
        return $"미션 {index+1} : " + str;
    }

    public int MissionClear(int index, int value)
    {
        int num = 0;
		
		if (value >= missions[index].value)
		{
			num = 1; //미션 클리어
		}
		return num;
    }
}
