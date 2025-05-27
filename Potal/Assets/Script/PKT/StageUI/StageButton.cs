using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    public bool isClear;
    private StageUIManager stageManger;

   
    private int index;
    private UnityEngine.UI.Button button;
    private TextMeshProUGUI buttonText;

    private void Start()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        if (TryGetComponent<UnityEngine.UI.Button>(out button))
        {
           button.onClick.AddListener(OnClickStageButton); //T씬로드
        } //이런식으로 안전하게
    }

    public void OnClickStageButton()
    {

        stageManger.OnSelectedClicked(index);
        stageManger.gameObject.SetActive(false); //임시로 끄기 
    }


    public void InitStageName(string name)
    {
        buttonText.text = name;
    }
    public void InitButton(int _index , StageUIManager _manager)
    {
        index = _index;
        stageManger = _manager;

        this.button.interactable =  index <= _manager.CurStage ? true : false; //나는 바보야~~
    }


}
