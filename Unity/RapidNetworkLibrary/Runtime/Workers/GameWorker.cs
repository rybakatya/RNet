using RapidNetworkLibrary.Connections;
using RapidNetworkLibrary.Logging;
using RapidNetworkLibrary.Memory;
using RapidNetworkLibrary.Threading;
using RapidNetworkLibrary.Threading.ThreadMessages;
using System;
using System.Diagnostics;
using System.Threading;


namespace RapidNetworkLibrary.Workers
{
    internal class GameWorker : Worker
    {
#if SERVER
        public Action<Connection> onSocketConnected;

        public Action<uint> test;
#elif CLIENT
        public Action<Connection> onConnectedToServer;
#endif
        public Action<Connection, ushort, IntPtr> onSocketReceive;
        

        internal override void OnConsume(WorkerThreadMessageID message, IntPtr data)
        {
            
            switch (message)
            {
                case WorkerThreadMessageID.SendConnection:
                    var connection = MemoryHelper.Read<Connection>(data);
                   
#if SERVER
                    
                    
                        if(onSocketConnected != null)
                            onSocketConnected(connection);
                    
#elif CLIENT
                    if(onConnectedToServer != null)
                        onConnectedToServer(connection);
#endif
                    
                    break;


                case WorkerThreadMessageID.SendNetworkMessageToGameThread:
                    var msgData = MemoryHelper.Read<NetworkMessageDataThreadMessage>(data);
                    Logger.Log(LogLevel.Info, "Received message id " + msgData.messageID.ToString());
                    if (onSocketReceive != null)
                        onSocketReceive(msgData.sender, msgData.messageID, msgData.messageData);

                    MemoryHelper.Free(msgData.messageData);
                    break;

                
                        
            }
        }

        public void Tick()
        {
            Consume();
            
        }
        internal override void OnDestroy()
        {

        }
    }
}



