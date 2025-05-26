using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SW
{
    public class PrefabListViewUI : MonoBehaviour
    {
        private PrefabLoader prefabLoader;
        private Dictionary<string, GameObject> prefabs;
        private (string, GameObject)? selectedPrefab;
        [SerializeField]
        private GameObject buttonPrefab;
        [SerializeField]
        private Transform contentRoot;
        [SerializeField]
        private string prefabPath;
        [SerializeField]
        private SelectedListViewUI selectedListViewUI;
        void Awake()
        {
            prefabLoader = new PrefabLoader();
            prefabs = prefabLoader.LoadAllPrefabs(prefabPath);
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
                label.SetText(pair.Key);

                buttonGameObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    OnButtonClicked(pair.Key);
                });
            }
        }

        private void OnButtonClicked(string prefabName)
        { 
            selectedPrefab = (prefabName, prefabs[prefabName]);
            Debug.Log($"Button clicked: {prefabName}");
            if (prefabs[prefabName] == null)
            {
                return;
            }
            selectedListViewUI.AddPrefab(prefabs[prefabName], prefabName);
            selectedListViewUI.BuildList();
        }
    }
}
