using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainStageSelecter : MonoBehaviour
{

    public static int stageNum;
    public int CurStage { get { return curStage; } }


    private List<MainStageButton> mainButtons = new List<MainStageButton>();


    private const string curStageKey = "curstage";
    [SerializeField]
    private int curStage;

    private void OnEnable()
    {
        StageManager.OnClearStage += HandleStage;

    }

    private void OnDestory()
    {
        StageManager.OnClearStage -= HandleStage;

    }


    private void HandleStage()
    {

        Debug.Log("Handle호출");
        //UpdateCurStage();
        InitButtons();

    }


    private void Awake()
    {
        mainButtons = GetComponentsInChildren<MainStageButton>().ToList();
        

        curStage = PlayerPrefs.GetInt(curStageKey, 0);

    }

    private void Start()
    {
        InitButtons();

    }

 


    public void InitButtons()
    {
        for (int i = 0; i < mainButtons.Count; i++)
        {
            mainButtons[i].InitMainButton(i, this);
            if (curStage > i)
            {
                mainButtons[curStage].IsClear = true;
            }

            //버튼 인덱스 넣어주기 
        }

    }

    public void OnSeletedClickButton(string name)
    {
        LoadSceneManager.Instance.LoadSceneNormalMap(name);
    }


}
