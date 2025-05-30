using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    public bool IsClear { get => isClear; set => isClear = value; }
    private StageUIManager stageUIManger;
    private MainStageSelecter mainStage;

    [SerializeField]
    private int index;
    private Button button;
    [SerializeField]
    private bool isClear = false;

    

  

    private void Start()
    {
        if (TryGetComponent<Button>(out button))
        {
            button.onClick.AddListener(OnClickStageButton); //T씬로드
        } //이런식으로 안전하게
    }


    public void OnClickStageButton()
    {
        stageUIManger.OnSelectedClicked(index);
       
    }


   
    public void InitButton(int _index , StageUIManager _manager)
    {
        index = _index;
        stageUIManger = _manager;

        if (button == null)
        {
            button = GetComponent<Button>();
        }

        this.button.interactable =  index <= stageUIManger.CurStage ? true : false; //현재 인덱스 가 CurStage보다 작으면 클릭가능 이외는 불가능
    }


    public void InitMainButton(int _index, MainStageSelecter mainSelect)
    {
        index = _index;
        mainStage = mainSelect;

        if (button == null)
        {
            button = GetComponent<Button>();
        }
        this.button.interactable = index <= mainSelect.CurStage ? true : false;
    }

}
