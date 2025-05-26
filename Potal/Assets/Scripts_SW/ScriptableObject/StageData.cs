using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SW
{
    [CreateAssetMenu(fileName = "Stage", menuName = "New Stage")]
    public class SceneData : ScriptableObject
    {
        [Header("Info")]
        public List<PrefabEntry> PrefabEntries;
    }

    [Serializable]
    public class PrefabEntry
    {
        public GameObject Prefab;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
    }
}