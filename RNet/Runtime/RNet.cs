using System;
using ENet;
using RapidNet.Connections;
using RapidNet.Extensions;
using RapidNet.Logging;
using RapidNet.Memory;
using RapidNet.Serialization;
using RapidNet.Threading.ThreadEvents;
using RapidNet.Workers;


namespace RapidNet
{
    /// <summary>
    /// Static class used to initialize and manage RNet. Everything in this class is safe in all threads at all times.
    /// </summary>
    public static class RNet
    {
        private static WorkerCollection workers = new WorkerCollection();

        private static Action onInit;
        private static bool isInit;
        private static ExtensionManager extensionManager;

       
        private static ulong bytesSent;
        private static ulong bytesReceived;
        private static uint lastReceiveTime;
        private static uint lastSendTime;
        private static uint lastRoundTripTime;
        private static uint mtu;
        private static ulong packetsSent;
        private static ulong packetsLost;

        /// <summary>
        /// Returns the number of bytes sent from this machine.
        /// </summary>
        public static ulong BytesSent { get => bytesSent; internal set => bytesSent = value; }

        /// <summary>
        /// Returns the number of bytes received on this machine.
        /// </summary>
        public static ulong BytesReceived { get => bytesReceived; internal set => bytesReceived = value; }

        /// <summary>
        /// Returns the time that a packet was last received on this machine.
        /// </summary>
        public static uint LastReceiveTime { get => lastReceiveTime; internal set => lastReceiveTime = value; }

        /// <summary>
        /// Returns the time that a packet was last sent on this machine.
        /// </summary>
        public static uint LastSendTime { get => lastSendTime; internal set => lastSendTime = value; }

        /// <summary>
        /// Returns the Round Trip Time for the connection on this machine.
        /// </summary>
        public static uint LastRoundTripTime { get => lastRoundTripTime; internal set => lastRoundTripTime = value; }

        /// <summary>
        /// Returns this machines MTU
        /// </summary>
        public static uint Mtu { get => mtu; internal set => mtu = value; }

        /// <summary>
        /// Returns the number of packets sent by this machine.
        /// </summary>
        public static ulong PacketsSent { get => packetsSent; internal set => packetsSent = value; }


        /// <summary>
        /// Returns the number of packets lost by this machine.
        /// </summary>
        public static ulong PacketsLost { get => packetsLost; internal set => packetsLost = value; }



        /// <summary>
        /// Initializes RNet,  must be called and initAction  must be invoked before any other methods in RNet are called.
        /// </summary>
        /// <param name="initAction">The method invoked when RNet is finished initializing.</param>
        /// <param name="alloc">Optional parameter used to pass a custom memory allocator to be used by RNet.</param>
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


        /// <summary>
        /// Deinitializes the network and logic thread.
        /// </summary>
        public static void TearDown()
        {
            
            workers.gameWorker.OnDestroy();

            workers.logicWorker.OnDestroy();

            workers.socketWorker.OnDestroy();

            isInit = false;

        }


        /// <summary>
        /// Registers an RNet extension to be loaded, needs to be called prior to RNet.InitializeServer or RNet.InitializeClient.
        /// </summary>
        /// <typeparam name="T">The type inheriting from RNetExtension</typeparam>
        public static void RegisterExtension<T>() where T : RNetExtension
        {
            extensionManager.LoadExtension<T>();
        }


        /// <summary>
        /// Called once per frame to keep main thread events flowing from logic thread.
        /// </summary>
        public static void Tick()
        {
            if (!isInit)
            {
                if (workers.gameWorker == null)
                    return;
                
                workers.gameWorker.Tick();
            }
            else
            {
                workers.gameWorker.Tick();
            }
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

            workers.gameWorker = new GameWorker(onGameInit, workers, extensionManager);
            workers.gameWorker.shouldRun = true;
            
        }

        private static void onGameInit()
        {
            Logger.Log(LogLevel.Info, "Main Thread Initialized!");


            Logger.Log(LogLevel.Info, "RNetInitialized!");
            if (onInit != null)
                onInit();


            isInit = true;

            workers.socketWorker.Enqueue(WorkerThreadEventID.SendRegisterThreadEvent);
            workers.logicWorker.Enqueue(WorkerThreadEventID.SendRegisterThreadEvent);
            extensionManager.OnThreadRegistered(ThreadType.Game);
        }

        

#if SERVER

