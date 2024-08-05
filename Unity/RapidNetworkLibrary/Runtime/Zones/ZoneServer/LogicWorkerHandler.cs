#if SERVER
using RapidNetworkLibrary.Connections;
using System.Diagnostics;

namespace RapidNetworkLibrary.Runtime.Zones
{
    internal class LogicWorkerHandler
    {

        public LogicWorkerHandler()
        {


        }
        public void OnServerConnect(Connection connection)
        {
            if(connection.ConnectionType == ConnectionType.CellServer)
            {

            }
        }
    }
}
#endif