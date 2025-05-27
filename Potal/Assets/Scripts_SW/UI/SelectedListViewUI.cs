using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SW
{
    public class SelectedListViewUI : MonoBehaviour
    {
        private Dictionary<GameObject, string> prefabs;
        private (GameObject, string)? selectedPrefab;
        [SerializeField]
        private GameObject buttonPrefab;
        [SerializeField]
        private Transform contentRoot;
        void Awake()
        {
            prefabs = new();
            selectedPrefab = null;
            BuildList();
        }

        void Start()
        {
        }

        void Update()
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
                label.SetText(pair.Value);

                buttonGameObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    OnButtonClicked(pair.Key);
                });
            }
        }

        private void OnButtonClicked(GameObject prefab)
        {
            selectedPrefab = (prefab, prefabs[prefab]);
            Logger.Log($"[SelectedListViewUI] selected : {prefabs[prefab]}");
        }

        public void AddPrefab(GameObject prefab, string prefabName)
        {
            prefabs.Add(GameObject.Instantiate(prefab), prefabName);
            BuildList();

        }
        public void RemovePrefab(GameObject prefab)
        {
            prefabs.Remove(prefab);
        }
        public void OnRemoveSelectedButtonClicked()
        {
            if (selectedPrefab != null)
            {
                RemovePrefab(selectedPrefab.Value.Item1);
                GameObject.Destroy(selectedPrefab.Value.Item1);
                BuildList();
            }
        }
    }
}
