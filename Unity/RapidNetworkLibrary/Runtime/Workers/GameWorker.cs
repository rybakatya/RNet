using RapidNetworkLibrary.Connections;
using RapidNetworkLibrary.Extensions;
using RapidNetworkLibrary.Logging;
using RapidNetworkLibrary.Memory;
using RapidNetworkLibrary.Threading;
using RapidNetworkLibrary.Threading.ThreadMessages;
using System;

namespace RapidNetworkLibrary.Workers
{
    public class GameWorker : Worker
    {

        public Action<Connection> onSocketConnected;
        public Action<Connection> onSocketDisconnected;
        public Action<Connection> onSocketTimedout;

        public Action<Connection, ushort, IntPtr> onSocketReceive;
        

        private readonly WorkerCollection _workers;
        private readonly ExtensionManager _extensionManager;
        
        internal GameWorker(WorkerCollection workers, ExtensionManager extensionManager)
        {
            _workers = workers;
            _extensionManager = extensionManager;
        }

        internal override void OnConsume(ushort message, IntPtr data)
        {
            
            switch (message)
            {
                case (ushort)WorkerThreadMessageID.SendConnection:
                    var connection = MemoryHelper.Read<Connection>(data);
                    _extensionManager.OnSocketConnect(ThreadType.Game,connection);
                    
                    if(onSocketConnected != null)
                        onSocketConnected(connection);                  
                    break;

                case (ushort)WorkerThreadMessageID.SendDisconnection:
                    var con = MemoryHelper.Read<Connection>(data);
                    _extensionManager.OnSocketDisconnect(ThreadType.Game,con);
                    if(onSocketDisconnected != null) 
                        onSocketDisconnected(con);
                    break;


                case (ushort)WorkerThreadMessageID.SendTimeout:
                    var c = MemoryHelper.Read<Connection>(data);
                    _extensionManager.OnSocketTimeout(ThreadType.Game, c);
                    if(onSocketTimedout != null)
                        onSocketTimedout(c);
                    break;

                case (ushort)WorkerThreadMessageID.SendNetworkMessageToGameThread:
                    var msgData = MemoryHelper.Read<NetworkMessageDataThreadMessage>(data);
                    Logging.Logger.Log(LogLevel.Info, "Received message id " + msgData.messageID.ToString());
                    

                    if (onSocketReceive != null)
                        onSocketReceive(msgData.sender, msgData.messageID, msgData.messageData);

                    MemoryHelper.Free(msgData.messageData);
                    break;

                default:
                    _extensionManager.OnThreadMessageReceived(ThreadType.Game, message, data);
                    break;



            }
        }

        internal void Tick()
        {
            Consume();           
        }
        public override void OnDestroy()
        {

        }
    }
}



