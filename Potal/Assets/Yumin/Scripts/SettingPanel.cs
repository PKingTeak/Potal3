using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
	[SerializeField] private GameSceneUI gameSceneUI;
	[SerializeField] private SettingData settingData;

	[SerializeField] private Slider soundSlider;
	[SerializeField] private TextMeshProUGUI soundValueText; 

	[SerializeField] private Slider SFXSlider;
	[SerializeField] private TextMeshProUGUI SFXValueText;

	[SerializeField] private Slider mouseSensitivitySlider;
	[SerializeField] private TextMeshProUGUI mouseSensitivityValueText;


	[SerializeField] private Button closeButton;
	[SerializeField] private Button selectSceneButton;
	//필요 : 배경음악
	//필요 : 효과음

	private void Awake()
	{
		settingData = SettingData.Instance;
		OnSoundSliderChanged(settingData.soundVolume);
		OnSFXSliderChanged(settingData.SFXVolume);
		OnMouseSensitivitySliderChanged(settingData.lookSensitivity);

		soundSlider.onValueChanged.AddListener(OnSoundSliderChanged);
		SFXSlider.onValueChanged.AddListener(OnSFXSliderChanged);
		mouseSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivitySliderChanged);

		closeButton.onClick.AddListener(() => ExitButton());
		
		if (selectSceneButton != null)
		{
			selectSceneButton.onClick.AddListener(() => SceneManager.LoadScene("StartScene"));
		}
	}

	private void OnSoundSliderChanged(float value)
	{
		// 필요 : 배경음악.볼륨
		Debug.Log($"Sound volume changed to: {value}");
		settingData.soundVolume = value;
		soundValueText.text = $"음악 볼륨: {value*100:F0}%"; 
	}
	private void OnSFXSliderChanged(float value)
	{
		// 필요 : 효과음. 볼륨
		Debug.Log($"SFX volume changed to: {value}");
		settingData.SFXVolume = value;
		SFXValueText.text = $"효과음 볼륨: {value*100:F0}%";
	}

	private void OnMouseSensitivitySliderChanged(float value)
	{
		// 필요 : 효과음. 볼륨
		Debug.Log($"마우스 감도 mouse changed to: {value}");
		settingData.lookSensitivity = value;
		mouseSensitivityValueText.text = $"마우스 감도: {value*100:F0}"; // 소수점 둘째 자리까지 표시
	}

	private void ExitButton()
	{
		Debug.Log("닫기");
		gameObject.SetActive(false);
		if (gameSceneUI != null)
		{
			gameSceneUI.OpenUI(!gameObject.activeSelf);
		}
	}
}
