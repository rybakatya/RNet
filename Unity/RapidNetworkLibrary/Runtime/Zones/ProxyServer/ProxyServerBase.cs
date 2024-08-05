using RapidNetworkLibrary.Connections;
using RapidNetworkLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace RapidNetworkLibrary.Runtime.Zones.ProxyServer
{
    internal enum ProxyServerExitCode
    {
        DisconnectedFromZone
    }
    public class GameThreadHandler : ThreadHandler
    {
#if SERVER

        private Connection _zoneServer;

        private readonly List<Connection> _clients;

        internal GameThreadHandler(int maxConnections)
        {
            _clients = new List<Connection>(maxConnections);
        }
        internal override void OnClientConnected(Connection connection)
        {
           _clients.Add(connection);
        }

        internal override void OnClientDisconnected(Connection connection)
        {
            _clients.Remove(connection);
        }

      

        internal override void OnServerConnected(Connection connection)
        {
            if(connection.ConnectionType == ConnectionType.ZoneServer)
            {
                _zoneServer = connection;
                Logger.Log(LogLevel.Info, "Connected to the zone server!");
            }
        }

        internal override void OnServerDisconnected(Connection connection)
        {
            if(connection.Equals(_zoneServer))
            {
                Logger.Log(LogLevel.Error, "Zone server disconnected! Shutting down.");
                System.Environment.Exit((int)ProxyServerExitCode.DisconnectedFromZone);
            }
        }
#endif
        internal override void OnReceived(Connection connection, ushort messageID, IntPtr messageData)
        {

        }

        internal override void Tick()
        {
            
        }
    }

    public abstract class ThreadHandler
    {

        internal abstract void Tick();
        internal abstract void OnReceived(Connection connection, ushort messageID, IntPtr messageData);

#if SERVER
        internal abstract void OnServerConnected(Connection connection);
        internal abstract void OnServerDisconnected(Connection connection);
        internal abstract void OnClientConnected(Connection connection);
        internal abstract void OnClientDisconnected(Connection connection);
#endif
    }

    [Serializable]
    public class ProxyServerData
    {
        public RNetIPAddress address;
        public byte maxChannels;
        public ushort maxConnections;

        public RNetIPAddress zoneServer;
    }
    public abstract class ProxyServerBase
    {
        private readonly ProxyServerData _serverData;
        private readonly MapSettings _mapSettings;

        private bool isInit = false;

        private readonly ThreadHandler _gameThreadHandler;
        public ProxyServerBase(ProxyServerData serverData, MapSettings mapSettings)
        {
            _gameThreadHandler = new GameThreadHandler(serverData.maxConnections);
            _serverData = serverData;
            _mapSettings = mapSettings;
            Logger.Log(LogLevel.Info, "Initializing Proxy Server");
            RNet.Init(onInit, ConnectionType.ZoneServer);
        }

        private void onInit()
        {
            RNet.RegisterGetConnectionTypeEvent(GetIncomingConnectionType);
            RNet.RegisterOnServerConnectEvent(serverConnectGameAction: _gameThreadHandler.OnServerConnected);
            RNet.RegisterReceiveEvent(gameReceiveAction: _gameThreadHandler.OnReceived);
            RNet.RegisterOnClientConnectEvent(clientConnectGameAction: _gameThreadHandler.OnClientConnected);
            
            RNet.InitializeServer(_serverData.address.ip, _serverData.address.port, _serverData.maxChannels, _serverData.maxConnections);
            RNet.Connect(_serverData.zoneServer.ip,_serverData.zoneServer.port);
            isInit = true;
        }

        public void Tick()
        {
            if (!isInit)
                return;
            RNet.Tick();
            _gameThreadHandler.Tick();
        }

        private ConnectionType GetIncomingConnectionType(RNetIPAddress address)
        {
            if(address.ip.Equals(_serverData.zoneServer.ip) && address.port == _serverData.zoneServer.port)
            {
                return ConnectionType.ZoneServer;
            }
            return ConnectionType.Client;
        }
    }
}
