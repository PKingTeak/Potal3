using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GizmoSelectButtonType
{
    Default = 0,
    Position = 1,
    Rotation = 2,
    Scale = 3,
    Total = 4
}
public class GizmoSelectUI : MonoBehaviour
{
    [SerializeField]
    private Button defaultButton;
    [SerializeField]
    private Button positionButton;
    [SerializeField]
    private Button rotationButton;
    [SerializeField]
    private Button scaleButton;
    [SerializeField]
    private GizmoUI gizmoUI;
    [SerializeField]
    private Color defaultColor;
    [SerializeField]
    private Color selectedColor;
    [SerializeField]
    private Color defaultHighlightColor;

    private Button[] buttons;

    // Start is called before the first frame update
    private void Start()
    {
        buttons = new Button[(int)GizmoSelectButtonType.Total];
        buttons[(int)GizmoSelectButtonType.Default] = defaultButton;
        buttons[(int)GizmoSelectButtonType.Position] = positionButton;
        buttons[(int)GizmoSelectButtonType.Rotation] = rotationButton;
        buttons[(int)GizmoSelectButtonType.Scale] = scaleButton;

        defaultButton?.onClick.AddListener(() => { 
            OnDefaultButtonClicked(); 
        });
        positionButton?.onClick.AddListener(() =>
        {
            OnPositionButtonClicked();
        });
        rotationButton?.onClick.AddListener(() =>
        {
            OnRotationButtonClicked();
        });
        scaleButton?.onClick.AddListener(() =>
        {
            OnScaleButtonClicked();
        });
        OnDefaultButtonClicked();
    }

    private void OnDefaultButtonClicked()
    {
        gizmoUI?.HideAllHandles();
        select(GizmoSelectButtonType.Default);
    }

    private void OnPositionButtonClicked()
    {
        gizmoUI?.ShowOnly(GizmoHandleType.Position);
        select(GizmoSelectButtonType.Position);
    }

    private void OnRotationButtonClicked()
    {
        gizmoUI?.ShowOnly(GizmoHandleType.Rotation);
        select(GizmoSelectButtonType.Rotation);
    }
    private void OnScaleButtonClicked()
    {
        gizmoUI?.ShowOnly(GizmoHandleType.Scale);
        select(GizmoSelectButtonType.Scale);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void select(GizmoSelectButtonType gizmoSelectButton)
    {
        for(int i = 0; i < (int)GizmoSelectButtonType.Total; i++)
        {
            Button button = buttons[i];
            if ((int)i == (int)gizmoSelectButton)
            {
                ColorBlock colorBlock = button.colors;
                colorBlock.normalColor = selectedColor;
                colorBlock.selectedColor = selectedColor;
                colorBlock.highlightedColor = selectedColor;
                button.colors = colorBlock;
            }
            else
            {
                ColorBlock colorBlock = button.colors;
                colorBlock.normalColor = defaultColor;
                colorBlock.selectedColor = defaultHighlightColor;
                colorBlock.highlightedColor = defaultHighlightColor;
                button.colors = colorBlock;
            }
        }
    }
}
