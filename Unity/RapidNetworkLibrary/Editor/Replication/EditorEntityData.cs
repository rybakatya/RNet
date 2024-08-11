#if ENABLE_MONO || ENABLE_IL2CPP
using RapidNetworkLibrary.Replication;
using UnityEngine;

namespace RapidNetworkLibrary.Editor.Replication
{
    [System.Serializable]
    public class EditorEntityData
    {
        public PeerType peerType;
        public GameObject go;
        public ushort poolSize;

        [HideInInspector]
        public bool isShown;
    }

}
#endif
