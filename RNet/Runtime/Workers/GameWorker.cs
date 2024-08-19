using RapidNet.Connections;
using RapidNet.Extensions;
using RapidNet.Logging;
using RapidNet.Memory;
using RapidNet.Threading;
using RapidNet.Threading.ThreadEvents;
using System;

namespace RapidNet.Workers
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
        private readonly Action onInit;
        private bool isInit = false;
        ///
        internal GameWorker(Action initAction, WorkerCollection workers, ExtensionManager extensionManager)
        {
            _workers = workers;
            _extensionManager = extensionManager;
            onInit += initAction;
        }

        public override void OnConsume(ushort eventID, IntPtr data)
        {
           
            switch (eventID)
            {
                case WorkerThreadEventID.SendConnection:
                    var connection = MemoryHelper.Read<Connection>(data);
                    _extensionManager.OnSocketConnect(ThreadType.Game,connection);
                    
                    if(onSocketConnected != null)
                        onSocketConnected(connection);                  
                    break;

                case WorkerThreadEventID.SendDisconnection:
                    var con = MemoryHelper.Read<Connection>(data);
                    _extensionManager.OnSocketDisconnect(ThreadType.Game,con);
                    if(onSocketDisconnected != null) 
                        onSocketDisconnected(con);
                    break;


                case WorkerThreadEventID.SendTimeout:
                    var c = MemoryHelper.Read<Connection>(data);
                    _extensionManager.OnSocketTimeout(ThreadType.Game, c);
                    if(onSocketTimedout != null)
                        onSocketTimedout(c);
                    break;

                case WorkerThreadEventID.SendNetworkMessageToGameThread:
                    var msgData = MemoryHelper.Read<NetworkMessageDataThreadEvent>(data);
                    Logging.Logger.Log(LogLevel.Info, "Received message id " + msgData.messageID.ToString());

                    var value = _extensionManager.OnSocketReceive(ThreadType.Game, msgData.sender, msgData.messageID, msgData.messageData);
                    if (value == false)
                    {

                        if (onSocketReceive != null)
                            onSocketReceive(msgData.sender, msgData.messageID, msgData.messageData);
                    }

                    MemoryHelper.Free(msgData.messageData);
                    break;

                default:
                    _extensionManager.OnThreadEventReceived(ThreadType.Game, eventID, data);
                    break;



            }
        }

        internal void Tick()
        {
            if (!isInit)
            {
                if (onInit != null)
                    onInit();
                isInit = true;
            }

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



