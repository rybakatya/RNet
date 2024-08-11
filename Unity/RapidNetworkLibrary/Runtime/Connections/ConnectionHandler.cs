using RapidNetworkLibrary.Extensions;
using RapidNetworkLibrary.Logging;

using RapidNetworkLibrary.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;



namespace RapidNetworkLibrary.Connections
{
    
    public class ConnectionHandler
    {
       
        private WorkerCollection workers;
        Dictionary<uint, Connection> connections = new Dictionary<uint, Connection>();

        private readonly ExtensionManager _extensionManager;
        internal ConnectionHandler(WorkerCollection wrk, ExtensionManager extensionManager)
        {
            workers = wrk; 
            _extensionManager = extensionManager;
        }


        internal Connection GetConnection(uint id)
        {
            if (connections.ContainsKey(id))
            {
                return connections[id];
            }
            else
            {
                throw new Exception("Id has not yet connected");
            }
        }



        internal unsafe void HandleSocketConnection(uint peerID, NativeString ip, ushort port)

        {

            var c = Connection.Create(peerID, ip, port);

#if SERVER
            
            
           

            connections.Add(peerID, c);
            if (workers.logicWorker.onSocketConnect != null)
                workers.logicWorker.onSocketConnect(c);
#elif CLIENT
            connections.Add(peerID, c);
            if (workers.logicWorker.onConnectedToServer != null)
                workers.logicWorker.onConnectedToServer(c);

#endif

            _extensionManager.OnSocketConnect(ThreadType.Logic, c);
            workers.gameWorker.Enqueue((ushort)WorkerThreadMessageID.SendConnection, c);
        }

        internal void HandleDisconnect(uint peer)
        {
            _extensionManager.OnSocketDisconnect(ThreadType.Logic, GetConnection(peer));
            Connection.Destroy(connections[peer]);
            connections.Remove(peer);
            Logger.Log(LogLevel.Info, "A socket disconnected");
        }

        internal void HandleTimeout(uint peer)
        {
            _extensionManager.OnSocketTimeout(ThreadType.Logic, GetConnection(peer));
            Connection.Destroy(connections[peer]);
            connections.Remove(peer);
            Logger.Log(LogLevel.Info, "A socket timed out");
        }

        internal void SetConnection(uint id, Connection con)
        {
            Connection.Destroy(connections[id]);
            connections[id] = con;
        }

        internal void Destroy()
        {
            
            foreach(var kvp in connections)
            {
                kvp.Value.IpAddress.Free();
            }
        }
    }
}



