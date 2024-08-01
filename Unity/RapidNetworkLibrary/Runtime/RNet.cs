using System;
using System.Runtime.InteropServices;
using ENet;
using RapidNetworkLibrary.Connections;
using RapidNetworkLibrary.Logging;
using RapidNetworkLibrary.Threading.ThreadMessages;
using RapidNetworkLibrary.Workers;
using Smmalloc;


namespace RapidNetworkLibrary
{
    public static class RNet
    {
        private static WorkerCollection workers = new WorkerCollection();

        private static Action onInit;
        internal static ConnectionType connectionType;
        public static void Init(Action initAction, ConnectionType conType)
        {
            connectionType = conType;
            SmmallocInstance smmalloc = new SmmallocInstance(8, 4 * 1024 * 1024);
            MemoryHelper.SetMalloc(smmalloc);
            workers.socketWorker = new SocketWorkerThread(OnSocketInit, workers, smmalloc);
            onInit += initAction;

        }


        public static void Tick()
        {
            workers.gameWorker.Tick();
        }
        private static void OnSocketInit()
        {
            Logger.Log(LogLevel.Info, "Network Thread Initialized!");
            workers.logicWorker = new LogicWorkerThread(OnLogicInit, workers);


        }

        private static void OnLogicInit()
        {
            Logger.Log(LogLevel.Info, "Logic Thread Initialized!");
            workers.gameWorker = new GameWorker();
            Logger.Log(LogLevel.Info, "Main Thread Initialized!");
            Logger.Log(LogLevel.Info, "RNetInitialized!");
            if (onInit != null)
                onInit();

        }

#if SERVER
        public static void InitializeServer(string ip, ushort port, byte maxChannels, ushort maxConnections)
        {

            var serverInit = new InitializeServer();
            serverInit.ip = new NativeString(ip);
            serverInit.port = port;
            serverInit.maxChannels = maxChannels;
            serverInit.maxConnections = maxConnections;

            workers.socketWorker.Enqueue(WorkerThreadMessageID.SendInitializeServer, serverInit);
        }

        internal static void Disconnect(uint connection)
        {
            workers.socketWorker.Enqueue(WorkerThreadMessageID.SendDisconnection, connection);
        }
        public static void Disconnect(Connection connection)
        {
            workers.socketWorker.Enqueue(WorkerThreadMessageID.SendDisconnection, connection.ID);
        }
#elif CLIENT
        public static void InitializeClient(byte maxChannels)
        {
            workers.socketWorker.Enqueue(WorkerThreadMessageID.SendInitClient, maxChannels);
        }
#endif
        public static void Disconnect()
        {
            workers.socketWorker.Enqueue(WorkerThreadMessageID.SendDisconnectionFromPeers);
        }
        public static void Connect(string ip, ushort port)
        {
            var address = new NativeString(ip);
            var connectMessage = new ConnectToSocketThreadMessage()
            {
                ip = address,
                port = port
            };
            workers.socketWorker.Enqueue(WorkerThreadMessageID.SendConnectToSocket, connectMessage);

        }
        private static void SendMessage<T>(uint target, ushort messageID, byte channel, PacketFlags flags, T message) where T : unmanaged, IMessageObject
        {
            var msg = new SerializeNetworkMessageThreadMessage()
            {
                target = target,
                id = messageID,
                messageObjectPointer = MemoryHelper.Write(message),
                channel = channel,
            };
            if(msg.flags.HasFlag(PacketFlags.NoAllocate))
                msg.flags |= PacketFlags.NoAllocate;
            workers.logicWorker.Enqueue(WorkerThreadMessageID.SendSerializeNetworkMessage, msg);
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
        public static void RegisterOnClientConnectEvent(Action<Connection> clientConnectLogicAction = null, Action<Connection> clientConnectGameAction = null)
        {
            if (clientConnectLogicAction != null)
                workers.logicWorker.onClientConnected += clientConnectLogicAction;

            if(clientConnectGameAction != null)
                workers.gameWorker.onClientConnected += clientConnectGameAction; 
        }

        public static void RegisterOnServerConnectEvent(Action<Connection> serverConnectLogicAction = null, Action<Connection> serverConnectGameAction = null)
        {
            if(serverConnectLogicAction != null)
                workers.logicWorker.onServerConnected += serverConnectLogicAction;

            if (serverConnectGameAction != null)
                workers.gameWorker.onServerConnected += serverConnectGameAction;
        }
        public static void RegisterGetConnectionTypeEvent(GetConnectionTypeDelegate connection)
        {
            workers.logicWorker.getConnectionType += connection;
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

    public struct SerializeNetworkMessageThreadMessage
    {
        public uint target;
        public ushort id;
        public byte channel;
        public PacketFlags flags;
        public IntPtr messageObjectPointer;
    }


}



