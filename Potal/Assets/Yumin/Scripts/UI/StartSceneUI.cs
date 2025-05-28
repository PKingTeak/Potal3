using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

	[SerializeField] private UnityEngine.UI.Button startButton;
	[SerializeField] private UnityEngine.UI.Button mapBuildButton;
	[SerializeField] private UnityEngine.UI.Button settingButton;

	private void Start()
	{
		Utility.ButtonBind(startButton, () => SceneManager.LoadScene("MapSelectScene"));
		Utility.ButtonBind(settingButton, () => settingPanel.SetActive(true));
		Utility.ButtonBind(mapBuildButton, () => SceneManager.LoadScene("MapBuildScene"));
	}
}
