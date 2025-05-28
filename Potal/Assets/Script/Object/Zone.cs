using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ZoneType
{ 
    StartZone,
    EndZone,
    DeadZone
}
public class Zone : MonoBehaviour
{
  

    [Header("ZoneType")]
    [SerializeField] private ZoneType type;

    private StageManager stageManager;

    private void Start()
    {
        stageManager = FindObjectOfType<StageManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (type)
            {

                case ZoneType.EndZone:
                    stageManager.ClearStage();
                    break;

                case ZoneType.DeadZone:
                    stageManager.OnPlayerDead();
                    break;
                default:
                    break;
                    
            }
        }
        
    }
}




