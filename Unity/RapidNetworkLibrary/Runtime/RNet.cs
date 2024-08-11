using System;
using ENet;
using RapidNetworkLibrary.Connections;
using RapidNetworkLibrary.Extensions;
using RapidNetworkLibrary.Logging;
using RapidNetworkLibrary.Memory;


using RapidNetworkLibrary.Runtime.Threading.ThreadMessages;
using RapidNetworkLibrary.Threading.ThreadMessages;
using RapidNetworkLibrary.Workers;


namespace RapidNetworkLibrary
{

    public static class RNet
    {
        private static WorkerCollection workers = new WorkerCollection();

        private static Action onInit;
        private static bool isInit;
        private static ExtensionManager extensionManager;
        public unsafe static void Init(Action initAction, MemoryAllocator alloc = null)
        {
            
            
            if (alloc == null)
            {
#if ENABLE_MONO || ENABLE_IL2CPP
                MemoryHelper.SetMalloc(new UnityAllocator());
#else
                MemoryHelper.SetMalloc(new RNetAllocator());
#endif
            }

            extensionManager = new ExtensionManager(workers);
            workers.socketWorker = new SocketWorkerThread(OnSocketInit, workers, extensionManager);
            workers.socketWorker.StartThread(20);
            onInit += initAction;

        }

        public static void TearDown()
        {

            workers.gameWorker.OnDestroy();

            workers.logicWorker.OnDestroy();

            workers.socketWorker.OnDestroy();

            isInit = false;

        }

        public static void RegisterExtension<T>() where T : RNetExtension
        {
            extensionManager.LoadExtension<T>();
        }

        public static void Tick()
        {
            if (!isInit)
                return;
            workers.gameWorker.Tick();
        }
        private static void OnSocketInit()
        {       
            Logger.Log(LogLevel.Info, "Network Thread Initialized!");
            
            workers.logicWorker = new LogicWorkerThread(OnLogicInit, workers, extensionManager);
            workers.logicWorker.StartThread(20);
        }

        private static void OnLogicInit()
        {
            Logger.Log(LogLevel.Info, "Logic Thread Initialized!");

            workers.gameWorker = new GameWorker(workers, extensionManager);
            workers.gameWorker.shouldRun = true;
            Logger.Log(LogLevel.Info, "Main Thread Initialized!");
            
            
            Logger.Log(LogLevel.Info, "RNetInitialized!");
            if (onInit != null)
                onInit();

            
            isInit = true;


        }

        private static void OnEntityThreadInit()
        {
            Logger.Log(LogLevel.Info, "Entity Thread Is Initialized!");
            
        }

#if SERVER


        public static void InitializeServer(string ip, ushort port, byte maxChannels, ushort maxConnections)
        {

            var serverInit = new InitializeServer();
            serverInit.ip = new NativeString(ip);
            serverInit.port = port;
            serverInit.maxChannels = maxChannels;
            serverInit.maxConnections = maxConnections;

            workers.socketWorker.Enqueue((ushort)WorkerThreadMessageID.SendInitializeServer, serverInit);
        }

        internal static void Disconnect(uint connection)
        {
            workers.socketWorker.Enqueue((ushort)WorkerThreadMessageID.SendDisconnection, connection);
        }
        public static void Disconnect(Connection connection)
        {
            workers.socketWorker.Enqueue((ushort)WorkerThreadMessageID.SendDisconnection, connection.ID);
        }
#elif CLIENT
        public static void InitializeClient(byte maxChannels)
        {
            workers.socketWorker.Enqueue((ushort)WorkerThreadMessageID.SendInitClient, maxChannels);
        }
#endif
        public static void Disconnect()
        {
            workers.socketWorker.Enqueue((ushort)WorkerThreadMessageID.SendDisconnectionFromPeers);
        }
        public static void Connect(string ip, ushort port)
        {
            var address = new NativeString(ip);
            var connectMessage = new ConnectToSocketThreadMessage()
            {
                ip = address,
                port = port
            };
            workers.socketWorker.Enqueue((ushort)WorkerThreadMessageID.SendConnectToSocket, connectMessage);

        }


