using UnityEngine;

public class PromptTrigger : MonoBehaviour
{
    [SerializeField] private string promptId;

    private bool _triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered) return;

        if (other.CompareTag("Player"))
        {
            _triggered = true;
            var ui = FindObjectOfType<GameSceneUI>();
            if (ui != null)
                ui.ShowPromptById(promptId);
        }
    }
}