#if ENABLE_MONO || ENABLE_IL2CPP
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapidNetworkLibrary.Replication
{
    public enum PeerType
    {
        Controller,
        Server,
        Peer
    }
#if SERVER || UNITY_EDITOR
    [Serializable]
    public class ServerEntityData
    {
        public ushort key;
        public string bundleName;
        public int poolSize;
    }

#endif
#if CLIENT || UNITY_EDITOR
    [Serializable]
    public class ClientEntityRootData
    {
        public ushort key;
        public List<ClientEntityData> data;
    }

    [Serializable]
    public class ClientEntityData
    {
        public PeerType peerType;
        public int poolSize;
        public string bundleName;
    }
#endif
}
#endif