using UnityEngine;

public class InteractableButton : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("버튼이 눌렸습니다!");
    }

    public bool CanShowUI()
    {
        return true;
    }
}
