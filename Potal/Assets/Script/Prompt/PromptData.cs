using UnityEngine;

[CreateAssetMenu(menuName = "Data/PromptData")]
public class PromptData : ScriptableObject
{
    public string id;
    [TextArea]
    public string message;
    public float duration = 3f;
}
