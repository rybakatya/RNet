using ENet;
using System;
using RapidNetworkLibrary.Threading;
using RapidNetworkLibrary.Connections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using RapidNetworkLibrary.Logging;
using System.Reflection;
using RapidNetworkLibrary.Serialization;
using RapidNetworkLibrary.Threading.ThreadMessages;


using RapidNetworkLibrary.Runtime.Threading.ThreadMessages;
using RapidNetworkLibrary.Runtime.Zones;
using RapidNetworkLibrary.Runtime.Memory;


namespace RapidNetworkLibrary.Workers
{
    public delegate ConnectionType GetConnectionTypeDelegate(RNetIPAddress address);
    internal class LogicWorkerThread : WorkerThread
    {

        public Dictionary<ushort, Serializer> serializers = new Dictionary<ushort, Serializer>();

        /// <summary>
        /// Called from the logic thread before the update loop begins.
        /// </summary>
        private Action onLogicInit;

#if SERVER
        internal Action<Connection> onClientConnected;
        internal Action<Connection> onServerConnected;
        internal GetConnectionTypeDelegate getConnectionType;
#elif CLIENT
        internal Action<Connection> onConnectedToServer;
#endif




        private WorkerCollection workers;

        private PacketFreeCallback packetFree;
        private SmmallocInstance smmalloc;
        public LogicWorkerThread(Action logicInitAction, WorkerCollection wrk, SmmallocInstance malloc)
        {
            smmalloc = malloc;
           
            workers = wrk;

            onLogicInit += logicInitAction;           
        }

