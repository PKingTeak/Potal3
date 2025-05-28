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
            Logger.LogWarning("[SaveUI] 입력된 파일명이 비어 있습니다.");
            saveResult.text = "저장 실패...";
            return;
        }

        StageData stageData = selectedListViewUI.getStageData();
        if (stageData == null || stageData.PrefabEntries == null || stageData.PrefabEntries.Count == 0)
        {
            Logger.LogWarning("[SaveUI] StageData가 비어 있거나 생성 실패");
            saveResult.text = "저장 실패...";
            return;
        }

        string dirPath = Path.Combine(savePath, "StageData");
        Directory.CreateDirectory(dirPath);

        string jsonPath = Path.Combine(dirPath, $"{fileName}.json");
        string json = JsonUtility.ToJson(stageData, true);
        File.WriteAllText(jsonPath, json);

        Logger.Log($"[SaveUI] StageData JSON 저장 완료: {jsonPath}");
        saveResult.text = "저장 성공!";
    }

    public void OnGoToMainButtonClicked()
    {
        //
    }
}
