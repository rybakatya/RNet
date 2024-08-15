#if ENABLE_MONO || ENABLE_IL2CPP
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapidNetworkLibrary.Editor.Messages
{
    public class NetworkMessageScriptable : ScriptableObject
    {
        public ushort id;
        public string messageName;
        public List<NetworkMessageFields> fields;

        [HideInInspector]
        public UnityEditor.Editor editor;

        [HideInInspector]
        public bool foldedOut;
    }



    public enum FieldType
    {
        Bool,
        Byte,
        Int,
        Float,
        Long,
        Short,
        UInt,
        ULong,
        UShort
    }

    [System.Serializable]
    public class NetworkMessageFields
    {
        public string fieldName;
        public FieldType fieldType;
        public int fieldTypeIndex;
    }
}
#endif