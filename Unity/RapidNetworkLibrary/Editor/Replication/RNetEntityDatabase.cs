
#if ENABLE_MONO || ENABLE_IL2CPP
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace RapidNetworkLibrary.Replication
{
    public enum PeerType
    {
        Controller,
        Owner,
        Peer,
        Proxy
    }


    public class IDGenerator
    {
        public List<int> ids;
        public IDGenerator(int size)
        {
            ids = new List<int>(size);
            for (int i = 0; i < size; i++)
            {
                ids.Add(i);
            }
        }

        public int Rent()
        {
            var i = ids[0];
            ids.RemoveAt(0);
            return i;
        }

        public void Take(int id)
        {
            ids.Remove(id);
        }

        public void Return(int id)
        {
            ids.Add(id);
        }
    }




    [System.Serializable]
    public class EntityRootData
    {

        public ushort key;
        public string name;
        public List<EntityData> entityData;

        [HideInInspector]
        public bool folded;

        [HideInInspector]
        public bool listFolded;

    }

    [System.Serializable]
    public class EntityData
    {
        public PeerType peerType;
        public GameObject go;
        public ushort poolSize;

        [HideInInspector]
        public bool isShown;
    }
    [CreateAssetMenu]
    public class RNetEntityDatabase : ScriptableObject
    {
        public List<EntityRootData> entities;
    }

    [CustomEditor(typeof(RNetEntityDatabase))]
    public class LookAtPointEditor : UnityEditor.Editor
    {
        RNetEntityDatabase instance;




        private void OnEnable()
        {
            instance = target as RNetEntityDatabase;

            content = new GUIContent();
            content.text = "Element";
           
        }

        private bool isFolded = false;
        private GUIContent content;
        private GUIStyle style;
        private GUIStyle currentStyle;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            style = new GUIStyle("Foldout");
            if(currentStyle == null)
            { 
                currentStyle = new GUIStyle(GUI.skin.box);
                currentStyle.normal.background = MakeTex(2, 2, Color.black);
            }


            isFolded = EditorGUILayout.Foldout(isFolded, "Entities");
            if (isFolded)
            {

                for(int i = 0; i < instance.entities.Count; i++) 
                {

                    DrawEntityRootData(instance.entities[i], i);
                    

                }
                GUILayout.Space(5);

                if(GUILayout.Button("Create Entity"))
                {
                    
                    var root = new EntityRootData();
                    root.entityData = new List<EntityData>();
                    instance.entities.Add(root);
                }

                GUILayout.FlexibleSpace();
                if(GUILayout.Button("Bake"))
                {
                    BakeEntities();
                }


            }
            serializedObject.ApplyModifiedProperties();
            //base.OnInspectorGUI();


        }

        private void BakeEntities()
        {
            throw new NotImplementedException();
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
        public void DrawEntityRootData(EntityRootData entity, int i)
        {

            //foldedEntities[i] = EditorGUILayout.Foldout(foldedEntities[i], content, style);
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            instance.entities[i].folded = EditorGUILayout.Foldout(instance.entities[i].folded, "Test");
            GUILayout.FlexibleSpace();
            if(GUILayout.Button("Delete"))
            {
                instance.entities.Remove(instance.entities[i]);
                return;
            }
            GUILayout.EndHorizontal();
            if (instance.entities[i].folded == true)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(30);
                GUILayout.Label("Name");
                instance.entities[i].name = GUILayout.TextField(instance.entities[i].name);
                GUILayout.EndHorizontal();
                DrawEntityDatas(entity, i);

            }


            //GUILayout.Box(currentStyle, GUILayout.Width(Screen.width), GUILayout.Height(15));

            DrawBottomBorder(15);
        }

        private void DrawEntityDatas(EntityRootData entity, int i)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(45f);
            instance.entities[i].listFolded = EditorGUILayout.Foldout(instance.entities[i].listFolded, "Data");
            GUILayout.EndHorizontal();

            if (instance.entities[i].listFolded == true)
            {
                for (int x = 0; x < entity.entityData.Count; x++)
                {
                    DrawEntityData(i, x);
                }
                GUILayout.Space(instance.entities[i].entityData.Count > 0 ? 20 : 0);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Create") == true)
                {
                    var data = new EntityData();

                    instance.entities[i].entityData.Add(new EntityData());
                }
                GUILayout.EndHorizontal();
            }
        }

        private void DrawEntityData(int i, int x)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(50);
            instance.entities[i].entityData[x].isShown = EditorGUILayout.Foldout(
                instance.entities[i].entityData[x].isShown, "Elemenet " + x);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Delete"))
            {
                instance.entities[i].entityData.Remove(instance.entities[i].entityData[x]);
                return;
            }
            GUILayout.EndHorizontal();
            if (instance.entities[i].entityData[x].isShown == true)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(60);

                //instance.entities[i].entityData[x].peerType = (PeerType)EditorGUILayout.EnumPopup("PeerType",
                 //   instance.entities[i].entityData[x].peerType);

                GUILayout.Label("Peer Type");
                GUILayout.FlexibleSpace();
                instance.entities[i].entityData[x].peerType = (PeerType)EditorGUILayout.EnumPopup((Enum)instance.entities[i].entityData[x].peerType);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(60);
                GUILayout.Label("Prefab: ");
                GUILayout.FlexibleSpace();
                instance.entities[i].entityData[x].go = (GameObject)EditorGUILayout.ObjectField(instance.entities[i].entityData[x].go, typeof(GameObject), false);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(60);
                GUILayout.Label("Pool Size: ");
                GUILayout.FlexibleSpace();
                instance.entities[i].entityData[x].poolSize = (ushort)EditorGUILayout.IntField(instance.entities[i].entityData[x].poolSize);


                GUILayout.EndHorizontal();
            }
        }

        private void DrawBottomBorder(float offset)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(offset);
            GUILayout.Box(new GUIContent().text = "", currentStyle, GUILayout.Width(Screen.width), GUILayout.Height(1.5f));
            GUILayout.EndHorizontal();
        }
    }
}
#endif