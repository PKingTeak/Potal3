using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SW
{
    public class PrefabListViewUI : MonoBehaviour
    {
        public string prefabPath;
        private PrefabLoader prefabLoader;
        private Dictionary<string, GameObject> prefabs;
        private (string, GameObject)? selectedPrefab;
        private Button selectedButton;
        [SerializeField]
        private GameObject buttonPrefab;
        [SerializeField]
        private Transform contentRoot;
        [SerializeField]
        private SelectedListViewUI selectedListViewUI;
        [SerializeField]
        private string selectedColor;
        [SerializeField]
        private string defaultColor;
        private void Awake()
        {
            prefabLoader = new PrefabLoader();
            prefabs = prefabLoader.LoadAllPrefabs(prefabPath);
            selectedPrefab = null;
            selectedButton = null;
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
            foreach (Transform child in contentRoot)
            {
                Destroy(child.gameObject);
            }

            foreach (var pair in prefabs)
            {
                GameObject buttonGameObject = Instantiate(buttonPrefab, contentRoot);
                TextMeshProUGUI label = buttonGameObject.GetComponentInChildren<TextMeshProUGUI>();
                label.SetText(pair.Key);
                Button button = buttonGameObject.GetComponent<Button>();
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

        private void OnButtonClicked(Button clickedButton, string prefabName)
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
            selectedPrefab = (prefabName, prefabs[prefabName]);
            Logger.Log($"[PrefabListViewUI] selected: {prefabName}");
            if (prefabs[prefabName] == null)
            {
                return;
            }
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
        }

        public void OnAddPrefabButtonClicked()
        {
            if(selectedPrefab != null)
            {
                selectedListViewUI.AddPrefab(prefabs[selectedPrefab.Value.Item1], selectedPrefab.Value.Item1);
                selectedListViewUI.RemoveSelectFocus();
            }
        }
    }
}
