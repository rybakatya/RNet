#if ENABLE_MONO || ENABLE_IL2CPP
using Codice.Utils.Buffers;
using Newtonsoft.Json;
using RapidNetworkLibrary.Connections;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using RapidNetworkLibrary;
using RapidNetworkLibrary.Replication;
using System.CodeDom.Compiler;
using System.Text;

namespace RapidNetworkLibrary.Editor.Replication
{
    [CreateAssetMenu]
    public class RNetEntityDatabase : ScriptableObject
    {
        public List<EditorEntityRootData> entities;
    }

    [CustomEditor(typeof(RNetEntityDatabase))]
    public class EntityDatabaseEditor : UnityEditor.Editor
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
            if (currentStyle == null)
            {
                currentStyle = new GUIStyle(GUI.skin.box);
                currentStyle.normal.background = MakeTex(2, 2, Color.black);
            }


            isFolded = EditorGUILayout.Foldout(isFolded, "Entities");
            if (isFolded)
            {
                if (instance.entities == null)
                    instance.entities = new List<EditorEntityRootData>();

                for (int i = 0; i < instance.entities.Count; i++)
                {

                    DrawEntityRootData(instance.entities[i], i);


                }
                GUILayout.Space(5);

                if (GUILayout.Button("Create Entity"))
                {
                    var root = new EditorEntityRootData();
                    root.entityData = new List<EditorEntityData>();
                    instance.entities.Add(root);
                }

                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Bake"))
                {
                    BakeEntities();
                }


            }
            serializedObject.ApplyModifiedProperties();
            //base.OnInspectorGUI();


        }

        private void BakeEntities()
        {
            clientDatas.Clear();
            serverDatas.Clear();
            ushort i = 0;
            foreach (var entity in instance.entities)
            {
                entity.key = i;
                BakeEntity(entity);
                i++;
            }
            RebuildAssetBundles();
            BuildJson();
            GenerateEntityCode();
        }

        private void GenerateEntityCode()
        {
            var builder = new StringBuilder();
            builder.AppendLine("public class EntityKeys");
            builder.AppendLine("{");
            foreach (var entity in instance.entities)
            {
                
                builder.AppendLine("    public const ushort " + entity.name + " = " + entity.key + ";");


            }
            builder.AppendLine("}");
            var p = "Assets/_code/generated/entities/EntityKeys_generated.cs";
            if (File.Exists(p))
            {
                File.Delete(p);
            }
            if (Directory.Exists(Path.GetDirectoryName(p)) == false)
                Directory.CreateDirectory(Path.GetDirectoryName(p));

            File.WriteAllText(p, builder.ToString());

            
        }

       

        List<ServerEntityData> serverDatas = new List<ServerEntityData>();
        List<ClientEntityRootData> clientDatas = new List<ClientEntityRootData>();
        public void BuildJson()
        {
            var path = Path.Combine(Application.streamingAssetsPath, "server");
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);
            path = Path.Combine(path, "ServerEntities.json");
            File.WriteAllText(path, JsonConvert.SerializeObject(serverDatas, Formatting.Indented));

            path = Path.Combine(Application.streamingAssetsPath, "client");
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);
            path = Path.Combine(path, "ClientEntities.json");
            File.WriteAllText(path, JsonConvert.SerializeObject(clientDatas, Formatting.Indented));
        }
        private List<AssetBundleBuild> bundlesToRebuild = new List<AssetBundleBuild>();
        private void RebuildAssetBundles()
        {
            var oPath = Path.Combine(Application.streamingAssetsPath);
            if (Directory.Exists(oPath) == false)
                Directory.CreateDirectory(oPath);

            Debug.Log(oPath);
            BuildPipeline.BuildAssetBundles(oPath, bundlesToRebuild.ToArray(), BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
            bundlesToRebuild.Clear();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void BakeEntity(EditorEntityRootData entity)
        {
            BakeServer(entity);
            BakeClient(entity);
        }


        private void BakeClient(EditorEntityRootData entity)
        {
            var clientData = new ClientEntityRootData();
            clientData.key= entity.key;
           
            clientData.data = new List<ClientEntityData>();
            foreach(var data in entity.entityData)
            {
                if(data.peerType != PeerType.Server)
                {
                    var bundleName = "client/" + entity.name + "_" + data.peerType.ToString().ToLower();
                    clientData.data.Add(new ClientEntityData()
                    {
                        bundleName = bundleName,
                        peerType = data.peerType,
                        poolSize = data.poolSize
                    });

                    data.go.name = bundleName;
                    var assetPath = AssetDatabase.GetAssetPath(data.go);
                    var importer = UnityEditor.AssetImporter.GetAtPath(assetPath);

                    importer.assetBundleName = bundleName;


                    AssetBundleBuild buildMap = new AssetBundleBuild();
                    buildMap.assetBundleName = importer.assetBundleName;

                    // Find all asset paths assigned to this AssetBundle
                    string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(importer.assetBundleName);
                    buildMap.assetNames = assetPaths;

                    

                    bundlesToRebuild.Add(buildMap);
                }
            }
            clientDatas.Add(clientData);

        }
        private void BakeServer(EditorEntityRootData entity)
        {
            var serverData = new ServerEntityData();

            serverData.key = entity.key;
            foreach(var data in entity.entityData)
            {
                if(data.peerType == PeerType.Server)
                {
                    serverData.poolSize = data.poolSize;
                   
                    var assetPath = AssetDatabase.GetAssetPath(data.go);

                    var importer = UnityEditor.AssetImporter.GetAtPath(assetPath);
                    serverData.bundleName = "server/"+entity.name;
                    importer.assetBundleName = serverData.bundleName;

                    data.go.name = serverData.bundleName;
                    AssetBundleBuild buildMap = new AssetBundleBuild();
                    buildMap.assetBundleName = importer.assetBundleName;

                    // Find all asset paths assigned to this AssetBundle
                    string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(importer.assetBundleName);
                    buildMap.assetNames = assetPaths;



                    bundlesToRebuild.Add(buildMap);
                    serverDatas.Add(serverData);
                    break;
                }
            } 
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
        public void DrawEntityRootData(EditorEntityRootData entity, int i)
        {

            //foldedEntities[i] = EditorGUILayout.Foldout(foldedEntities[i], content, style);
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            instance.entities[i].folded = EditorGUILayout.Foldout(instance.entities[i].folded, "Test");
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Delete"))
            {
                foreach(var data in instance.entities[i].entityData)
                {
                    if(data.go != null)
                    {
                        var assetPath = AssetDatabase.GetAssetPath(data.go);
                        if(assetPath != null)
                        {
                            var importer = AssetImporter.GetAtPath(assetPath);
                            importer.assetBundleName = "";
                        }
                    }
                }
                instance.entities.Remove(instance.entities[i]);
                BakeEntities();
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

        private void DrawEntityDatas(EditorEntityRootData entity, int i)
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
                    var data = new EditorEntityData();

                    instance.entities[i].entityData.Add(new EditorEntityData());
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
                if(instance.entities[i].entityData[x].go != null)
                {
                    var assetPath = AssetDatabase.GetAssetPath(instance.entities[i].entityData[x].go);
                    if (assetPath != null)
                    {
                        var importer = AssetImporter.GetAtPath(assetPath);
                        importer.assetBundleName = "";
                    }
                }
                instance.entities[i].entityData.Remove(instance.entities[i].entityData[x]);
                BakeEntities();
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