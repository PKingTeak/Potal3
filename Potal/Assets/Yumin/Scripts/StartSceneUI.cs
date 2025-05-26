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

	[SerializeField] private Button startButton;
	[SerializeField] private Button mapBuildButton;
	[SerializeField] private Button settingButton;

	private void Awake()
	{
		SetActivePanel(currentState);
		startButton.onClick.AddListener(() => SceneManager.LoadScene("MapSelectScene"));
		settingButton.onClick.AddListener(() => SetActivePanel(UIState.Setting));
		mapBuildButton.onClick.AddListener(() => SceneManager.LoadScene("MapSelectScene"));
	}

	public void SetActivePanel(UIState setState)
	{
		currentState = setState;
		mainPanel.SetActive(setState == UIState.Main);
		settingPanel.SetActive(setState == UIState.Setting);
		//mapBuildPanel.SetActive(setState == UIState.MapBuild);
	}
}
