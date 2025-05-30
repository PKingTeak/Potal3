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

    private void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>(); 
	}

    private Coroutine _typingCoroutine;
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
            settingPanel.SetActive(!settingPanel.activeSelf);
            OpenUI(!settingPanel.activeSelf);
		}
    }

    public void OpenUI(bool isOpen)
    {
        Cursor.lockState = isOpen == true ? CursorLockMode.Locked : CursorLockMode.None;
        playerInput.enabled = isOpen; // 플레이어 입력 비활성화
	}
    
    public void ShowPromptById(string id)
    {
	    PromptData data = PromptDatabase.Instance.GetPrompt(id);
	    if (data == null) return;

	    promptText.text = data.message;
	    promptText.maxVisibleCharacters = 0;
	    promptText.gameObject.SetActive(true);

	    if (_typingCoroutine != null)
		    StopCoroutine(_typingCoroutine);
	    _typingCoroutine = StartCoroutine(TypePrompt(data.message, data.duration));
    }

    private IEnumerator TypePrompt(string message, float duration)
    {
	    promptText.text = message;
	    promptText.ForceMeshUpdate();

	    int totalChars = promptText.textInfo.characterCount;
	    for (int i = 0; i <= totalChars; i++)
	    {
		    promptText.maxVisibleCharacters = i;
		    yield return new WaitForSeconds(0.03f);
	    }

	    yield return new WaitForSeconds(duration);
	    promptText.gameObject.SetActive(false);
    }
}
