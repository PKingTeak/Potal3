using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoorPair : MonoBehaviour
{
    public ButtonController button;
    public DoorController door;

    private void Awake()
    {
        button.OnPressed += door.Open;
        button.OnReleased += door.Close;
    }
}
