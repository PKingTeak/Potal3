using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SW
{
    public class SelectedListViewUI : MonoBehaviour
    {
        private Dictionary<GameObject, string> prefabs;
        private Dictionary<GameObject, (Vector3, Quaternion, Vector3)> originTransform;
        private List<(Button, GameObject)> buttonGameObjectPairList;
        private (GameObject, string)? selectedPrefab;
        private Button selectedButton;
        [SerializeField]
        private GameObject buttonPrefab;
        [SerializeField]
        private Transform contentRoot;
        [SerializeField]
        private InspectorUI inspectorUI;
        [SerializeField]
        private string selectedColor;
        [SerializeField]
        private string defaultColor;

        private void Awake()
        {
            prefabs = new();
            selectedPrefab = null;
            originTransform = new();
            buttonGameObjectPairList = new();
            BuildList();
        }

        private void Start()
        {
        }

        private void Update()
        {

        }

        public void BuildList()
        {
            selectedPrefab = null;
            buttonGameObjectPairList.Clear();
            foreach (Transform child in contentRoot)
            {
                Destroy(child.gameObject);
            }

            foreach (var pair in prefabs)
            {
                GameObject buttonGameObject = Instantiate(buttonPrefab, contentRoot);
                TextMeshProUGUI label = buttonGameObject.GetComponentInChildren<TextMeshProUGUI>();
                label.SetText(pair.Value);

                Button button = buttonGameObject.GetComponent<Button>();
                buttonGameObjectPairList.Add((button, pair.Key));
                if (ColorUtility.TryParseHtmlString(defaultColor, out var color))
                {
                    ColorBlock colorBlock = button.colors;
                    colorBlock.normalColor = color;
                    button.colors = colorBlock;
                }
                button.onClick.AddListener(() =>
                {
                    OnButtonClicked(button, pair.Key);
                });
            }
        }

        private void OnButtonClicked(Button clickedButton, GameObject prefab)
        {
            RemoveSelectFocus();
            selectedButton = clickedButton;
            if (ColorUtility.TryParseHtmlString(selectedColor, out var color))
            {
                ColorBlock colorBlock = selectedButton.colors;
                colorBlock.normalColor = color;
                colorBlock.selectedColor = color;
                selectedButton.colors = colorBlock;
            }
            selectedPrefab = (prefab, prefabs[prefab]);
            Logger.Log($"[SelectedListViewUI] selected : {prefabs[prefab]}");
            inspectorUI.InspectObject(selectedPrefab.Value.Item1);
        }

        public void RemoveSelectFocus()
        {
            UnityEngine.Color color;
            ColorBlock colorBlock;
            if (selectedButton != null)
            {
                colorBlock = selectedButton.colors;
                if (ColorUtility.TryParseHtmlString(defaultColor, out color))
                {
                    colorBlock.normalColor = color;
                    colorBlock.selectedColor = color;
                    selectedButton.colors = colorBlock;
                }
            }
            selectedButton = null;
            selectedPrefab = null;
            inspectorUI.ClearObject();
        }

        public void AddPrefab(GameObject prefab, string prefabName)
        {
            GameObject gameObject = GameObject.Instantiate(prefab);
            prefabs.Add(gameObject, prefabName);
            Transform transform = gameObject.transform;
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            Vector3 scale = transform.localScale;
            originTransform.Add(gameObject, (position, rotation, scale));
            BuildList();
            Logger.Log($"[SelectedListViewuI] prefab count after Add: {prefabs.Count}");
        }

        public void RemovePrefab(GameObject prefab)
        {
            originTransform.Remove(selectedPrefab.Value.Item1);
            prefabs.Remove(prefab);
            inspectorUI.ClearObject();
            GameObject.Destroy(selectedPrefab.Value.Item1);
            Logger.Log($"[SelectedListViewuI] prefab count after Remove: {prefabs.Count}");
        }
        public void OnRemoveSelectedButtonClicked()
        {
            if (selectedPrefab != null)
            {
                int selectedButtonIndex = -1;
                for (int i = 0; i < buttonGameObjectPairList.Count; i++)
                {
                    if (buttonGameObjectPairList[i].Item1 == selectedButton)
                    {
                        selectedButtonIndex = i;
                    }
                    
                }

                RemovePrefab(selectedPrefab.Value.Item1);
                BuildList();

                if (selectedButtonIndex >= 0 && selectedButtonIndex < buttonGameObjectPairList.Count)
                {
                    var newButton = buttonGameObjectPairList[selectedButtonIndex];
                    newButton.Item1.onClick.Invoke();
                }
                else
                {
                    inspectorUI.ClearObject();
                }
            }
        }

        public StageData getStageData()
        {
            return null;
        }
    }
}
