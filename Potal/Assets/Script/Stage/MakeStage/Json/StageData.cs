using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SW
{
    [Serializable]
    public class StageData
    {
        public List<PrefabEntry> PrefabEntries;
    }

    [Serializable]
    public class PrefabEntry
    {
        public string prefabPath;
        public int connectID;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
    }
}