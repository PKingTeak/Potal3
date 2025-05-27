using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClearPanel : MonoBehaviour
{
	[SerializeField] private GameSceneUI gameSceneUI;

	//스테이지 정보
	[SerializeField] private MissionData missionData;

	[SerializeField] private TextMeshProUGUI mission1Text;
	[SerializeField] private Image mission1Icon;

	[SerializeField] private TextMeshProUGUI mission2Text;
	[SerializeField] private Image mission2Icon;

	[SerializeField] private TextMeshProUGUI mission3Text;
	[SerializeField] private Image mission3Icon;

	[SerializeField] private Button exitButton;
	//필요 : 배경음악
	//필요 : 효과음

	private void Awake()
	{
		Utility.ButtonBind(exitButton,() => ExitButton());
		MissionCheck();
	}

	private void MissionCheck()
	{
		//스테이지 미션에 맞게 Text 전달
		mission1Text.text = missionData.SetMissionData(0);
		//미션에 진행 여부에 따라 FillAmount 수정
		mission1Icon.fillAmount = missionData.MissionClear(0, MissionValue(missionData.missions[0].type));

		//스테이지 미션에 맞게 Text 전달
		mission2Text.text = missionData.SetMissionData(1);
		//미션에 진행 여부에 따라 FillAmount 수정
		mission2Icon.fillAmount = missionData.MissionClear(1, MissionValue(missionData.missions[1].type));

		//스테이지 미션에 맞게 Text 전달
		mission3Text.text = missionData.SetMissionData(2);
		//미션에 진행 여부에 따라 FillAmount 수정
		mission3Icon.fillAmount = missionData.MissionClear(2, MissionValue(missionData.missions[2].type));
	}

	private int MissionValue(MissionType type)
	{
		int num = 0;

		switch (type)
		{
			case MissionType.Time:
				num = (int)EventCheckData.Instance.TimeCount;
				break;
			case MissionType.Jump:
				num = EventCheckData.Instance.JumpCount;
				//B = 1; // 점프 미션 클리어
				break;
			case MissionType.Point:
				num = EventCheckData.Instance.PointCount;
				//C = 1; // 아이템 미션 클리어
				break;
		}
		return num;
	}

	private void ExitButton()
	{
		Debug.Log("닫기");
		gameObject.SetActive(false);
		if (gameSceneUI != null)
		{
			gameSceneUI.OpenUI(!gameObject.activeSelf);
		}

		//SceneManager.LoadScene("");
	}
}
