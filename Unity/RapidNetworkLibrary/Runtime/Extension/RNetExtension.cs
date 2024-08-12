using RapidNetworkLibrary.Connections;
using System;
using System.Collections;


namespace RapidNetworkLibrary.Extensions
{
    public abstract class RNetExtension 
    {
        private WorkerCollection _workers;

        public RNetExtension(WorkerCollection workers)
        {
            _workers = workers;
        }

        public abstract void OnSocketConnect(ThreadType threadType, Connection connection);

        public abstract void OnSocketDisconnect(ThreadType threadType, Connection connection);

        public abstract void OnSocketTimeout(ThreadType threadType, Connection connection);

        public abstract void OnSocketReceive(ThreadType threadType, Connection sender, ushort messageID, IntPtr messageData);

        public abstract void OnThreadMessageReceived(ThreadType threadType, ushort id, IntPtr messageData);
    }
}