        private void GetAllSerializers()
        {
            Logger.Log(LogLevel.Info, "Setting up serializers");
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach(var assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.GetCustomAttributes(typeof(SerializerAttribute), false).Length > 0)
                    {
                        if (type.IsSubclassOf(typeof(Serializer)))
                        {
                            Logger.Log(LogLevel.Info, "Serializer " + type.FullName + " initialized!");

                            var t = type.GetCustomAttribute<SerializerAttribute>();
                            if (serializers.ContainsKey(t.messageID) == false)
                                serializers[t.messageID] = Activator.CreateInstance(type) as Serializer;

                            else
                            {
                                Logger.Log(LogLevel.Error, "Cannot create serializer " + type.FullName + " id is already in use!");
                            }
                        }
                    }
                }
            }    
        }

        public ConnectionHandler connectionHandler;
        internal Action<Connection, ushort, IntPtr> onSocketReceive;

        protected override void Init()
        {
            shouldRun = true;
            smmalloc.CreateThreadCache(4 * 1024, CacheWarmupOptions.Hot);
            packetFree += onPacketFree;
            while(workers == null)
            {
                Console.WriteLine("Waiting for main thread");
            }
            connectionHandler = new ConnectionHandler(workers);
            GetAllSerializers();
            if (onLogicInit != null)
                onLogicInit();
        }

       
        

        protected override void Tick()
        {

        }


        internal override void OnConsume(WorkerThreadMessageID messageID, IntPtr data)
        {

            switch (messageID)
            {
                case WorkerThreadMessageID.SendConnection:
#if SERVER
                    var msgData = MemoryHelper.Read<SendConnectionDataThreadMessage>(data);
                    connectionHandler.HandleSocketConnection(msgData.id,msgData.ip, msgData.port);
#endif
                    break;

                case WorkerThreadMessageID.SendDisconnection:
                    connectionHandler.HandleDisconnect(MemoryHelper.Read<uint>(data));
                    break;

                case WorkerThreadMessageID.SendTimeout:
                    connectionHandler.HandleTimeout(MemoryHelper.Read<uint>(data));
                    break;

                case WorkerThreadMessageID.SendSerializeNetworkMessage:
                    SerializeOutgoingMessage(MemoryHelper.Read<SerializeNetworkMessageThreadMessage>(data));
                    break;

                case WorkerThreadMessageID.SendDeserializeNetworkMessage:
                    DeserializeIncomingMessage(MemoryHelper.Read<DeserializeNetworkMessageThreadMessage>(data));
                    break;

                case WorkerThreadMessageID.SendPeerData:
                    TransferPeerData(MemoryHelper.Read<PeerDataThreadMessage>(data));
                    break;

            }
        }



        private void TransferPeerData(PeerDataThreadMessage data)
        {
            if (connectionHandler.IsConnectionValid(data.id) == false)
                return;
            var con = Connection.Create(connectionHandler.GetConnection(data.id));
            con.BytesReceived = data.bytesReceived;
            con.BytesSent = data.bytesSent;         
            con.LastReceiveTime = data.lastReceiveTime;
            con.LastSendTime = data.lastSendTime;
            con.LastRoundTripTime = data.lastRoundTripTime;
            con.Mtu = data.mtu;
            con.PacketsLost = data.packetsLost;
            con.PacketsSent = data.packetsSent;
            con.Port = data.port;
            connectionHandler.SetConnection(data.id, con);
        }

        private unsafe void DeserializeIncomingMessage(DeserializeNetworkMessageThreadMessage data)
        {

            var buffer = BufferPool.GetBitBuffer();

            var span = new ReadOnlySpan<byte>(data.packet.Data.ToPointer(), data.packet.Length);

            buffer.FromSpan(ref span, data.packet.Length);
            var msgID = buffer.ReadUShort();
            Logger.Log(LogLevel.Info, "Packet received on logic thread, deserializing...");

            if (msgID == NetworkMessages.informConnectionType)
            {
                var messageData = serializers[msgID].Deserialize(buffer);
                var msg = MemoryHelper.Read<InformConnectionType>(messageData);
#if CLIENT
                connectionHandler.HandleSocketConnection(data.sender, msg.type, data.ip, data.port);
#elif SERVER
                var con = connectionHandler.GetConnection(data.sender);
                con.ConnectionType = msg.type;
#endif
                MemoryHelper.Free(messageData);

            }
            else
            {

                var msgData = new NetworkMessageDataThreadMessage()
                {
                    messageID = msgID,
                    messageData = serializers[msgID].Deserialize(buffer),
                    sender = connectionHandler.GetConnection(data.sender)
                };

                workers.gameWorker.Enqueue(WorkerThreadMessageID.SendNetworkMessageToGameThread, msgData);
            }

            data.packet.Dispose();
            //data.ip.Free();
            buffer.Clear();

        }

        private unsafe void SerializeOutgoingMessage(SerializeNetworkMessageThreadMessage data)
        {
            Logger.Log(LogLevel.Info, "Attempting to serialize outgoing message");

            var buffer = BufferPool.GetBitBuffer();
            buffer.AddUShort(data.id);
            serializers[data.id].Serialize(buffer, data.messageObjectPointer);

            Logger.Log(LogLevel.Info, "Attempting to allocate logic packet");
            var ptr = Marshal.AllocHGlobal(buffer.Length * Marshal.SizeOf<byte>());
            //var ptr = (IntPtr)UnsafeUtility.MallocTracked(buffer.Length * sizeof(byte), 0, Unity.Collections.Allocator.Persistent, 0);
            var span = new Span<byte>(ptr.ToPointer(), buffer.Length);
            buffer.ToSpan(ref span);
            Packet packet = default(Packet);
            packet.Create(ptr, buffer.Length, data.flags);
            packet.SetFreeCallback(packetFree);
            workers.socketWorker.Enqueue(WorkerThreadMessageID.SendSerializeNetworkMessage, new PacketDataThreadMessage()
            {
                target = data.target,
                channel = data.channel,
                payload = packet
            });

            MemoryHelper.Free(data.messageObjectPointer);
            buffer.Clear();
        }
        private unsafe void onPacketFree(Packet packet)
        {
            Logger.Log(LogLevel.Warning, "Freeing packet!");
            Marshal.FreeHGlobal(packet.Data);
            //UnsafeUtility.FreeTracked(packet.Data.ToPointer(), Unity.Collections.Allocator.Persistent);
        }

        protected override void Destroy()
        {
            
            connectionHandler.Destroy();
            smmalloc.DestroyThreadCache();

        }
    }
}



