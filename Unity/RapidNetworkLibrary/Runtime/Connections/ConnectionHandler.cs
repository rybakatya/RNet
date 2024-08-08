using RapidNetworkLibrary.Logging;
using RapidNetworkLibrary.Serialization;
using System;
using System.Collections.Generic;



namespace RapidNetworkLibrary.Connections
{
    
    internal class ConnectionHandler
    {

        private WorkerCollection workers;
        Dictionary<uint, Connection> connections = new Dictionary<uint, Connection>();

        public ConnectionHandler(WorkerCollection wrk)
        {
            workers = wrk; 
        }
        
        public bool IsConnectionValid(uint id)
        {
            if (connections.ContainsKey(id))
                return true;
            return false;
        }
        public Connection GetConnection(uint id)
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



        public unsafe void HandleSocketConnection(uint peerID, NativeString ip, ushort port)

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
           

            workers.gameWorker.Enqueue(WorkerThreadMessageID.SendConnection, c);
        }

        public void HandleDisconnect(uint peer)
        {
            Connection.Destroy(connections[peer]);
            connections.Remove(peer);
            Logger.Log(LogLevel.Info, "A socket disconnected");
        }

        public void HandleTimeout(uint peer)
        {
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