        internal static void SendMessage<T>(uint target, ushort messageID, byte channel, PacketFlags flags, T message) where T : unmanaged, IMessageObject
        {
            var ptr = MemoryHelper.Write(message);

            
            var msg = new SerializeNetworkMessageThreadMessage()
            {
                target = target,
                id = messageID,
                messageObjectPointer = ptr,
                channel = channel,
                flags = flags | PacketFlags.NoAllocate
            };

            workers.logicWorker.Enqueue((ushort)WorkerThreadMessageID.SendSerializeMessage, msg);
        }
        
        public static void SendUnreliable<T>(Connection target, ushort messageID, byte channel, T message) where T : unmanaged, IMessageObject
        {
            SendMessage(target.ID, messageID, channel, PacketFlags.None, message);
        }

        public static void SendUnreliable<T>(Connection target, ushort messageID, T message) where T : unmanaged, IMessageObject
        {
            SendMessage(target.ID, messageID, 237, PacketFlags.None, message);
        }
        
        public static void SendReliable<T>(Connection target, ushort messageID, byte channel, T message) where T : unmanaged, IMessageObject
        {
            SendMessage(target.ID, messageID, channel, PacketFlags.Reliable, message);
        }
        public static void SendReliable<T>(Connection target, ushort messageID, T message) where T : unmanaged, IMessageObject
        {
            SendReliable(target, messageID, 238, message);
        }


        public static void BroadcastReliable<T>(ushort messageID, byte channel, T message) where T : unmanaged, IMessageObject
        {
            SendMessage(uint.MaxValue, messageID, channel, PacketFlags.Reliable, message);
        }

        public static void BroadcastReliable<T>(ushort messageID, T message) where T : unmanaged, IMessageObject
        {
            BroadcastReliable(messageID, 239, message);
        }

        public static void BroadcastUnreliable<T>(ushort messageID, byte channel, T message) where T : unmanaged, IMessageObject
        {
            SendMessage(uint.MaxValue, messageID, channel, PacketFlags.None, message);
        }

        public static void BroadcastUnreliable<T>(ushort messageID, T message) where T : unmanaged, IMessageObject
        {
            BroadcastUnreliable(messageID, 240, message);
        }

        public static void RegisterReceiveEvent(Action<Connection, ushort, IntPtr> logicReceiveAction = null, Action<Connection, ushort, IntPtr> gameReceiveAction = null)
        {
            if (logicReceiveAction != null)
                workers.logicWorker.onSocketReceive += logicReceiveAction;

            if(gameReceiveAction != null)
                workers.gameWorker.onSocketReceive += gameReceiveAction;
        }

        public static void UnRegisterReceiveEvent(Action<Connection, ushort, IntPtr> logicReceiveAction, Action<Connection, ushort, IntPtr> gameReceiveAction)
        {

            if(logicReceiveAction != null)
                workers.logicWorker.onSocketReceive -= logicReceiveAction;


            if (gameReceiveAction != null)
                workers.gameWorker.onSocketReceive += gameReceiveAction;
        }



#if SERVER


        public static void RegisterOnSocketConnect(Action<Connection> socketConnectLogicAction = null, Action<Connection> socketConnectGameAction = null)
        {
            if(socketConnectLogicAction != null)
                workers.logicWorker.onSocketConnect += socketConnectLogicAction;

            if (socketConnectGameAction != null)
                workers.gameWorker.onSocketConnected += socketConnectGameAction;
        }
        
#elif CLIENT
        public static void RegisterOnConnectedToServerEvent(Action<Connection> logicConnectedToServerAction = null, Action<Connection> gameConnectedToServerAction = null)
        {
            if(logicConnectedToServerAction != null)
                workers.logicWorker.onConnectedToServer += logicConnectedToServerAction;

            if (gameConnectedToServerAction != null)
                workers.gameWorker.onConnectedToServer += gameConnectedToServerAction;
        }

#endif
    }


}



