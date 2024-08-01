using RapidNetworkLibrary.Connections;
using RapidNetworkLibrary.Workers;
using System;


namespace RapidNetworkLibrary
{
    internal class WorkerCollection
    {
        public GameWorker gameWorker;
        public SocketWorkerThread socketWorker;
        public LogicWorkerThread logicWorker;

        public Action<Connection> onClientConnect;
        
    }
}



