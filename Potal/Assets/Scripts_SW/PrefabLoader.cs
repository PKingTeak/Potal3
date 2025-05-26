using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SW
{
    public class PrefabLoader
    {
        public Dictionary<string, GameObject> LoadAllPrefabs(string path)
        {
            Dictionary<string, GameObject> prefabs = new();

            GameObject[] prefabArr = Resources.LoadAll<GameObject>(path);

            foreach (GameObject prefab in prefabArr)
            {
                if (prefab != null)
                {
                    string key = prefab.name;
                    prefabs[key] = prefab;
                }
            }

            return prefabs;
        }
    }
}
