using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSceneUI : MonoBehaviour
{
    [Header("잡담 표시")]
    [SerializeField] private TextMeshProUGUI promptText;
    
    [Header("상호작용 시 표시")]
    [SerializeField] private GameObject interactPanel;
    [SerializeField] private TextMeshProUGUI keyText;
	[SerializeField] private TextMeshProUGUI actionNameText;

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
}
