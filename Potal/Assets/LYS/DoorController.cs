using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorController : MonoBehaviour
{
    Vector3 openPosition;
    Vector3 closeTargetPosition;
    
    private Tween moveTween;
    
    private void Start()
    {
        openPosition = transform.position + new Vector3(-2f, 0f, 0f);
        closeTargetPosition = transform.position;
    }
    public void Open()
    {
        Debug.Log("Open Tween");
        moveTween?.Kill();
        moveTween = transform.DOMoveX(openPosition.x, 1f).SetEase(Ease.InOutSine);
    }

    public void Close()
    {
        Debug.Log("Close Tween");
        moveTween?.Kill();
        moveTween = transform.DOMoveX(closeTargetPosition.x, 1f).SetEase(Ease.InOutSine);
    }
}
