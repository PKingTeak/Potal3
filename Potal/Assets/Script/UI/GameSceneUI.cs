using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameSceneUI : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput; 
												//플레이어 isMove 값 필요. 
	[Header("잡담 표시")]
    [SerializeField] private TextMeshProUGUI promptText;
    
    [Header("상호작용 시 표시")]
    [SerializeField] private GameObject interactPanel;
    [SerializeField] private TextMeshProUGUI keyText;
	[SerializeField] private TextMeshProUGUI actionNameText;

    [Header("패널 On, Off")]
    [SerializeField] private GameObject diePanel; // 죽으면 표시
    [SerializeField] private GameObject clearPanel; // 끝 부분에 도달하면 표시
    [SerializeField] private GameObject settingPanel;

    public void GetInteractData(string tag = "None")
    {
        switch (tag)
        {
            case "None":
				interactPanel.SetActive(false);
                break;

			case "Interactable":
				interactPanel.SetActive(true);
				keyText.text = "E";
				actionNameText.text = "상호작용";
				break;
		}
    }

    public void OnSetting(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            settingPanel.SetActive(true);
            OpenUI(!settingPanel.activeSelf);
		}
    }

    public void OpenUI(bool isOpen)
    {
        Cursor.lockState = isOpen == true ? CursorLockMode.Locked : CursorLockMode.None;
        playerInput.enabled = isOpen; // 플레이어 입력 비활성화
	}
}
