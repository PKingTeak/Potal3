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

        private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (type)
            {

                case ZoneType.EndZone:
                    StageManager.Instance.OnClearStage();
                    break;

                case ZoneType.DeadZone:
                    StageManager.Instance.OnPlayerDead();
                    break;
                default:
                    break;
                    
            }
        }
        
    }
}




