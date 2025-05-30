using System;
using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour
{

    public static event Action OnClearStage; //클리어시

    private void Awake()
    {
        StageSettingHelper.onCompleted += GetPlayer;
    }

    


    public const string curStageKey = "curstage";
    public int curStage;


    public Vector3 RespawnPos { get { return respawnPos; } }

    private static StageManager instance;
    [SerializeField]
    private Vector3 respawnPos;
    [SerializeField]
    private float respawnTime = 1f;
    [SerializeField] GameObject player;

    [SerializeField] private GameSceneUI gameSceneUI;




    [SerializeField]
    private GameObject DoorConnecter;

    public void Start()
    {

        Invoke("FindPlayer",1f);
        curStage = PlayerPrefs.GetInt(curStageKey, 0);
        

    }

    public void GetPlayer()
    {
        if (player == null)
        {
            Debug.Log("플레이어가 없습니다");
            //맵에서 로드 되어야함
            return;
        }
        player = FindObjectOfType<PlayerMovement>().gameObject;
       // SettingSpawnPos();

    }


    private void FindPlayer()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
        SettingPos();
        GetPlayer();

    }

  
    public void InitRespawnPos(Vector3 pos)
    {
        respawnPos = pos;
    }

    public void SettingSpawnPos()
    {
        respawnPos = player.transform.position;

    }

    private void SettingPos()
    {
        if (player != null)
        {
            player.transform.position = respawnPos;
            
        }

    }


    private IEnumerator RespawnDelay()
    {
        //죽음 이벤트 호출
        yield return new WaitForSeconds(respawnTime);
        SettingPos();
    }

    public void OnPlayerDead()
    {
        StartCoroutine(RespawnDelay());
    }



    public void ClearStage()
    {

        gameSceneUI.ClearPanelOpen();
                    

        if (MainStageSelecter.stageNum <= curStage)
        {
            return;
        }
        else
        {
            curStage += 1;
            PlayerPrefs.SetInt(curStageKey, curStage);
        }


            Debug.Log("클리어");
       
    }
    
}
