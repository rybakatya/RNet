using RapidNet.Extensions;
using RapidNet.Logging;

using RapidNet.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;



namespace RapidNet.Connections
{
    
    internal class ConnectionHandler
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


            _extensionManager.OnSocketConnect(ThreadType.Logic, c);

            connections.Add(peerID, c);

            bool value = false;
            if (workers.logicWorker.onSocketConnect != null)
                value = workers.logicWorker.onSocketConnect(c);


            if(value == false)
                workers.gameWorker.Enqueue((ushort)WorkerThreadEventID.SendConnection, c);
        }

        internal void HandleDisconnect(uint peer)
        {
            _extensionManager.OnSocketDisconnect(ThreadType.Logic, GetConnection(peer));
            bool value = false;
            if (workers.logicWorker.onSocketDisconnect != null)
                value = workers.logicWorker.onSocketDisconnect(connections[peer]);

            if(value == false)
                workers.gameWorker.Enqueue((ushort)WorkerThreadEventID.SendDisconnection, connections[peer]);

            Connection.Destroy(connections[peer]);
            connections.Remove(peer);
            Logger.Log(LogLevel.Info, "A socket disconnected");
        }

        internal void HandleTimeout(uint peer)
        {
            _extensionManager.OnSocketTimeout(ThreadType.Logic, GetConnection(peer));

            bool value = false;
            if (workers.logicWorker.onSocketTimeout != null)
                value = workers.logicWorker.onSocketDisconnect(connections[peer]);

            if(value == false)
                workers.gameWorker.Enqueue((ushort)WorkerThreadEventID.SendTimeout, connections[peer]);
            
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



