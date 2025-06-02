using UnityEngine;
using UnityEngine.UI;

public class MainStageButton : MonoBehaviour
{
    public bool IsClear { get => isClear; set => isClear = value; }
   
    private MainStageSelecter mainStage;

    [SerializeField]
    private int index;
    private Button button;
    [SerializeField]
    private bool isClear = false;
    [SerializeField]
    private string sceneName;

    
    private void Awake()
    {
        if (TryGetComponent<Button>(out button))
        {
            button.onClick.AddListener(OnClickStageButton); //T씬로드
        }
    }


 

    public void OnClickStageButton()
    {
        Debug.Log(mainStage);
        MainStageSelecter.stageNum =  index + 1; //스테이지 넘겨주기 위해서 
        mainStage.OnSeletedClickButton(sceneName);

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
