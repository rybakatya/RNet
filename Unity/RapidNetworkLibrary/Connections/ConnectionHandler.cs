using ENet;
using Newtonsoft.Json;
using RapidNetworkLibrary.Logging;
using RapidNetworkLibrary.Serialization;
using System;
using System.Collections.Generic;
using System.IO;


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


#if SERVER
        public unsafe void HandleSocketConnection(uint peerID, NativeString ip, ushort port)
#elif CLIENT
        public unsafe void HandleSocketConnection(uint peerID, ConnectionType connectionType, NativeString ip, ushort port)
#endif
        {
            var conType = ConnectionType.Client;


#if SERVER
            
            if (workers.logicWorker.getConnectionType != null)
            {
               conType = workers.logicWorker.getConnectionType(ip.ToString(), port);
            }
            else
            {
                Logger.Log(LogLevel.Exception, "Must call RNet.RegisterGetConnectionTypeEvent before accepting any connections when building as a server.");
                return;
            }
            var c = Connection.Create(peerID, conType, ip.ToString(), port);
            
            RNet.SendReliable(c, NetworkMessages.informConnectionType, new InformConnectionType()
            {
                type = RNet.connectionType
            });

            connections.Add(peerID, c);

            if (conType == ConnectionType.Client)
            {
                if(workers.logicWorker.onClientConnected != null)
                    workers.logicWorker.onClientConnected(c);
            }

            else if(conType == ConnectionType.Server || conType == ConnectionType.SubServer)
            {
                workers.logicWorker.onServerConnected(c);
            }
#elif CLIENT
            conType = connectionType;
            var c = Connection.Create(peerID, connectionType, ip.ToString(), port);
            connections.Add(peerID, c);

            if(connectionType == ConnectionType.Server || conType == ConnectionType.SubServer)
            {
                if(workers.logicWorker.onConnectedToServer != null)
                    workers.logicWorker.onConnectedToServer(c);
            }
#endif
            ip.Free();

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
    }
}



