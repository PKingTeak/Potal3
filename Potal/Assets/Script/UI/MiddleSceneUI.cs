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
		settingPanel.SetActive(!settingPanel.activeSelf);
		var data = SettingManager.Instance.Current;
		SettingManager.Instance.SaveSettings(data.soundVolume, data.SFXVolume, data.lookSensitivity);

		if (gameSceneUI != null)
		{
			if (!settingPanel.activeSelf)
			{

				gameSceneUI.TimerData.TimeStart();
				Time.timeScale = 1f;
			}
			else
			{
				gameSceneUI.TimerData.TimeStop();
				Time.timeScale = 0f;
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
