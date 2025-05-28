using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour, IIdentifiable
{
    [SerializeField] private string id;
    public string Id => id;
    
    private Tween leftMoveTween;
    private Tween rightMoveTween;

    private float leftInitX;
    private float rightInitX;

    private float leftTargetX;
    private float rightTargetX;

    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;

    [SerializeField] private float openDistance;

    private void Awake()
    {
        id = ExtractInstanceIndex(gameObject.name);
    }

    private string ExtractInstanceIndex(string id)
    {
        Match match = Regex.Match(name, @"\((\d+)\)");
        if (match.Success)
            return match.Groups[1].Value;
        return "0";
    }

    private void Start()
    {
        leftInitX = leftDoor.transform.localPosition.x;
        rightInitX = rightDoor.transform.localPosition.x;
        leftTargetX = leftDoor.transform.localPosition.x - openDistance;
        rightTargetX = rightDoor.transform.localPosition.x + openDistance;
    }
    public void Open()
    { 
        leftMoveTween?.Kill();
        rightMoveTween?.Kill();
        
        leftMoveTween = leftDoor.transform.DOLocalMoveX(leftTargetX, 1f).SetEase(Ease.InOutSine);
        rightMoveTween = rightDoor.transform.DOLocalMoveX(rightTargetX, 1f).SetEase(Ease.InOutSine);
    }

    public void Close()
    {
        leftMoveTween?.Kill();
        rightMoveTween?.Kill();
        leftMoveTween = leftDoor.transform.DOLocalMoveX(leftInitX, 1f).SetEase(Ease.InOutSine);
        rightMoveTween = rightDoor.transform.DOLocalMoveX(rightInitX, 1f).SetEase(Ease.InOutSine);
    }
}
