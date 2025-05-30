using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    public bool isClear;
    private StageUIManager stageUIManger;

    [SerializeField]
    private int index;
    private Button button;


    private void Start()
    {
      
        if (TryGetComponent<UnityEngine.UI.Button>(out button))
        {
           button.onClick.AddListener(OnClickStageButton); //T씬로드
        } //이런식으로 안전하게
    }

    public void OnClickStageButton()
    {

        stageUIManger.OnSelectedClicked(index);
        stageUIManger.gameObject.SetActive(false); //임시로 끄기 
    }


   
    public void InitButton(int _index , StageUIManager _manager)
    {
        index = _index;
        stageUIManger = _manager;

        this.button.interactable =  index <= stageUIManger.CurStage ? true : false; //현재 인덱스 가 CurStage보다 작으면 클릭가능 이외는 불가능
    }


}
