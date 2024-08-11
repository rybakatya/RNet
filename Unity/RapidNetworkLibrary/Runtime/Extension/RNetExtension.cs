using RapidNetworkLibrary.Connections;
using System;
using System.Collections;
using System.Collections.Generic;


namespace RapidNetworkLibrary.Extensions
{

    public enum ThreadType
    {
        Game,
        Logic,
        Network
    }
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

        public void OnSocketReceive(ThreadType threadType, Connection connection, ushort messageID, IntPtr messageData)
        {
            foreach( var ext in _extensions)
            {
                ext.OnSocketReceive(threadType, connection, messageID, messageData);
            }
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
