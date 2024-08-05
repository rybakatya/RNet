#if SERVER
using RapidNetworkLibrary.Connections;
using RapidNetworkLibrary.Logging;
using RapidNetworkLibrary.Runtime.Memory;
using System.Collections.Generic;

namespace RapidNetworkLibrary.Runtime.Zones
{

    public class ZoneServerData
    {
        public RNetIPAddress address;
        public byte maxChannels;
        public ushort maxConnections;
        public List<RNetIPAddress> cellServers;
    }
    public abstract class ZoneServerBase
    {       
        private GameWorkerHandler _gameWorkerHandler;

        private List<RNetIPAddress> cellServerWhiteList = new List<RNetIPAddress>();
        protected readonly ZoneServerData _serverData;
        protected readonly MapSettings _mapSettings;
        public ZoneServerBase(ZoneServerData serverData, MapSettings mapSettings)
        {
            _serverData = serverData;
            _mapSettings = mapSettings;
            Logger.Log(LogLevel.Info, "Initializing Zone Server");

           
            RNet.Init(onInit, ConnectionType.ZoneServer);
            
        }

        public abstract void OnInit();
        private void onInit()
        {
            _gameWorkerHandler = new GameWorkerHandler(_mapSettings);
            RNet.RegisterGetConnectionTypeEvent(GetConnectionType);
            RNet.RegisterOnServerConnectEvent(serverConnectGameAction: _gameWorkerHandler.OnServerConnect);            
            RNet.InitializeServer(_serverData.address.ip, _serverData.address.port, _serverData.maxChannels, _serverData.maxConnections);
            OnInit();
        }

        private ConnectionType GetConnectionType(RNetIPAddress connectionAddress)
        {
            return GetIncomingConnectionType(connectionAddress);
        }

        public void Tick()
        {
            RNet.Tick();
        }

        public abstract ConnectionType GetIncomingConnectionType(RNetIPAddress incomingConnectionAddress);


    }
}
#endif