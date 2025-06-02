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

	private void Start()
	{
		settingData = SettingManager.Instance.Current;
		OnSoundSliderChanged(settingData.soundVolume);
		OnSFXSliderChanged(settingData.SFXVolume);
		OnMouseSensitivitySliderChanged(settingData.lookSensitivity);
		soundSlider.value = settingData.soundVolume;
		SFXSlider.value = settingData.SFXVolume;
		mouseSensitivitySlider.value = settingData.lookSensitivity;

		soundSlider.onValueChanged.AddListener(OnSoundSliderChanged);
		SFXSlider.onValueChanged.AddListener(OnSFXSliderChanged);
		mouseSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivitySliderChanged);

		Utility.ButtonBind(closeButton ,ExitButton);

		if (selectSceneButton != null)
		{
			Utility.ButtonBind(selectSceneButton, () => { SceneManager.LoadScene("StartScene"); gameObject.SetActive(false); });
		}
		gameObject.SetActive(false); 
	}

	private void OnEnable()
	{
		gameSceneUI = MiddleSceneUI.Instance?.GameSceneUI;
	}

	private void OnSoundSliderChanged(float value)
	{
		// 필요 : 배경음악.볼륨
		AudioManager.Instance.SetBGMVolume(value);
		settingData.soundVolume = value;
		soundValueText.text = $"음악 볼륨: {value*100:F0}%"; 
	}
	private void OnSFXSliderChanged(float value)
	{
		// 필요 : 효과음. 볼륨
		AudioManager.Instance.SetSFXVolume(value);
		settingData.SFXVolume = value;
		SFXValueText.text = $"효과음 볼륨: {value*100:F0}%";
	}

	private void OnMouseSensitivitySliderChanged(float value)
	{
		// 필요 : 효과음. 볼륨
		settingData.lookSensitivity = value;
		mouseSensitivityValueText.text = $"마우스 감도: {value*100:F0}"; // 소수점 둘째 자리까지 표시
	}

	private void ExitButton()
	{
		Debug.Log("닫기");

		var data = settingData;
		SettingManager.Instance.SaveSettings(data.soundVolume, data.SFXVolume, data.lookSensitivity); // esc 닫을 때 저장
		//gameSceneUI = MiddleSceneUI.Instance.GameSceneUI;
		
		if (gameSceneUI != null)
		{
			Debug.Log("뭐징");
			if (gameObject.activeSelf)
			{
				gameSceneUI.TimerData.TimeStart();
				Time.timeScale = 1f;
				Debug.Log("시작");
			}
			else
			{
				gameSceneUI.TimerData.TimeStop();
				Time.timeScale = 0f;
			}
			gameSceneUI.OpenUI(gameObject.activeSelf);
		}
		gameObject.SetActive(false);
	}
}
