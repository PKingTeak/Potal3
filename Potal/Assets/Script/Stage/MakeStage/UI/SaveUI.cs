using System.Collections;
using System.Collections.Generic;
using System.IO;
using SW;
using TMPro;
using UnityEditor;
using UnityEngine;

public class SaveUI : MonoBehaviour
{
    [SerializeField]
    private SelectedListViewUI selectedListViewUI;
    [SerializeField]
    private PrefabListViewUI prefabListViewUI;
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private string savePath;
    [SerializeField]
    private TMP_Text saveResult;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSaveButtonClicked()
    {
        string fileName = inputField.text.Trim();
        if (string.IsNullOrEmpty(fileName))
        {
            Logger.LogWarning("[SaveUI] fileName Empty");
            saveResult.text = "저장 실패...";
            return;
        }

        StageData stageData = selectedListViewUI.getStageData();
        if (stageData == null || stageData.PrefabEntries == null || stageData.PrefabEntries.Count == 0)
        {
            Logger.LogWarning("[SaveUI] StageData is Empty or get Failed");
            saveResult.text = "저장 실패...";
            return;
        }

        string dirPath = Path.Combine(savePath, "StageData");
        Directory.CreateDirectory(dirPath);

        string jsonPath = Path.Combine(dirPath, $"{fileName}.json");
        string json = JsonUtility.ToJson(stageData, true);
        File.WriteAllText(jsonPath, json);

        Logger.Log($"[SaveUI] StageData JSON save Complete: {jsonPath}");
        saveResult.text = "저장 성공!";
    }

    public void OnLoadButtonClicked()
    {
        string fileName = inputField.text.Trim();
        if (string.IsNullOrEmpty(fileName))
        {
            Logger.LogWarning("[SaveUI] fileName empty");
            saveResult.text = "불러오기 실패...";
            return;
        }

        string jsonPath = Path.Combine(savePath, "StageData", $"{fileName}.json");

        if (!File.Exists(jsonPath))
        {
            Logger.LogWarning($"[SaveUI] StageData json file not Exist: {jsonPath}");
            saveResult.text = "불러오기 실패...";
            return;
        }

        string json = File.ReadAllText(jsonPath);
        StageData stageData = JsonUtility.FromJson<StageData>(json);
        if (stageData == null || stageData.PrefabEntries == null)
        {
            Logger.LogWarning("[SaveUI] JSON parsing Failed or Empty");
            saveResult.text = "불러오기 실패...";
            return;
        }

        selectedListViewUI.SetStageData(stageData);
        Logger.Log($"[SaveUI] StageData load completed: {jsonPath}");
        saveResult.text = "불러오기 성공!";
    }
}
