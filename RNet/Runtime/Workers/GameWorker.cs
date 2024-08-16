using RapidNetworkLibrary.Connections;
using RapidNetworkLibrary.Extensions;
using RapidNetworkLibrary.Logging;
using RapidNetworkLibrary.Memory;
using RapidNetworkLibrary.Threading;
using RapidNetworkLibrary.Threading.ThreadEvents;
using System;

namespace RapidNetworkLibrary.Workers
{
    /// <summary>
    /// 
    /// </summary>
    public class GameWorker : Worker
    {

        internal Action<Connection> onSocketConnected;
        internal Action<Connection> onSocketDisconnected;
        internal Action<Connection> onSocketTimedout;

        internal Action<Connection, ushort, IntPtr> onSocketReceive;
        

        private readonly WorkerCollection _workers;
        private readonly ExtensionManager _extensionManager;
        
        ///
        internal GameWorker(WorkerCollection workers, ExtensionManager extensionManager)
        {
            _workers = workers;
            _extensionManager = extensionManager;
        }

        internal override void OnConsume(ushort eventID, IntPtr data)
        {
            
            switch (eventID)
            {
                case (ushort)WorkerThreadEventID.SendConnection:
                    var connection = MemoryHelper.Read<Connection>(data);
                    _extensionManager.OnSocketConnect(ThreadType.Game,connection);
                    
                    if(onSocketConnected != null)
                        onSocketConnected(connection);                  
                    break;

                case (ushort)WorkerThreadEventID.SendDisconnection:
                    var con = MemoryHelper.Read<Connection>(data);
                    _extensionManager.OnSocketDisconnect(ThreadType.Game,con);
                    if(onSocketDisconnected != null) 
                        onSocketDisconnected(con);
                    break;


                case (ushort)WorkerThreadEventID.SendTimeout:
                    var c = MemoryHelper.Read<Connection>(data);
                    _extensionManager.OnSocketTimeout(ThreadType.Game, c);
                    if(onSocketTimedout != null)
                        onSocketTimedout(c);
                    break;

                case (ushort)WorkerThreadEventID.SendNetworkMessageToGameThread:
                    var msgData = MemoryHelper.Read<NetworkMessageDataThreadEvent>(data);
                    Logging.Logger.Log(LogLevel.Info, "Received message id " + msgData.messageID.ToString());
                    

                    if (onSocketReceive != null)
                        onSocketReceive(msgData.sender, msgData.messageID, msgData.messageData);

                    MemoryHelper.Free(msgData.messageData);
                    break;

                default:
                    _extensionManager.OnThreadEventReceived(ThreadType.Game, eventID, data);
                    break;



            }
        }

        internal void Tick()
        {
            Consume();           
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnDestroy()
        {

        }
    }
}



