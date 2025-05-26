using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SW
{
    public class PrefabLoader
    {
        public Dictionary<string, GameObject> LoadAllPrefabs(string path)
        {
            Dictionary<string, GameObject> dict = new();

            GameObject[] prefabs = Resources.LoadAll<GameObject>(path);

            foreach (GameObject prefab in prefabs)
            {
                if (prefab != null)
                {
                    string key = prefab.name;
                    dict[key] = prefab;
                }
            }

            return dict;
        }
    }
}
