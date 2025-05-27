using UnityEngine;
using System.IO;

public class SettingManager : MonoSingleton<SettingManager>
{
    private string SavePath => Path.Combine(Application.persistentDataPath, "setting_data.json");

    public SettingData Current { get; private set; } = new SettingData();

    private void Awake()
    {
        if (Instance == this)
        {
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
    }

    public void SaveSettings(float bgmVolume, float sfxVolume, float mouseSensitivity)
    {
        Current.soundVolume = bgmVolume;
        Current.SFXVolume = sfxVolume;
        Current.lookSensitivity = mouseSensitivity;

        string json = JsonUtility.ToJson(Current, true);
        File.WriteAllText(SavePath, json);
        Debug.Log(SavePath);
    }

    public void LoadSettings()
    {
        if (File.Exists(SavePath))
        {
            // 불러오기
            string json = File.ReadAllText(SavePath);
            Current = JsonUtility.FromJson<SettingData>(json);
            
        }
        else
        {
            // 파일 없다면 기본 값 (SettingData에 있는 값)
        }
    }
}