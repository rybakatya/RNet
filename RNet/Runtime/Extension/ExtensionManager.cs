using RapidNetworkLibrary.Connections;
using System;
using System.Collections.Generic;


namespace RapidNetworkLibrary.Extensions
{
    internal class ExtensionManager
    {
        private readonly List<RNetExtension> _extensions = new List<RNetExtension>();
        private readonly WorkerCollection _workers;

        internal ExtensionManager(WorkerCollection workers)
        {
            _workers = workers;
        }
        internal void LoadExtension<T>() where T : RNetExtension
        {
            var ext = (RNetExtension)Activator.CreateInstance(typeof(T), _workers);
            _extensions.Add(ext);
        }

        public void OnSocketConnect(ThreadType threadType, Connection connection)
        {
            foreach(var ext in _extensions)
            {
                ext.OnSocketConnect(threadType, connection);
            }
        }

        public void OnSocketDisconnect(ThreadType threadType, Connection connection)
        {
            foreach(var ext in _extensions)
            {
                ext.OnSocketDisconnect(threadType, connection);
            }
        }

        public void OnSocketTimeout(ThreadType threadType, Connection connection)
        {
            foreach(var ext in _extensions)
            {
                ext.OnSocketTimeout(threadType, connection);
            }
        }

        public bool OnSocketReceive(ThreadType threadType, Connection connection, ushort messageID, IntPtr messageData)
        {
            foreach( var ext in _extensions)
            {
                if(ext.OnSocketReceive(threadType, connection, messageID, messageData) == true)
                {
                    return true;
                }
            }
            return false;
        }

        public void OnThreadMessageReceived(ThreadType threadType, ushort id, IntPtr messageData)
        {
            foreach(var ext in _extensions)
            {
                ext.OnThreadMessageReceived(threadType, id, messageData);
            }
        }
    }
}
