using UnityEngine;
using System.IO;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance { get; private set; }

    private string SavePath => Path.Combine(Application.persistentDataPath, "setting_data.json");

    public SettingData Current { get; private set; } = new SettingData();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveSettings(float bgmVolume, float sfxVolume, float mouseSensitivity)
    {
        Current.bgmVolume = bgmVolume;
        Current.sfxVolume = sfxVolume;
        Current.mouseSensitivity = mouseSensitivity;

        string json = JsonUtility.ToJson(Current, true);
        File.WriteAllText(SavePath, json);
        // 파일 저장, 경로 출력
        Debug.Log(SavePath);
    }

    public void LoadSettings()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            Current = JsonUtility.FromJson<SettingData>(json);
        }
        else
        {
            // 파일 없을 시 기본 값으로 출력
        }
    }
}