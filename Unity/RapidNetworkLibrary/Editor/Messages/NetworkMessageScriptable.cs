#if ENABLE_MONO || ENABLE_IL2CPP
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace RapidNetworkLibrary.Editor.Messages
{
    public class NetworkMessageScriptable : ScriptableObject
    {
        [ReadOnly]
        public ushort id;
        [ReadOnly]
        public string messageName;
        public List<NetworkMessageFields> fields;

        [HideInInspector]
        public UnityEditor.Editor editor;

        [HideInInspector]
        public bool foldedOut;

        [HideInInspector]
        public bool generateSerializer;
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

    public static class FieldTypeExtensions
    {
        public static string ToFriendlyString(this FieldType me)
        {
            var str = Regex.Replace(me.ToString(), @"\s+", "");
            return str.ToLower();

        }
    }


    [System.Serializable]
    public class NetworkMessageFields
    {
        public string fieldName;
        public FieldType fieldType;
    }
}
#endif