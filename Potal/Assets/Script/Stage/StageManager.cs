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

    [Header("ClearUI")]
    public GameObject clearPanel;
    
    


    public const string curStageKey = "curstage";
    public int curStage;
    
    public Vector3 RespawnPos { get { return respawnPos; } }

    private static StageManager instance;
    [SerializeField]
    private Vector3 respawnPos;
    [SerializeField]
    private float respawnTime = 1f;
    GameObject player;
    
    private GameObject playerObject;
    
   

    [SerializeField]
    private GameObject DoorConnecter;

    public void Start()
    {
       
        SpawnPlayer();
       // curStage = PlayerPrefs.GetInt(curStageKey, 0);
       
        
    }

    public void GetPlayer()
    {

        playerObject = FindObjectOfType<PlayerMovement>().gameObject;
      
        
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

    public void ClearStage()
    {//이벤트로 만들예정 엑션으로 만들고 


        // clearPanel.GetComponent<ClearPanel>().Show(); //유민님이 만드신 클리어 UI와 연동 -> 이거 미션이 없어서 뺌
        OnClearStage?.Invoke();
       LoadSceneManager.Instance.LoadSceneNormalMap("CustomMapSelectScene");
        
        Debug.Log("클리어");
       
    }
    
}
