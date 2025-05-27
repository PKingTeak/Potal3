using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SW
{
    [CreateAssetMenu(fileName = "Stage", menuName = "New Stage")]
    public class StageData : ScriptableObject
    {
        [Header("Info")]
        public List<PrefabEntry> PrefabEntries;
        [Header("Start")]
        public Vector3 startPosition;
    }

    [Serializable]
    public class PrefabEntry
    {
        public string prefabPath;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;

    }
}