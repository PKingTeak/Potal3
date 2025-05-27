using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoorConnector : MonoBehaviour
{
    private readonly Dictionary<string, ButtonController> _buttonMap = new Dictionary<string, ButtonController>();
    private readonly Dictionary<string, DoorController> _doorMap = new Dictionary<string, DoorController>();
    private readonly Dictionary<string, (ButtonController, DoorController)> _activeLinks = new Dictionary<string, (ButtonController, DoorController)>();
    
    private void Start()
    {
        MatchButtonDoor();
    }
    private void MatchButtonDoor()
    {
        ButtonController[] buttons = FindObjectsOfType<ButtonController>();
        DoorController[] doors  = FindObjectsOfType<DoorController>();
        
        //Button 캐싱
        foreach (var button in buttons)
        {
            if (!_buttonMap.ContainsKey(button.Id))
                _buttonMap[button.Id] = button;
        }
        //Door 캐싱
        foreach (var door in doors)
        {
            if (!_doorMap.ContainsKey(door.Id))
                _doorMap[door.Id] = door;
        }

        //연결
        foreach (var id in _buttonMap.Keys)
        {
            if (_activeLinks.ContainsKey(id)) continue;

            if (_doorMap.TryGetValue(id, out var door) && _buttonMap.TryGetValue(id, out var button))
            {
                button.OnPressed += door.Open;
                button.OnReleased += door.Close;
                
                _activeLinks[id] = (button, door);
                
                Debug.Log($"[Connected] Button:{id} → Door:{id}");
            }
        }
    }
    
    //특정 id의 버튼과 도어 연결 해제
    public void Unregister(string id)
    {
        if (_activeLinks.TryGetValue(id, out var pair))
        {
            pair.Item1.OnPressed -= pair.Item2.Open;
            pair.Item1.OnReleased -= pair.Item2.Close;
            
            _activeLinks.Remove(id);
        }

        _buttonMap.Remove(id);
        _doorMap.Remove(id);
    }
    
    //완전히 초기화 후 전부 다시 연결
    public void RematchAll()
    {
        ClearAll();
        MatchButtonDoor();
    }

    public void ClearAll()
    {
        foreach (var (button, door) in _activeLinks.Values)
        {
            button.OnPressed -= door.Open;
            button.OnReleased -= door.Close;
        }
        
        _activeLinks.Clear();
        _buttonMap.Clear();
        _doorMap.Clear();
    }
}
