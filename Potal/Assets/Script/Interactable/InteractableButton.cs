using UnityEngine;

public class InteractableButton : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        // 버튼 동작
    }

    public bool CanShowUI()
    {
        return true;
    }
}