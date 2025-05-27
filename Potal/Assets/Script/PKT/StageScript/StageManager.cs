using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("StageManager").AddComponent<StageManager>();
            }
            return instance;
        }
    }
    public Vector3 RespawnPos { get => respawnPos; set => respawnPos = value; }

    private static StageManager instance;
    [SerializeField]
    private Vector3 respawnPos;
    [SerializeField]
    private float respawnTime = 3f;
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void SpawnPlayer()
    {
        if (player == null)
        {
            Debug.Log("플레이어가 없습니다");
            return;
        }

        player.transform.position = respawnPos;



    }

    private IEnumerator RespawnDelay()
    {
        //죽음 이벤트 호출
        yield return new WaitForSeconds(respawnTime);
        SpawnPlayer();
    }

    public void OnPlayerDead()
    {
        StartCoroutine(RespawnDelay());


    }

    public void OnClearStage()
    {
        Debug.Log("클리어");
        //유민님이 만드신 클리어 UI와 연동
    }


}
