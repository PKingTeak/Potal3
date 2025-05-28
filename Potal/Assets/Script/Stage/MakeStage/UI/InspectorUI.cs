using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class InspectorUI : MonoBehaviour
{
    private GameObject selectedObject;
    [SerializeField]
    private TextMeshProUGUI selectedObjectName;
    [SerializeField]
    private TMP_InputField[] attributeTexts = new TMP_InputField[(int)AttributeIndex.Total];
    private float[] currentValues;
    private enum AttributeIndex
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
        currentValues = new float[attributeTexts.Length];
        selectedObject = null;
    }
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void InspectObject(GameObject gameObject)
    {
        selectedObject = gameObject;
        selectedObjectName.text = gameObject.name;

        currentValues = TransformToArray(gameObject.transform);

        for (int i = 0; i < attributeTexts.Length; i++)
        {
            int index = i;
            attributeTexts[i].text = currentValues[i].ToString();
            attributeTexts[i].onEndEdit.RemoveAllListeners();
            attributeTexts[i].onEndEdit.AddListener((string val) => OnAttributeChanged(index, val));
        }
    }

    private void OnAttributeChanged(int index, string value)
    {
        if (selectedObject == null)
        {
            attributeTexts[index].text = "";
            return;
        }

        if (!float.TryParse(value, out float result))
        {
            attributeTexts[index].text = currentValues[index].ToString();
            return;
        }

        currentValues[index] = result;
        ArrayToTransform(selectedObject.transform, currentValues);
    }

    public void ClearObject()
    {
        selectedObjectName.SetText("None");
        selectedObject = null;
        foreach (var attribute in attributeTexts)
        {
            attribute.text = "";
        }
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
