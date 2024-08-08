#if ENABLE_MONO || ENABLE_IL2CPP
using Newtonsoft.Json;
using RapidNetworkLibrary.Logging;
using RapidNetworkLibrary.Replication;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace RapidNetworkLibrary
{
    public abstract class RNetEntityPoolBase<T>
    {
        public RNetEntityPoolBase() 
        {
            OnInit();
        }

        protected abstract void OnInit();

#if SERVER
        public abstract T Rent(ushort key);

        public abstract void Return(ushort key, T go);
#elif CLIENT
        public abstract GameObject Rent(ushort key, PeerType peerType);
        public abstract void Return(ushort key, PeerType peerType, T go);
#endif
    }


    public abstract class RNetJsonParserPool<T> : RNetEntityPoolBase<T>
    {
        protected RNetJsonParserPool() : base() 
        {

        }

        protected override void OnInit()
        {
#if SERVER
            var json = File.ReadAllText(Application.streamingAssetsPath + "/server/ServerEntities.json");
            OnEntityDataLoaded(JsonConvert.DeserializeObject<List<ServerEntityData>>(json));

#elif CLIENT
            var json = File.ReadAllText(Application.streamingAssetsPath + "/server/ServerEntities.json");
            OnEntityDataLoaded(JsonConvert.DeserializeObject<List<ClientEntityRootData>>(json));
#endif
        }
#if SERVER
        protected abstract void OnEntityDataLoaded(List<ServerEntityData> entities);
#elif CLIENT
        protected abstract void OnEntityDataLoaded(List<ClientEntityRootData> entities);
#endif

    }
    public class RNetEntityPool<T> : RNetJsonParserPool<GameObject>
    {
        private Vector3 poolPos = new Vector3(-20000, -20000, -20000);
#if SERVER
        private Dictionary<ushort, Stack<GameObject>> pool = new Dictionary<ushort, Stack<GameObject>>();
#elif CLIENT
        private Dictionary<ushort, Dictionary<PeerType, Stack<GameObject>>> pool = new Dictionary<ushort, Dictionary<PeerType, Stack<GameObject>>>();

#endif

#if SERVER
        public override GameObject Rent(ushort key)

#elif CLIENT
        public override GameObject Rent(ushort key, PeerType peerType)
        
#endif
        { 

#if SERVER
            return pool[key].Pop();
#elif CLIENT
            return pool[key][peerType].Pop();
#endif
        }
#if SERVER
        public override void Return(ushort key, GameObject go)
#elif CLIENT
        public override void Return(ushort key, PeerType peerType, GameObject go)
#endif
        {
#if SERVER
            pool[key].Push(go);
#elif CLIENT
            pool[key][peerType].Push(go);

#endif
            go.transform.position = poolPos;
        }


#if SERVER
        protected override void OnEntityDataLoaded(List<ServerEntityData> entities)
#elif CLIENT
        protected override void OnEntityDataLoaded(List<ClientEntityRootData> entities)
#endif
        {
#if SERVER
            
            foreach (var entity in entities)
            {
                pool.Add(entity.key, new Stack<GameObject>());
                var path = Path.Combine(Application.streamingAssetsPath, entity.bundleName);
                var loadedBundle = AssetBundle.LoadFromFile(path);
                if (loadedBundle == null)
                {
                    Logging.Logger.Log(LogLevel.Warning, "Unable to load entity asset bundle at path " + path);
                    continue;
                }
                var prefab = loadedBundle.LoadAsset<GameObject>(entity.bundleName);
                if(prefab == null)
                {
                    Logging.Logger.Log(LogLevel.Warning, "Unable to load asset " + entity.bundleName + " in entity asset bundle at path " + path);
                    continue;
                }
                for (int i = 0; i < entity.poolSize; i++)
                {
                    var go = GameObject.Instantiate(prefab);
                    pool[entity.key].Push(go);
                }               
            }
#elif CLIENT
            foreach(var entity in entities)
            {
                pool.Add(entity.key, new Dictionary<PeerType, Stack<GameObject>>());
                foreach(var data in entity.data)
                {

                    
                    var path = Path.Combine(Application.streamingAssetsPath, data.bundleName);
                    var loadedBundle = AssetBundle.LoadFromFile(path);
                    if (loadedBundle == null)
                    {
                        Logging.Logger.Log(LogLevel.Warning, "Unable to load entity asset bundle at path " + path);
                        continue;
                    }
                    var prefab = loadedBundle.LoadAsset<GameObject>(data.bundleName);
                    if (prefab == null)
                    {
                        Logging.Logger.Log(LogLevel.Warning, "Unable to load asset " + data.bundleName + " in entity asset bundle at path " + path);
                        continue;
                    }
                    if (pool[entity.key].ContainsKey(data.peerType) == false)
                        pool[entity.key].Add(data.peerType, new Stack<GameObject>());

                    for(int i = 0; i < data.poolSize; i ++)
                        pool[entity.key][data.peerType].Push(prefab);
                    
                    
                }
            }
#endif
        }
    }
}
#endif