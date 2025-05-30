using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiddleSceneUI : MonoSingleton<MiddleSceneUI>
{
	[SerializeField] private GameObject settingPanel;
	[SerializeField] private GameSceneUI gameSceneUI;

	public GameSceneUI GameSceneUI
	{
		get { return gameSceneUI; }
		set { gameSceneUI = value; }
	}

	protected override void Awake()
	{
		base.Awake();

		if (instance == this)
		{
			DontDestroyOnLoad(gameObject);
		}
	}

	public void SettingPanelOpen()
	{
		if (gameSceneUI != null)
		{
		settingPanel.SetActive(!settingPanel.activeSelf);
			if (!settingPanel.activeSelf)
			{
				gameSceneUI.TimerData.TimeStart();
			}
			else
			{
				gameSceneUI.TimerData.TimeStop();
			}
			gameSceneUI.OpenUI(!settingPanel.activeSelf);
		}
		AudioManager.Instance.SFXSourceUIOpenPanel.Play();
	}
	public void OnSetting(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			SettingPanelOpen();
		}
	}
}
