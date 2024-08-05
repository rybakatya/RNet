#if ENABLE_MONO || ENABLE_IL2CPP

using RapidNetworkLibrary.Runtime.Zones;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace RapidNetworkLibrary.Editor
{
    [System.Serializable]
    public class RNetCluster : ScriptableObject
    {
        public MapSettings mapSettings;
        public List<ZoneServerSettings> zoneServers;
        public List<CellServerSettings> cellServers;
    }

  

    [System.Serializable]
    public class ServerSettings
    {
        public RNetIPAddress address;
        public byte maxChannels;
        public ushort maxConnections;
    }

    [System.Serializable]
    public class ZoneServerSettings : ServerSettings
    {
        public string zoneName;
    }

    [System.Serializable]
    public class CellServerSettings : ServerSettings
    {
        public RNetIPAddress zoneServer;
    }
   
    public class ClusterContainer
    {
        public string clusterName;
        public UnityEditor.Editor editor;
        public RNetCluster cluster;
    }
    
    public class ClusterServerWindow : RNetWindow
    {

        [MenuItem("RNet/Clusters")]
        static void GetWindow()
        {
            EditorWindow.GetWindow(typeof(ClusterServerWindow));
        }
        protected override void DoGUI(int index)
        {
            var array = clusters.Keys.ToArray();
            clusters[array[index]].editor.OnInspectorGUI();
            

           

            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("          Save          "))
            {
                SaveCluster(clusters.Values.ToArray()[index]);
            }
            GUILayout.EndHorizontal();
        }


        private void SaveCluster(ClusterContainer clusterContainer)
        {
            var mapJson = JsonConvert.SerializeObject(clusterContainer.cluster.mapSettings, Formatting.Indented);
            File.WriteAllText("Assets/Editor/Resources/clusters/" + GetFolderName(clusterContainer.clusterName) + "/MapSettings.json", mapJson);

            foreach (var zone in clusterContainer.cluster.zoneServers)
            {
                var zoneData = new ZoneServerData();
                zoneData.address = zone.address;
                zoneData.cellServers = new List<RNetIPAddress>();
                foreach (var cell in clusterContainer.cluster.cellServers)
                {
                    if (cell.zoneServer.ip == zone.address.ip && cell.zoneServer.port == zone.address.port)
                    {
                        zoneData.cellServers.Add(cell.address);

                    }
                }
                zoneData.maxChannels = zone.maxChannels;
                zoneData.maxConnections = zone.maxConnections;
                var json = JsonConvert.SerializeObject(zoneData, Formatting.Indented);

                var path = "Assets/Editor/Resources/clusters/" + GetFolderName(clusterContainer.clusterName) + "/zones/" + zone.zoneName + "/ZoneSettings.json";

                if (Directory.Exists(Path.GetDirectoryName(path)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                File.WriteAllText(path, json);
            }

            foreach (var zone in clusterContainer.cluster.zoneServers)
            {
                Debug.Log("Called");
                int idx = 0;
                foreach (var cell in clusterContainer.cluster.cellServers)
                {
                    Debug.Log("Called");
                    if (zone.address.ip.Equals(cell.zoneServer.ip) && zone.address.port == cell.zoneServer.port)
                    {
                        Debug.Log("Called");
                        var cellJson = JsonConvert.SerializeObject(cell, Formatting.Indented);

                        var p = "Assets/Editor/Resources/clusters/" + GetFolderName(clusterContainer.clusterName) + "/zones/" + zone.zoneName + "/cells/CellSettings_" + idx + ".json";
                        if (Directory.Exists(Path.GetDirectoryName(p)) == false)
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(p));
                        }
                        Debug.Log(p);
                        File.WriteAllText(p, cellJson);
                        //File.WriteAllText("Assets/Editor/Resources/clusters/" + GetFolderName(clusterContainer.clusterName) + "zones/" + zone.zoneName +"/cells/CellSettings_" + idx + ".json", cellJson);
                        idx += 1;
                    }

                }
            }
            SetupZoneDropdown();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private string GetFolderName(string clusterName)
        {
            return clusterName.Split('_')[0];
        }
    }
    


    public abstract class RNetWindow : EditorWindow
    {
        protected Dictionary<string, ClusterContainer> clusters;

        protected Dictionary<string, ZoneServerSettings> zoneServerData = new Dictionary<string, ZoneServerSettings>();
        private void OnEnable()
        {
            Debug.Log("Initializing RNet Window");
            
            var guids = AssetDatabase.FindAssets("t:RNetCluster");

            clusters = new Dictionary<string, ClusterContainer>();
            foreach (var guid in guids)
            {
               

                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<RNetCluster>(path);

                AddCluster(asset.name, asset);
            }
            SetupZoneDropdown();
            
        }

        
        protected void SetupZoneDropdown()
        {
            zoneServerData.Clear();

            var array = clusters.Keys.ToArray();
            

            foreach (var zone in clusters[array[clusterIndex]].cluster.zoneServers)
            {
                if (string.IsNullOrEmpty(zone.zoneName))
                    return;
                zoneServerData.Add(zone.zoneName, zone);

            }
        }
        int clusterIndex = 0;
        int zoneIndex = 0;
        string clusterName;
        Vector2 scrollPosition;

        protected abstract void DoGUI(int index);
        private void OnGUI()
        {
            SetupZoneDropdown();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Current Cluster:");
            clusterIndex = EditorGUILayout.Popup(clusterIndex, clusters.Keys.ToArray());

            GUILayout.Label("Current Zone:");
            zoneIndex = EditorGUILayout.Popup(zoneIndex, zoneServerData.Keys.ToArray());
            GUILayout.EndHorizontal();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
            DoGUI(clusterIndex);
            GUILayout.EndScrollView();
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Name:");
            clusterName = GUILayout.TextField(clusterName, GUILayout.MinWidth(125));
            
            if (GUILayout.Button("Create a cluster"))
            {
                CreateCluster(clusterName +"_cluster");
                
            }
            GUILayout.EndHorizontal();




        }



        
        private void AddCluster(string name, RNetCluster cluster)
        {
            clusters.Add(name, new ClusterContainer()
            {
                clusterName = name,
                cluster = cluster,
                editor = UnityEditor.Editor.CreateEditor(cluster)
            });
        }

        public void CreateCluster(string cName)
        {
            cName = cName.ToLower();
            var set = (RNetCluster)ScriptableObject.CreateInstance<RNetCluster>();

            var folderName = cName.Split('_')[0];
            var path = Path.GetFullPath("Assets/Editor/Resources/clusters/" + folderName + "/");
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);
            string assetName = UnityEditor.AssetDatabase.GenerateUniqueAssetPath(
                "Assets/Editor/Resources/clusters/" + folderName + "/" + cName + ".asset");
            AssetDatabase.CreateAsset(set, assetName);
            AssetDatabase.SaveAssets();
            AddCluster(cName, set);
            SetupZoneDropdown();
        }

       
    }

    
    public class EditorWindowTabAttribute : System.Attribute
    {
        public string name;
        public int order;
    }
    public static class AttributeExtensions
    {
        public static TValue GetAttributeValue<TAttribute, TValue>(
            this Type type,
            Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute
        {
            var att = type.GetCustomAttributes(
                typeof(TAttribute), true
            ).FirstOrDefault() as TAttribute;
            if (att != null)
            {
                return valueSelector(att);
            }
            return default(TValue);
        }
    }

    public abstract class EditorWindowTab
    {
        public abstract void OnCreate();
        public abstract void OnEnable();
        public abstract void OnGUI();
        public abstract void OnDisable();
        public abstract void OnDestroy();
    }
    public class EditorWindowTabSystem
    {
        public Dictionary<int, KeyValuePair<string, EditorWindowTab>> tabs = new Dictionary<int, KeyValuePair<string, EditorWindowTab>>();


        public EditorWindowTabSystem()
        {

            Debug.Log("Initializing Tabs");

            currentTab = null;

            //Always clear the current dictionary in OnEnable or it will continue to grow everytime the editor "recreates" windows.
            tabs.Clear();

            //Begins the reflection process
            InitializeTabs();


        }

        //Loops through all loaded assemblies and calls InitializeTabsInAssembly method.
        private void InitializeTabs()
        {
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {

                InitializeTabsInAssembly(assembly);
            }
        }


        private void InitializeTabsInAssembly(Assembly assembly)
        {
            //Get all types that inherit from SublimeNetworkEditorWindowTab 
            var types = assembly.GetTypes().Where(t => t != typeof(EditorWindowTab) &&
                                          typeof(EditorWindowTab).IsAssignableFrom(t));

            //loops through all types found above
            foreach (var type in types.ToList())
            {
                //Detects if type has required SublimeNetworkEditorTabAttribute applied.
                var b = type.IsDefined(typeof(EditorWindowTabAttribute), false);
                if (b == true)
                {
                    //read name of tab from the attribute data.
                    string name = type.GetAttributeValue((EditorWindowTabAttribute dna) => dna.name);

                    //read sortOrder of tab from the attribute data.
                    int sortOrder = type.GetAttributeValue((EditorWindowTabAttribute dna) => dna.order);


                    /*
                     * We want to make sure multiple values are not assigned to the same sortOrder in the SublimeNetworkEditorTabAttribute
                     */
                    if (tabs.ContainsKey(sortOrder) == false)
                    {

                        var kvp = new KeyValuePair<string, EditorWindowTab>(name, (EditorWindowTab)Activator.CreateInstance(type));
                        tabs.Add(sortOrder, kvp);
                        kvp.Value.OnCreate();
                        Debug.Log(tabs[sortOrder].Key);
                        Debug.Log(tabs[sortOrder].Value.GetType().Name);
                    }
                    else
                    {
                        Debug.LogError("Type: " + type.Name + " and Type: " + tabs[sortOrder].GetType().Name + " have the same sort order");
                    }
                }
                else
                    Debug.Log(type.Name + " does not have a window tab attribute");
            }

        }

       
        Vector2 scrollPosition;
        Vector2 otherScroll;
        EditorWindowTab currentTab = null;
       
        public void DrawWindowTabs()
        {
            GUILayout.BeginHorizontal();
            scrollPosition = GUILayout.BeginScrollView(
            scrollPosition, GUILayout.Width(140));

            GUILayout.BeginVertical();

            foreach (var tab in tabs)
            {
                if (GUILayout.Button(tab.Value.Key))
                {
                    Debug.Log(tab.Value.Key);
                    currentTab = tab.Value.Value;
                    tab.Value.Value.OnEnable();
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();

            if (currentTab != null)
            {
                GUILayout.BeginVertical();
                otherScroll = GUILayout.BeginScrollView(
                    otherScroll, GUILayout.Width(Screen.width - 140));
                currentTab.OnGUI();
                GUILayout.EndScrollView();
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }


    }
}

#endif