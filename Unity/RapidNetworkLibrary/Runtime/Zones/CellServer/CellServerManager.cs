#if SERVER
using RapidNetworkLibrary.Connections;
using RapidNetworkLibrary.Logging;
using System.Collections.Generic;


namespace RapidNetworkLibrary.Runtime.Zones
{
    internal class CellServerManager
    {
        private Stack<Connection> cellsAwaitingAssignment = new Stack<Connection>();
        private ServerSpatialHash serverGrid;
        internal CellServerManager(MapSettings settings)
        {
            serverGrid = new ServerSpatialHash(settings.minX, settings.minY, settings.width, settings.height, settings.cellSize, this);
            Logger.Log(LogLevel.Info, "Cell Server Manager Initialized");     
        }
        public void HandleCellServerConnection(Connection connection)
        {
            cellsAwaitingAssignment.Push(connection);
        }

        public Connection RequestCellServer()
        {
            
            return cellsAwaitingAssignment.Pop();
        }
          
        
    }
}
#endif