using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Utility : MonoBehaviour
{
	public static void ButtonBind(Button button, Action action)
	{
		button.onClick.AddListener(() => action());
		button.onClick.AddListener(() => AudioManager.Instance.SFXSourceButton.Play());
	}
}
