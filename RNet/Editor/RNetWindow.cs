#if ENABLE_MONO || ENABLE_IL2CPP

using RapidNetworkLibrary.Editor.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace RapidNetworkLibrary.Editor
{



    [ExtensionWindowTab(name = "Messages", order = 0)]
    public class NetworkMessageTab : ExtensionWindowTab
    {
        private Dictionary<ushort, NetworkMessageScriptable> messages = new Dictionary<ushort, NetworkMessageScriptable>();
        private IDGenerator idGen;
        public NetworkMessageTab(RNetWindow window) : base(window)
        {
            
            string[] guids = AssetDatabase.FindAssets("t:NetworkMessageScriptable"); //FindAssets uses tags check documentation for more info
            idGen = new IDGenerator((ushort)(guids.Length + 2048));
            for (int i = 0; i < guids.Length; i++) //probably could get optimized
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i] );
                NetworkMessageScriptable a = AssetDatabase.LoadAssetAtPath<NetworkMessageScriptable>(path);
                idGen.Take(a.id);
                Add(a);
            }
        }


        private void Add(NetworkMessageScriptable msg)
        {
            msg.editor = UnityEditor.Editor.CreateEditor(msg);
            messages.Add(msg.id, msg);
        }
        public override void OnEnable()
        {
            
        }

        string name = string.Empty;
       
        public override void OnGUI()
        {
            GUILayout.BeginVertical();
            foreach(var msg in  messages)
            {
                msg.Value.foldedOut = EditorGUILayout.Foldout(msg.Value.foldedOut, msg.Value.name);
                if (msg.Value.foldedOut == true)
                {
                    GUILayout.BeginVertical();
                    msg.Value.editor.OnInspectorGUI();
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();

                    if(GUILayout.Button("Save"))
                    {
                        GenerateMessageIDSStruct();
                        GenerateMessageDataStruct(msg.Value);
                        if(msg.Value.generateSerializer == true)
                            GenerateMessageSerializer(msg.Value);
                        
                    }
                    if (GUILayout.Button("Delete"))
                    {
                        messages.Remove(msg.Key);
                        var asset = AssetDatabase.GetAssetPath(msg.Value);
                        AssetDatabase.DeleteAsset(asset);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                        GUILayout.EndVertical();
                        return;
                    }
                    GUILayout.Label("Generate Serializer");
                    msg.Value.generateSerializer = EditorGUILayout.Toggle(msg.Value.generateSerializer, GUILayout.MaxWidth(15));
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Message Name: ");
            name = GUILayout.TextField(name, GUILayout.MinWidth(120), GUILayout.MaxWidth(120));
            if (GUILayout.Button("Create") == true)
            {
                foreach (var msg in messages)
                {
                    if (msg.Value.messageName.Equals(name))
                    {
                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                        Logging.Logger.Log(Logging.LogLevel.Error, "Cannot create another message with the name " + name);
                        return;
                    }
                }
                NetworkMessageScriptable asset = ScriptableObject.CreateInstance<NetworkMessageScriptable>();
                asset.fields = new List<NetworkMessageFields>();
                asset.messageName = name;
                asset.id = idGen.Rent();
                var path = "Assets/Editor/Resources/Messages/" + name + ".asset";
                if (Directory.Exists(Path.GetDirectoryName(path)) == false)
                    Directory.CreateDirectory(Path.GetDirectoryName(path));

                string p = UnityEditor.AssetDatabase.GenerateUniqueAssetPath(path);
                AssetDatabase.CreateAsset(asset, p);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Add(asset);
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

        }

        private void GenerateMessageSerializer(NetworkMessageScriptable value)
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("using RapidNetworkLibrary.Serialization;");
            str.AppendLine("using RapidNetworkLibrary.Memory;");
            str.AppendLine("using System;");
            str.AppendLine("");
            str.AppendLine("[Serializer(NetworkMessageIDS." + value.messageName +")]");
            str.AppendLine("public class " + value.messageName + "Serializer : Serializer");
            str.AppendLine("{");
            str.AppendLine("    public override void Serialize(BitBuffer buffer, IntPtr messageData)");
            str.AppendLine("    {");
            str.AppendLine("        var msg = MemoryHelper.Read<" + value.messageName + ">(messageData);");
            foreach (var field in value.fields)
            {
                if (field.fieldType == FieldType.Bool)
                    str.AppendLine("        buffer.AddBool(msg." + field.fieldName + ");");

                if (field.fieldType == FieldType.Byte)
                    str.AppendLine("        buffer.AddByte(msg." + field.fieldName + ");");

                if (field.fieldType == FieldType.Float)
                    str.AppendLine("        buffer.AddUShort(HalfPrecision.Quantize(msg." + field.fieldName + "));");

                if (field.fieldType == FieldType.Int)
                    str.AppendLine("        buffer.AddInt(msg." + field.fieldName + ");");

                if (field.fieldType == FieldType.Long)
                    str.AppendLine("        buffer.AddLong(msg." + field.fieldName + ");");

                if (field.fieldType == FieldType.Short)
                    str.AppendLine("        buffer.AddShort(msg." + field.fieldName + ");");

                if (field.fieldType == FieldType.UInt)
                    str.AppendLine("        buffer.AddUInt(msg." + field.fieldName + ");");

                if (field.fieldType == FieldType.ULong)
                    str.AppendLine("        buffer.AddULong(msg." + field.fieldName + ");");

                if (field.fieldType == FieldType.UShort)
                    str.AppendLine("        buffer.AddUShort(msg." + field.fieldName + ");");

            }
            str.AppendLine("    }");

            str.AppendLine("    public override IntPtr Deserialize(BitBuffer buffer)");
            str.AppendLine("    {");
            str.AppendLine("        var msg = new " + value.messageName + "()");
            str.AppendLine("        {");
            foreach (var field in value.fields)
            {
                if (field.fieldType == FieldType.Bool)
                    str.AppendLine("            " + field.fieldName + " = buffer.ReadBool(),");

                if (field.fieldType == FieldType.Byte)
                    str.AppendLine("            " + field.fieldName + " = buffer.ReadByte(),");

                if (field.fieldType == FieldType.Float)
                    str.AppendLine("            " + field.fieldName + " = HalfPrecision.Dequantize(buffer.ReadUShort()),");

                if (field.fieldType == FieldType.Int)
                    str.AppendLine("            " + field.fieldName + " = buffer.ReadInt(),");

                if (field.fieldType == FieldType.Long)
                    str.AppendLine("            " + field.fieldName + " = buffer.ReadLong(),");

                if (field.fieldType == FieldType.Short)
                    str.AppendLine("            " + field.fieldName + " = buffer.ReadShort(),");

                if (field.fieldType == FieldType.UInt)
                    str.AppendLine("            " + field.fieldName + " = buffer.ReadUInt(),");

                if (field.fieldType == FieldType.ULong)
                    str.AppendLine("            " + field.fieldName + " = buffer.ReadULong(),");

                if (field.fieldType == FieldType.UShort)
                    str.AppendLine("            " + field.fieldName + " = buffer.ReadUShort(),");

            }
            str.AppendLine("        };");
            str.AppendLine("        return MemoryHelper.Write(msg);");
            str.AppendLine("    }");

            str.AppendLine("}");
            var p = "Assets/_code/generated/messages/" + value.messageName + "/" + value.messageName + "Serializer.cs";
            if (Directory.Exists(Path.GetDirectoryName(p)) == false)
                Directory.CreateDirectory(Path.GetDirectoryName(p));

            File.WriteAllText(p, str.ToString());
            AssetDatabase.Refresh();
        }


        private void GenerateMessageIDSStruct()
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("public class NetworkMessageIDS");
            str.AppendLine("{");
            foreach(var msg in messages.Values)
            {
                str.AppendLine("    public const ushort " + msg.messageName + " = " + msg.id + ";");
            }
            str.AppendLine("}");

            var p = "Assets/_code/generated/messages/NetworkMessageIDS.cs";
            if (Directory.Exists(Path.GetDirectoryName(p)) == false)
                Directory.CreateDirectory(Path.GetDirectoryName(p));

            File.WriteAllText(p, str.ToString());
            AssetDatabase.Refresh();
        }
        private void GenerateMessageDataStruct(NetworkMessageScriptable msg)
        {
            StringBuilder structStr = new StringBuilder();
            structStr.AppendLine("using RapidNetworkLibrary.Serialization;");
            structStr.AppendLine("public struct " + msg.messageName + " : IMessageObject");
            structStr.AppendLine("{");
            foreach (var field in msg.fields)
            {
                structStr.AppendLine("    public " + field.fieldType.ToFriendlyString() + " " + field.fieldName + ";");
            }
            structStr.AppendLine("}");


            var p = "Assets/_code/generated/messages/" + msg.messageName + "/" + msg.messageName + ".cs";
            if (Directory.Exists(Path.GetDirectoryName(p)) == false)
                Directory.CreateDirectory(Path.GetDirectoryName(p));

            File.WriteAllText(p, structStr.ToString());
            AssetDatabase.Refresh();

        }



        public override void OnDestroy()
        {
            
        }
    }

    public class RNetWindow : EditorWindow
    {
        [MenuItem("RNet/Main")]
        public static void ShowRNet()
        {
            RNetWindow wnd = GetWindow<RNetWindow>();
            wnd.titleContent = new GUIContent("RNet");
            wnd.Show();
        }

        private SortedDictionary<int, (string, ExtensionWindowTab)> tabs = new SortedDictionary<int, (string, ExtensionWindowTab)>();

        private void OnEnable()
        {
            FindTabs();
        }

        private void FindTabs()
        {
            foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach(var type in assembly.GetTypes())
                {
                    if(type.BaseType == typeof(ExtensionWindowTab))
                    {
                        if(type.IsAbstract == false)
                        {
                            var attribute = (ExtensionWindowTabAttribute[])type.GetCustomAttributes(typeof(ExtensionWindowTabAttribute), true);
                            if (attribute.Length > 0)
                            {
                                if (tabs.ContainsKey(attribute[0].order) == true)
                                {
                                    Logging.Logger.Log(Logging.LogLevel.Error, "Order " + attribute[0].order + " assigned to " + type.Name + " is already assigned to another tab!");
                                    continue;
                                }
                                tabs.Add(attribute[0].order, (attribute[0].name, (ExtensionWindowTab)Activator.CreateInstance(type, new object[] { this })));
                            }
                        }
                    }
                }
            }
        }
        Vector2 tabsPos;
        Vector2 contentPos;
        ExtensionWindowTab currentTab;
        private void OnGUI()
        {

            GUILayout.BeginHorizontal();

            tabsPos = EditorGUILayout.BeginScrollView(tabsPos, GUILayout.Width(Screen.width / 4), GUILayout.Height(Screen.height-25));
            GUILayout.BeginVertical();
            foreach (var tab in tabs)
            {

                if (GUILayout.Button(tab.Value.Item1))
                {

                    if (tab.Value.Item2 != currentTab)
                    {
                        if(currentTab != null)
                            currentTab.OnDestroy();
                    }
                    currentTab = tab.Value.Item2;
                    tab.Value.Item2.OnEnable();
                }

            }
            GUILayout.EndVertical();
            EditorGUILayout.EndScrollView();

            if (currentTab == null)
            {
                GUILayout.EndHorizontal();
                return;
            }
            contentPos = EditorGUILayout.BeginScrollView(contentPos, GUILayout.Width((Screen.width / 4) * 3), GUILayout.Height(Screen.height - 25));
            GUILayout.BeginVertical();
            currentTab.OnGUI();
            GUILayout.EndVertical();
            EditorGUILayout.EndScrollView();

            GUILayout.EndHorizontal();
        }

        private void OnDestroy()
        {
            currentTab.OnDestroy();
            tabs.Clear();
        }
    }

    public class ReadOnlyAttribute : PropertyAttribute { }



    // Put this script in the "Editor" folder
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}
#endif