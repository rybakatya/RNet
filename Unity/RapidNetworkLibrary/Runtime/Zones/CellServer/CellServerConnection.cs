#if SERVER
#if ENABLE_MONO || ENABLE_IL2CPP
using UnityEditor;
using UnityEngine;
#endif

namespace RapidNetworkLibrary.Runtime.Zones
{
    public struct CellServerConnection
    {
        public uint id;
        public Rect rect;
        public Cell cell;
    }
}
#endif