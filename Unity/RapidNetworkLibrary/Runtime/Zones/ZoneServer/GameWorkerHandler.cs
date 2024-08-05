#if SERVER

using RapidNetworkLibrary.Connections;

namespace RapidNetworkLibrary.Runtime.Zones
{
    internal class GameWorkerHandler
    {
        private readonly CellServerManager _cellServerManager;
        public GameWorkerHandler(MapSettings mapSettings)
        {
            _cellServerManager = new CellServerManager(mapSettings);
        }
        internal void OnServerConnect(Connection connection)
        {
            if(connection.ConnectionType == ConnectionType.CellServer)
            {
                _cellServerManager.HandleCellServerConnection(connection);
            }
        }
    }
}
#endif