        /// <summary>
        /// Initializes a server to receive connections.
        /// </summary>
        /// <param name="ip">The ip address the server is bound to.</param>
        /// <param name="port">The port used to establish socket connections</param>
        /// <param name="maxChannels">
        ///     Maximum amount of channels used by this connection, must match number called in RNet.InitializeClient
        /// </param>
        /// <param name="maxConnections"></param>
        public static void InitializeServer(string ip, ushort port, byte maxChannels, ushort maxConnections)
        {

            var serverInit = new InitializeServerThreadEvent();
            serverInit.ip = new NativeString(ip);
            serverInit.port = port;
            serverInit.maxChannels = maxChannels;
            serverInit.maxConnections = maxConnections;

            workers.socketWorker.Enqueue((ushort)WorkerThreadEventID.SendInitializeServer, serverInit);
        }

        internal static void Disconnect(uint connection)
        {
            workers.socketWorker.Enqueue((ushort)WorkerThreadEventID.SendDisconnection, connection);
        }


        /// <summary>
        /// Disconnects the passed connection from the server.
        /// </summary>
        /// <param name="connection"></param>
        public static void Disconnect(Connection connection)
        {
            workers.socketWorker.Enqueue((ushort)WorkerThreadEventID.SendDisconnection, connection.ID);
        }
#elif CLIENT
        public static void InitializeClient(byte maxChannels)
        {
            workers.socketWorker.Enqueue((ushort)WorkerThreadEventID.SendInitClient, maxChannels);
        }
#endif

        /// <summary>
        /// Disconnects this machine from everything.
        /// </summary>
        public static void Disconnect()
        {
            workers.socketWorker.Enqueue((ushort)WorkerThreadEventID.SendDisconnectionFromPeers);
        }


        /// <summary>
        /// Connects this socket to a server. Can be called from clients or servers.
        /// </summary>
        /// <param name="ip">IpAddress of the server to connect to.</param>
        /// <param name="port">Port of the server to connect to.</param>
        public static void Connect(string ip, ushort port)
        {
            var address = new NativeString(ip);
            var connectMessage = new ConnectToSocketThreadEvent()
            {
                ip = address,
                port = port
            };
            workers.socketWorker.Enqueue((ushort)WorkerThreadEventID.SendConnectToSocket, connectMessage);

        }


        public static void SendMessage<T>(uint target, ushort messageID, byte channel, PacketFlags flags, T message) where T : unmanaged, IMessageObject
        {
            var ptr = MemoryHelper.Write(message);

            
            var msg = new SerializeNetworkMessageThreadEvent()
            {
                target = target,
                id = messageID,
                messageObjectPointer = ptr,
                channel = channel,
                flags = flags | PacketFlags.NoAllocate
            };

            workers.logicWorker.Enqueue(WorkerThreadEventID.SendSerializeMessageEvent, msg);
        }
        
        /// <summary>
        /// Sends an unreliable message to the target connection.
        /// </summary>
        /// <param name="target">Target connection to send the message to.</param>
        /// <param name="messageID">MessageID of the messasge being sent.</param>
        /// <param name="channel">Channel to send the message on.</param>
        /// <param name="message">MessageObject to send.</param>
        public static void SendUnreliable<T>(Connection target, ushort messageID, byte channel, T message) where T : unmanaged, IMessageObject
        {
            SendMessage(target.ID, messageID, channel, PacketFlags.None, message);
        }

        /// <summary>
        /// Sends an unreliable message to the target connection.
        /// </summary>
        /// <param name="target">Target connection to send the message to.</param>
        /// <param name="messageID">MessageID of the messasge being sent.</param>
        /// <param name="message">MessageObject to send.</param>

        public static void SendUnreliable<T>(Connection target, ushort messageID, T message) where T : unmanaged, IMessageObject
        {

            SendMessage(target.ID, messageID, 237, PacketFlags.None, message);
        }


