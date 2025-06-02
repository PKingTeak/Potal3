using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum UIState
{
	Main,
	Setting,
	MapBuild
}

public class StartSceneUI : MonoBehaviour
{
	[SerializeField] private UIState currentState = UIState.Main;

	[SerializeField] private GameObject mainPanel;
	[SerializeField] private GameObject settingPanel;
	[SerializeField] private GameObject mapBuildPanel;

	[SerializeField] private Button startButton;
	[SerializeField] private Button mapBuildButton;
	[SerializeField] private Button mapSelectButton;
	[SerializeField] private Button settingButton;

	private void Start()
	{
		Utility.ButtonBind(startButton, () => SceneManager.LoadScene("MapSelectScene"));
		Utility.ButtonBind(mapBuildButton, () => SceneManager.LoadScene("MakeStageScene"));
		Utility.ButtonBind(mapSelectButton, () => SceneManager.LoadScene("CustomMapSelectScene"));
		Utility.ButtonBind(settingButton, () => MiddleSceneUI.Instance.SettingPanelOpen());
	}
}
