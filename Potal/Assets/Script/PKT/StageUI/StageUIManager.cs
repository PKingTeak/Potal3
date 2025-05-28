using SW;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
 
public class StageUIManager : MonoBehaviour
{
    public int CurStage { get { return curStage; } }
    public StageDataManager dataManger;
    private List<StageButton> Buttons = new List<StageButton>();

    private const string curStageKey = "curstage";
    [SerializeField]
    private int curStage;

    private void Awake()
    {
        Buttons = GetComponentsInChildren<StageButton>().ToList();
        dataManger = new StageDataManager();
        dataManger.JsonToData();
        

    }
    private void Start()
    {
        InitButtons();
    }


    public void UpdateCurStage() //버튼 인덱스
    {
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

}