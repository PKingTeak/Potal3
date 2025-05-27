using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorController : MonoBehaviour, IIdentifiable
{
    [SerializeField] private string id;
    public string Id => id;
    
    private Tween moveTween;
    
    private Vector3 openPosition;
    private Vector3 closeTargetPosition;
    
    private void Start()
    {
        openPosition = transform.position + new Vector3(-2f, 0f, 0f);
        closeTargetPosition = transform.position;
    }
    public void Open()
    { ;
        moveTween?.Kill();
        moveTween = transform.DOMoveX(openPosition.x, 1f).SetEase(Ease.InOutSine);
    }

    public void Close()
    {
        moveTween?.Kill();
        moveTween = transform.DOMoveX(closeTargetPosition.x, 1f).SetEase(Ease.InOutSine);
    }
}
