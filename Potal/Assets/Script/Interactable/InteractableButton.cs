using UnityEngine;

public class InteractableButton : MonoBehaviour, IInteractable
{
    public void Interact()
    {
    }

    public bool CanShowUI()
    {
        return true;
    }
}