        /// <summary>
        /// Sends a reliable message to the target connection.
        /// </summary>
        /// <param name="target">Target connection to send the message to.</param>
        /// <param name="messageID">MessageID of the messasge being sent.</param>
        /// <param name="channel">Channel to send the message on.</param>
        /// <param name="message">MessageObject to send.</param>
        public static void SendReliable<T>(Connection target, ushort messageID, byte channel, T message) where T : unmanaged, IMessageObject
        {
            SendMessage(target.ID, messageID, channel, PacketFlags.Reliable, message);
        }

        /// <summary>
        /// Sends an unreliable message to the target connection.
        /// </summary>
        /// <param name="target">Target connection to send the message to.</param>
        /// <param name="messageID">MessageID of the messasge being sent.</param>
        /// <param name="message">MessageObject to send.</param>
        public static void SendReliable<T>(Connection target, ushort messageID, T message) where T : unmanaged, IMessageObject
        {
            SendReliable(target, messageID, 238, message);
        }

        /// <summary>
        /// Broadcasts a reliable message to every connection.
        /// </summary>
        /// <param name="messageID">MessageID of the messasge being sent.</param>
        /// <param name="channel">Channel to send the message on.</param>
        /// <param name="message">MessageObject to send.</param>
        public static void BroadcastReliable<T>(ushort messageID, byte channel, T message) where T : unmanaged, IMessageObject
        {
            SendMessage(uint.MaxValue, messageID, channel, PacketFlags.Reliable, message);
        }


        /// <summary>
        /// Broadcasts a reliable message to every connection.
        /// </summary>
        /// <param name="messageID">MessageID of the messasge being sent.</param>
        /// <param name="message">MessageObject to send.</param>
        public static void BroadcastReliable<T>(ushort messageID, T message) where T : unmanaged, IMessageObject
        {
            BroadcastReliable(messageID, 239, message);
        }


        /// <summary>
        /// Broadcasts an unreliable message to every connection.
        /// </summary>
        /// <param name="messageID">MessageID of the messasge being sent.</param>
        /// <param name="channel">Channel to send the message on.</param>
        /// <param name="message">MessageObject to send.</param>
        public static void BroadcastUnreliable<T>(ushort messageID, byte channel, T message) where T : unmanaged, IMessageObject
        {
            SendMessage(uint.MaxValue, messageID, channel, PacketFlags.None, message);
        }


        /// <summary>
        /// Broadcasts an unreliable message to every connection.
        /// </summary>
        /// <param name="messageID">MessageID of the messasge being sent.</param>
        /// <param name="message">MessageObject to send.</param>
        public static void BroadcastUnreliable<T>(ushort messageID, T message) where T : unmanaged, IMessageObject
        {
            BroadcastUnreliable(messageID, 240, message);
        }


        /// <summary>
        /// Registers methods to be invoked after a packet has been deserialized into a network message
        /// </summary>
        /// <param name="logicReceiveAction">the method to be invoked on the logic thread.</param>
        /// <param name="gameReceiveAction">the method to be invoked on the game thread.</param>
        public static void RegisterReceiveEvent(OnSocketReceiveDelegate logicReceiveAction = null, Action<Connection, ushort, IntPtr> gameReceiveAction = null)
        {
            if (logicReceiveAction != null)
                workers.logicWorker.onSocketReceive += logicReceiveAction;

            if(gameReceiveAction != null)
                workers.gameWorker.onSocketReceive += gameReceiveAction;
        }

        /// <summary>
        /// Unregisters receive methods.
        /// </summary>
        /// <param name="logicReceiveAction">the logic method to unregister.</param>
        /// <param name="gameReceiveAction">the game method to unregister.</param>
        public static void UnRegisterReceiveEvent(OnSocketReceiveDelegate logicReceiveAction = null, Action<Connection, ushort, IntPtr> gameReceiveAction = null)
        {

            if(logicReceiveAction != null)
                workers.logicWorker.onSocketReceive -= logicReceiveAction;


            if (gameReceiveAction != null)
                workers.gameWorker.onSocketReceive += gameReceiveAction;
        }

