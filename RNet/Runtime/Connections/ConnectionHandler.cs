using RapidNet.Logging;
using System;
using System.Collections.Generic;




namespace RapidNet.Connections
{
    
    internal class ConnectionHandler
    {
      
        private WorkerCollection workers;
        Dictionary<uint, Connection> connections;

        
        internal ConnectionHandler(WorkerCollection wrk, ushort maxPlayers)
        {
            workers = wrk; 
            connections = new Dictionary<uint, Connection>(maxPlayers);
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



        internal unsafe void HandleSocketConnection(ushort peerID, NativeString ip, ushort port)

        {

            var c = Connection.Create(peerID, ip, port);


            

            connections.Add(peerID, c);

            bool value = false;
            if (workers.logicWorker.onSocketConnect != null)
                value = workers.logicWorker.onSocketConnect(c);


            if(value == false)
                workers.gameWorker.Enqueue(WorkerThreadEventID.SendConnection, c);
        }

        internal void HandleDisconnect(uint peer)
        {
            
            bool value = false;
            if (workers.logicWorker.onSocketDisconnect != null)
                value = workers.logicWorker.onSocketDisconnect(connections[peer]);

            if(value == false)
                workers.gameWorker.Enqueue(WorkerThreadEventID.SendDisconnection, connections[peer]);

            Connection.Destroy(connections[peer]);
            connections.Remove(peer);
            Logger.Log(LogLevel.Info, "A socket disconnected");
        }

        internal void HandleTimeout(uint peer)
        {
           

            bool value = false;
            if (workers.logicWorker.onSocketTimeout != null)
                value = workers.logicWorker.onSocketDisconnect(connections[peer]);

            if(value == false)
                workers.gameWorker.Enqueue(WorkerThreadEventID.SendTimeout, connections[peer]);
            
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

        internal bool IsSet(uint target)
        {
            return connections.ContainsKey(target);
        }
    }
}



