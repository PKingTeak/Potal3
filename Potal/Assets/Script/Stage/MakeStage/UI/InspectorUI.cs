using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class InspectorUI : MonoBehaviour
{
    private GameObject selectedObject;
    private float[] currentTransformArr;
    private int currentConnectID;

    [SerializeField]
    GizmoUI gizmoUI;
    [SerializeField]
    private OutlineDrawer outlineDrawer;
    [SerializeField]
    private TextMeshProUGUI selectedObjectName;
    [SerializeField]
    private TMP_InputField[] transformTexts = new TMP_InputField[(int)TransformIndex.Total];
    [SerializeField]
    private TMP_InputField connectIDText;
    [SerializeField]
    private SelectedListViewUI selectedListViewUI;
    bool isEditing = false;

    private enum TransformIndex
    {
        PosX = 0,
        PosY = 1,
        PosZ = 2,
        RotateX = 3,
        RotateY = 4,
        RotateZ = 5,
        ScaleX = 6,
        ScaleY = 7,
        ScaleZ = 8,
        Total = 9
    }
    // Start is called before the first frame update
    private void Awake()
    {
        ClearObject();
        currentTransformArr = new float[transformTexts.Length];
        selectedObject = null;
    }
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (selectedObject != null && !isEditing)
        {
            for (int i = 0; i < transformTexts.Length; i++)
            {
                int index = i;
                currentTransformArr = TransformToArray(selectedObject.transform);
                transformTexts[i].text = currentTransformArr[i].ToString();
            }
        }
    }

    public void InspectObject(GameObject gameObject, int connectID, string name)
    {
        selectedObject = gameObject;
        selectedObjectName.text = name;
        outlineDrawer?.SetTarget(selectedObject);
        gizmoUI?.SetTarget(selectedObject);

        currentTransformArr = TransformToArray(gameObject.transform);

        for (int i = 0; i < transformTexts.Length; i++)
        {
            int index = i;
            transformTexts[i].text = currentTransformArr[i].ToString();
            transformTexts[i].onEndEdit.RemoveAllListeners();
            transformTexts[i].onEndEdit.AddListener((string val) => OnTransformChanged(index, val));
            transformTexts[i].onSelect.AddListener(_ => isEditing = true);
            transformTexts[i].onDeselect.AddListener(_ => isEditing = false);
        }

        connectIDText.text = connectID.ToString();
        connectIDText.onEndEdit.RemoveAllListeners();
        connectIDText.onEndEdit.AddListener((string val) => OnConnectIDChanged(gameObject, val));
    }

    public void ClearObject()
    {
        selectedObjectName.SetText("None");
        selectedObject = null;
        outlineDrawer?.SetTarget(selectedObject);
        gizmoUI?.SetTarget(selectedObject);
        foreach (var transformText in transformTexts)
        {
            transformText.text = "";
        }
        connectIDText.text = "";
    }


    private void OnTransformChanged(int index, string value)
    {
        if (selectedObject == null)
        {
            transformTexts[index].text = "";
            return;
        }

        if (!float.TryParse(value, out float result))
        {
            transformTexts[index].text = currentTransformArr[index].ToString();
            return;
        }

        currentTransformArr[index] = result;
        ArrayToTransform(selectedObject.transform, currentTransformArr);
        gizmoUI?.SetTarget(selectedObject);
    }

    private void OnConnectIDChanged(GameObject gameObject, string value)
    {
        if (selectedObject == null)
        {
            connectIDText.text = "";
            return;
        }

        if (!int.TryParse(value, out int result))
        {
            connectIDText.text = currentConnectID.ToString();
            return;
        }

        currentConnectID = result;
        selectedListViewUI.changeConnectID(gameObject, result);
        ArrayToTransform(selectedObject.transform, currentTransformArr);
    }


    private float[] TransformToArray(Transform transform)
    {
        return new float[]
        {
        transform.position.x,
        transform.position.y,
        transform.position.z,
        transform.eulerAngles.x,
        transform.eulerAngles.y,
        transform.eulerAngles.z,
        transform.localScale.x,
        transform.localScale.y,
        transform.localScale.z
        };
    }

    private void ArrayToTransform(Transform transform, float[] values)
    {
        transform.position = new Vector3(values[0], values[1], values[2]);
        transform.eulerAngles = new Vector3(values[3], values[4], values[5]);
        transform.localScale = new Vector3(values[6], values[7], values[8]);
    }
}
