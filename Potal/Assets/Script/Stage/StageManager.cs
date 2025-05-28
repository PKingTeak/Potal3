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

    [Header("Player")]
    [SerializeField] private GameObject playerPrefab;




    private GameObject playerObject;

    public void Start()
    {
        SpawnPlayer();
    }
    public void InitRespawnPos(Vector3 pos)
    {
        respawnPos = pos;
    }

    private void SpawnPlayer()
    {
        if (player != null)
        {
            playerObject.transform.position = respawnPos;
            
        }
        playerObject = Instantiate(playerPrefab, respawnPos, Quaternion.identity);

        playerObject.tag = "Player";
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
    {//이벤트로 만들예정 엑션으로 만들고 
         clearPanel.GetComponent<ClearPanel>().Show(); //유민님이 만드신 클리어 UI와 연동
        //LoadSceneManager.Instance.LoadSceneNormalMap("MapSelectScene");가기 전에 UI틀어주기
        //UpdateCurStage(); cur
        Debug.Log("클리어");
       
    }


    public void ClearStage()
    {
      //  OnClearStage?.Invoke();
    }

}
