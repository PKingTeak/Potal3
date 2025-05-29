using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        private Dictionary<GameObject, int> connectIDs;
        //private Dictionary<GameObject, (Vector3, Quaternion, Vector3)> originTransform;
        private List<Button> buttonList;
        private GameObject selectedPrefab;
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
        [SerializeField]
        private string prefabPath;

        private void Awake()
        {
            prefabs = new();
            selectedPrefab = null;
            connectIDs = new();
            //originTransform = new();
            buttonList = new();
            BuildList();
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                // 1. UI 클릭 시 무시
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                    return;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                // 3. 일반 선택 처리
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    GameObject clickedObject = hit.collider.gameObject;

                    if (prefabs.ContainsKey(clickedObject))
                    {
                        for (int i = 0; i < buttonList.Count; i++)
                        {
                            if (prefabs.Keys.ElementAt(i) == clickedObject)
                            {
                                Button button = buttonList[i];
                                OnButtonClicked(button, clickedObject);
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void BuildList()
        {
            selectedPrefab = null;
            buttonList.Clear();
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
                buttonList.Add(button);
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
            selectedPrefab = prefab;
            Logger.Log($"[SelectedListViewUI] selected : {prefabs[prefab]}");
            inspectorUI.InspectObject(selectedPrefab, connectIDs[selectedPrefab], prefabs[selectedPrefab]);
        }

        public void changeConnectID(GameObject prefab,  int connectID)
        {
            connectIDs[prefab] = connectID;
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

        public void AddPrefab(GameObject prefab, string prefabName, int connectID)
        {
            GameObject gameObject = GameObject.Instantiate(prefab);
            prefabs.Add(gameObject, prefabName);
            connectIDs.Add(gameObject, connectID);
            Transform transform = gameObject.transform;
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            Vector3 scale = transform.localScale;
            //originTransform.Add(gameObject, (position, rotation, scale));
            BuildList();
            Logger.Log($"[SelectedListViewuI] prefab count after Add: {prefabs.Count}");
        }

        public void RemovePrefab(GameObject prefab)
        {
            //originTransform.Remove(prefab);
            prefabs.Remove(prefab);
            connectIDs.Remove(prefab);
            inspectorUI.ClearObject();
            GameObject.Destroy(prefab);
            Logger.Log($"[SelectedListViewuI] prefab count after Remove: {prefabs.Count}");
        }
        public void OnRemoveSelectedButtonClicked()
        {
            if (selectedPrefab != null)
            {
                int selectedButtonIndex = -1;
                for (int i = 0; i < buttonList.Count; i++)
                {
                    if (buttonList[i] == selectedButton)
                    {
                        selectedButtonIndex = i;
                    }
                    
                }

                RemovePrefab(selectedPrefab);
                BuildList();

                if (selectedButtonIndex >= 0 && selectedButtonIndex < buttonList.Count)
                {
                    var newButton = buttonList[selectedButtonIndex];
                    newButton.onClick.Invoke();
                }
                else
                {
                    inspectorUI.ClearObject();
                }
            }
        }

        public StageData getStageData()
        {
            StageData stageData = new StageData();
            stageData.PrefabEntries = new List<PrefabEntry>();

            foreach (var pair in prefabs)
            {
                GameObject gameObject = pair.Key;
                string prefabPath = pair.Value;

                Transform transform = gameObject.transform;

                stageData.PrefabEntries.Add(new PrefabEntry
                {
                    prefabPath = prefabPath,
                    position = transform.position,
                    rotation = transform.rotation.eulerAngles,
                    scale = transform.localScale,
                    connectID = connectIDs[pair.Key]
                });
            }

            return stageData;
        }

        public void SetStageData(StageData stageData)
        {
            foreach (var prefab in prefabs.Keys.ToList())
            {
                RemovePrefab(prefab);
            }

            prefabs.Clear();
            connectIDs.Clear();
            //originTransform.Clear();
            buttonList.Clear();
            selectedPrefab = null;
            selectedButton = null;

            foreach (var entry in stageData.PrefabEntries)
            {
                GameObject loadedPrefab = Resources.Load<GameObject>(Path.Combine(prefabPath, entry.prefabPath));
                if (loadedPrefab == null)
                {
                    Logger.LogWarning($"[SelectedListViewUI] Resources.Load Failed: {entry.prefabPath}");
                    continue;
                }

                GameObject prefab = Instantiate(loadedPrefab);
                prefab.transform.position = entry.position;
                prefab.transform.rotation = Quaternion.Euler(entry.rotation);
                prefab.transform.localScale = entry.scale;

                prefabs.Add(prefab, entry.prefabPath);
                connectIDs.Add(prefab, entry.connectID);
                //originTransform.Add(go, (entry.position, Quaternion.Euler(entry.rotation), entry.scale));
            }

            BuildList();
            inspectorUI.ClearObject();
        }
    }
}
