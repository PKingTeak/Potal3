using SW;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
 
public class StageManager : MonoBehaviour
{
    private List<StageButton> Buttons = new List<StageButton>();

    private const string curStageKey = "curstage";
    [SerializeField]
    private int curStage;
    public int CurStage { get { return curStage; } }


    public List<StageData> stageList = new List<StageData>(); //데이터만 가져오려고 만든것
    private Dictionary<int, StageData> stageDict; //인덱스에 맞게 그냥 불러오려고


    private void OnEnable()
    {
        InitButtons();


    }


    private void Start()
    {
        stageDict = new Dictionary<int, StageData>();
        for (int i = 0; i < stageList.Count; i++)
        {
            stageDict.Add(i + 1, stageList[i]);
        }
        //실험 데이터를 받아오면 넣어줄것이다. 

        curStage = PlayerPrefs.GetInt(curStageKey,1);
        Buttons =  GetComponentsInChildren<StageButton>().ToList();
        InitButtons();
    }

    public void UpdateCurStage() //버튼 인덱스
    {
        curStage += 1;
        PlayerPrefs.SetInt(curStageKey,curStage);
    }

    public void InitButtons()
    {
        for (int i = 0; i < Buttons.Count; i++)
        {
            Buttons[i].InitButton(i+1, this);
            //버튼 인덱스 넣어주기 
        }
        
    }


    public void SettingMap(StageData data)
    {
        foreach (var map in data.PrefabEntries)
        {
            for (int i = 0; i < data.PrefabEntries.Count; i++)
            {
                GameObject entryGO = Instantiate(data.PrefabEntries[i].Prefab);

            }
        }
    }







    public void OnSelectedClicked(int stage)
    {

       LoadSceneManager.Instance.LoadSceneAsync("TestStageScene", () => { SettingMap(stageDict[stage]); }); //이름 넣어주기



    }

}