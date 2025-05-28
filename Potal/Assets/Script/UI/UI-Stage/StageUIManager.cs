using SW;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageUIManager : MonoBehaviour
{

    public int CurStage { get { return curStage; } }
    public StageDataManager dataManger;
    private List<StageButton> Buttons = new List<StageButton>();

    private const string curStageKey = "curstage";
    [SerializeField]
    private int curStage;
    [SerializeField]
    private GameObject stageUI;
   




    private void Awake()
    {
        Buttons = GetComponentsInChildren<StageButton>().ToList();
        dataManger = new StageDataManager();
        dataManger.JsonToData();

        curStage = PlayerPrefs.GetInt(curStageKey, 0);

    }
    private void Start()
    {
        
        InitButtons();
    }


    private void OnEnable()
    {
        StageManager.OnClearStage += HandleStage;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        StageManager.OnClearStage -= HandleStage;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "CustomMapSelectScene")
        {
            this.gameObject.SetActive(true);
            stageUI.gameObject.SetActive(true);
        }
    }

    private void HandleStage()
    {

        Debug.Log("Handle호출");
        UpdateCurStage();
        InitButtons();
        
    }
    public void UpdateCurStage() //버튼 인덱스
    {
        Debug.Log("점수 업데이트");
        curStage += 1;
        PlayerPrefs.SetInt(curStageKey,curStage);
        //해당 스테이지 클리어시 호출해줘야함
    }

    public void InitButtons()
    {
        for (int i = 0; i < Buttons.Count; i++)
        {
            Buttons[i].InitButton(i, this);
           
            //버튼 인덱스 넣어주기 
        }
        
    }

    public void SettingMap(StageData data)
    {
       
       foreach (var map in data.PrefabEntries)
       {


                GameObject gameObject = Resources.Load<GameObject>($"Prefabs/MakeStagePrefab/{map.prefabPath}"); //일단 넣기
                GameObject entryGO = Instantiate(gameObject);
                entryGO.transform.position = map.position; //위치넣어주기
                
                //일단 0번째는 start라고 생각하고있음
           
       }
    }




    public void OnSelectedClicked(int stage)
    {

       LoadSceneManager.Instance.LoadSceneAsync("TestStageScene", () => { SettingMap(dataManger.GetStageData(stage));}); //이름 넣어주기



    }

    // 저희 각자맵 -> 선택 

    //선우님이 맵에디로 생성된 맵 -> 선택 =>

    //선우님 맵 표시 별개로 저희가 만든 맵 로드 

}