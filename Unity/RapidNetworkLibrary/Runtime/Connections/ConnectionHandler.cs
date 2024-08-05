using ENet;
using RapidNetworkLibrary.Logging;
using RapidNetworkLibrary.Runtime.Zones;
using RapidNetworkLibrary.Serialization;
using RapidNetworkLibrary.Threading.ThreadMessages;
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
                conType = workers.logicWorker.getConnectionType(new RNetIPAddress(ip.ToString(), port));
            }
            else
            {
                Logger.Log(LogLevel.Exception, "Must call RNet.RegisterGetConnectionTypeEvent before accepting any connections when building as a server.");
                return;
            }
            var c = Connection.Create(peerID, conType, ip, port);
            
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

            else if(conType == ConnectionType.Server)
            {
                workers.logicWorker.onServerConnected(c);
            }
#elif CLIENT
            conType = connectionType;
            var c = Connection.Create(peerID, connectionType, ip, port);
            connections.Add(peerID, c);

            if(connectionType == ConnectionType.Server)
            {
                if(workers.logicWorker.onConnectedToServer != null)
                    workers.logicWorker.onConnectedToServer(c);
            }
#endif
           

            workers.gameWorker.Enqueue(WorkerThreadMessageID.SendConnection, new SendConnectionDataThreadMessage() { id = c.ID});
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
                kvp.Value.ipAddress.Free();
            }
        }
    }
}



