using System.Collections.Generic;
using UnityEngine;

public class PromptDatabase : MonoBehaviour
{
    [SerializeField] private List<PromptData> prompts;

    private Dictionary<string, PromptData> _promptDict;

    private void Awake()
    {
        _promptDict = new Dictionary<string, PromptData>();
        foreach (var prompt in prompts)
        {
            if (!_promptDict.ContainsKey(prompt.id))
                _promptDict.Add(prompt.id, prompt);
        }
    }

    public PromptData GetPrompt(string id)
    {
        return _promptDict.TryGetValue(id, out var data) ? data : null;
    }

    public static PromptDatabase Instance { get; private set; }

    private void OnEnable()
    {
        Instance = this;
    }
}