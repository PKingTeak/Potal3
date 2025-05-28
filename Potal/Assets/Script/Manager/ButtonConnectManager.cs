using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonConnectManager : MonoBehaviour
{
    private void Start()
    {
        MatchButtonDoor();
    }
    private void MatchButtonDoor()
    {
        DoorButton[] buttons = FindObjectsOfType<DoorButton>();
        Door[] doors  = FindObjectsOfType<Door>();
    
        Dictionary<int, List<DoorButton>> buttonMap = new Dictionary<int, List<DoorButton>>();
        Dictionary<int, List<Door>> doorMap = new Dictionary<int, List<Door>>();
        
        //Button 캐싱
        foreach (var button in buttons)
        {
            if (!buttonMap.ContainsKey(button.Id))
                buttonMap[button.Id] = new List<DoorButton>();
            buttonMap[button.Id].Add(button);
        }
        //Door 캐싱
        foreach (var door in doors)
        {
            if (!doorMap.ContainsKey(door.Id))
                doorMap[door.Id] = new List<Door>();
            doorMap[door.Id].Add(door);
        }

        //연결
        foreach (var id in buttonMap.Keys)
        {
            if (doorMap.TryGetValue(id, out var matchedDoors))
            {
                foreach (var button in buttonMap[id])
                {
                    foreach (var door in matchedDoors)
                    {
                        button.OnPressed += door.Open;
                        button.OnReleased += door.Close;
                        Debug.Log($"[Connected] Button:{button.Id} → Door:{door.Id}");
                    }
                }
            }
        }
    }

    private void MatchSpawnerButton()
    {
        SpawnerButton[] spawnerButtons = FindObjectsOfType<SpawnerButton>();
        BoxSpawner[] boxSpawners = FindObjectsOfType<BoxSpawner>();
        
        Dictionary<int, SpawnerButton> buttonMap = new Dictionary<int, SpawnerButton>();
        Dictionary<int, BoxSpawner> spawnerMap = new Dictionary<int, BoxSpawner>();
        
        
    }
}