        /// <summary>
        /// Registers methods to be invoked  after  an incoming connection has been established.
        /// </summary>
        /// <param name="socketConnectLogicAction">the method invoked on the logic thread after a socket connects.</param>
        /// <param name="socketConnectGameAction">the method invoked on the game thread after a socket  connects.</param>
        public static void RegisterOnSocketConnectEvent(OnSocketConnectDelegate socketConnectLogicAction = null, Action<Connection> socketConnectGameAction = null)
        {
            if(socketConnectLogicAction != null)
                workers.logicWorker.onSocketConnect += socketConnectLogicAction;

            if (socketConnectGameAction != null)
                workers.gameWorker.onSocketConnected += socketConnectGameAction;
        }
        
        /// <summary>
        /// Unregisters methods for socket connect event.
        /// </summary>
        /// <param name="socketConnectLogicAction">the logic method to unregister.</param>
        /// <param name="socketConnectGameAction">the game method to unregister.</param>
        public static void UnRegisterOnSocketConnectEvent(OnSocketConnectDelegate socketConnectLogicAction, Action<Connection> socketConnectGameAction = null)
        {
            if( socketConnectLogicAction != null)
                workers.logicWorker.onSocketConnect -= socketConnectLogicAction;

            if(socketConnectGameAction != null)
                workers.gameWorker.onSocketConnected -= socketConnectGameAction;
        }


        /// <summary>
        /// Registers methods to be invoked after a socket connection has ended gracefully.
        /// </summary>
        /// <param name="socketDisconnectLogicAction">the method to invoke on the logic thread.</param>
        /// <param name="socketDisconnectGameAction">the method to  invoke on the game thread.</param>
        public static void RegisterOnSocketDisconnectEvent(OnSocketDisconnectDelegate socketDisconnectLogicAction = null, Action<Connection> socketDisconnectGameAction = null)
        {
            if (socketDisconnectGameAction != null)
                workers.gameWorker.onSocketDisconnected += socketDisconnectGameAction;

            if (socketDisconnectLogicAction != null)
                workers.logicWorker.onSocketDisconnect += socketDisconnectLogicAction;
        }

        /// <summary>
        /// Unregisters socket disconnect methods
        /// </summary>
        /// <param name="socketDisconnectLogicAction">the logic method to unregister</param>
        /// <param name="socketDisconnectGameAction">the game method to unregister</param>
        public static void UnRegisterOnSocketDisconnectEvent(OnSocketDisconnectDelegate socketDisconnectLogicAction = null, Action<Connection> socketDisconnectGameAction = null)
        {
            if (socketDisconnectGameAction != null)
                workers.gameWorker.onSocketDisconnected -= socketDisconnectGameAction;

            if (socketDisconnectLogicAction != null)
                workers.logicWorker.onSocketDisconnect -= socketDisconnectLogicAction;
        }

        /// <summary>
        /// Registers methods to be invoked after a socket connection has not ended gracefully.
        /// </summary>
        /// <param name="socketTimeoutLogicAction">the method to invoke on the logic thread.</param>
        /// <param name="socketTimeoutGameAction">the method to  invoke on the game thread.</param>
        public static void RegisterOnSocketTimeoutEvent(OnSocketTimeoutDelegate socketTimeoutLogicAction = null, Action<Connection> socketTimeoutGameAction = null)
        {
            if (socketTimeoutLogicAction != null)
                workers.logicWorker.onSocketTimeout += socketTimeoutLogicAction;

            if (socketTimeoutGameAction != null)
                workers.gameWorker.onSocketTimedout += socketTimeoutGameAction;
        }


        /// <summary>
        /// Unregisters socket timeout methods
        /// </summary>
        /// <param name="socketTimeoutLogicAction">the logic method to unregister</param>
        /// <param name="socketTimeoutGameAction">the game method to unregister</param>
        public static void UnregisterOnSocketTimeoutEvent(OnSocketTimeoutDelegate socketTimeoutLogicAction = null, Action<Connection> socketTimeoutGameAction = null)
        {
            if (socketTimeoutLogicAction != null)
                workers.logicWorker.onSocketTimeout -= socketTimeoutLogicAction;

            if (socketTimeoutGameAction != null)
                workers.gameWorker.onSocketTimedout -= socketTimeoutGameAction;
        }

    }


}



