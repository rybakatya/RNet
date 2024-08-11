#if ENABLE_MONO || ENABLE_IL2CPP
using System.Collections.Generic;
using UnityEngine;

namespace RapidNetworkLibrary.Editor.Replication
{
    [System.Serializable]
    public class EditorEntityRootData
    {

        public ushort key;
        public string name;
        public List<EditorEntityData> entityData;

        [HideInInspector]
        public bool folded;

        [HideInInspector]
        public bool listFolded;

    }

}
#endif