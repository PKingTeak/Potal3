using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerData : MonoBehaviour
{
	[Header("시간 타이머")]
	[SerializeField] private float timer = 0;
	[SerializeField] private Coroutine timerCoroutine;
	[SerializeField] private TextMeshProUGUI timerText;

	private void Start()
	{
		timerCoroutine = StartCoroutine("TimerStart");
	}
	private IEnumerator TimerStart()
	{
		while (true)
		{
			timer += Time.deltaTime;
			timerText.text = Mathf.FloorToInt(timer) + "초 " + ((int)(timer % 1 * 100)).ToString("D2");
			yield return null;
		}
	}

	public string TimerStop()
	{
		StopCoroutine(TimerStart());
		return timerText.text;
	}
}
