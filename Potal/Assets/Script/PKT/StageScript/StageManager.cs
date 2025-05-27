using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public Vector3 RespawnPos { get { return respawnPos; } }

    private static StageManager instance;
    [SerializeField]
    private Vector3 respawnPos;
    [SerializeField]
    private float respawnTime = 1f;
    GameObject player;
    [Header("ClearUI")]
    public GameObject clearPanel;

  

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
       
        SpawnPlayer();
    }
    public void InitRespawnPos(Vector3 pos)
    {
        //이걸 사용해서 넣어주는중
        //startpos가 0번째에 있다고 생각중 StageUIManger에서 
        respawnPos = pos;
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
        clearPanel.GetComponent<ClearPanel>().Show();
        Debug.Log("클리어");
        //유민님이 만드신 클리어 UI와 연동
    }


}
