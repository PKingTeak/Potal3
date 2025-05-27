using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
	[SerializeField] private StartSceneUI startSceneUI;

	[SerializeField] private Slider soundSlider;
	[SerializeField] private Slider SFXSlider;
	[SerializeField] private UnityEngine.UI.Button closeButton;
	//필요 : 배경음악
	//필요 : 효과음

	private void Awake()
	{
		soundSlider.onValueChanged.AddListener(OnSoundSliderChanged);
		SFXSlider.onValueChanged.AddListener(OnSFXSliderChanged);
		closeButton.onClick.AddListener(() => startSceneUI.SetActivePanel(UIState.Main));
	}

	private void OnSoundSliderChanged(float value)
	{
		// 필요 : 배경음악.볼륨
		Debug.Log($"Sound volume changed to: {value}");
	}
	private void OnSFXSliderChanged(float value)
	{
		// 필요 : 효과음. 볼륨
		Debug.Log($"SFX volume changed to: {value}");
	}
}
