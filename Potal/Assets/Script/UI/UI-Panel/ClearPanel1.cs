using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClearPanel1 : MonoBehaviour
{
	[SerializeField] private TimerData timerData;
	[SerializeField] private TextMeshProUGUI timeText;
	[SerializeField] private Button goToMainButton;
	//필요 : 배경음악
	//필요 : 효과음

	private void Awake()
	{
		Utility.ButtonBind(goToMainButton, () => SceneManager.LoadScene("StartScene"));
		timeText.text = timerData.TimeStop();
		Time.timeScale = 0f;
	}

	//경택 추가함
	public void Show()
	{

	}
}
