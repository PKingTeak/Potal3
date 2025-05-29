using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    [Header("볼륨 조절용")]
    [SerializeField] private AudioSource[] BGMSourceArray;
    [SerializeField] private AudioSource[] SFXSourceArray;

    [Header("배경음악")]
    [SerializeField] public AudioSource BGMSourceMain;

    [Header("게임 내 효과음")]
    [SerializeField] public AudioSource SFXSourceJump;
    [SerializeField] public AudioSource SFXSourceCoin;
    [SerializeField] public AudioSource SFXSourcePortalShoot;
    [SerializeField] public AudioSource SFXSourcePortalHit;
	[SerializeField] public AudioSource SFXSourceButton;

	private void Start()
    {
		//BGMSourceMain.Play();
      // SetBGMVolume(SettingManager.Instance.Current.soundVolume);
		//SetSFXVolume(SettingManager.Instance.Current.SFXVolume);
      // DontDestroyOnLoad(gameObject);
	}

    public void SetBGMVolume(float volume)
    {
        foreach(var BGMSource in BGMSourceArray)
		{
			BGMSource.volume = volume;
		}
	}

    public void SetSFXVolume(float volume)
	{
		foreach (var SFXSource in SFXSourceArray)
		{
			SFXSource.volume = volume;
		}
	}

}