using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SW
{
    public class PrefabListViewUI : MonoBehaviour
    {
        PrefabLoader prefabLoader;
        Dictionary<string, GameObject> prefabs;
        [SerializeField]
        private string prefabPath;
        void Awake()
        {
            prefabLoader = new PrefabLoader();
            prefabs = prefabLoader.LoadAllPrefabs(prefabPath);
            foreach (var pair in prefabs)
            {
                Debug.Log(pair.Key);
            }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
