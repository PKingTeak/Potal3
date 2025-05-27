using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingData : MonoSingleton<SettingData>
{
	public float lookSensitivity = 0.5f;
	public float soundVolume = 0.5f;
	public float SFXVolume = 0.5f;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}
}
