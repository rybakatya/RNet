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
using RapidNetworkLibrary.Memory;
using System.Diagnostics;
using System.Numerics;
using RapidNetworkLibrary.Extensions;




public struct RNetIPAddress : IEquatable<RNetIPAddress>
{
    public string ip;
    public ushort port;

    public RNetIPAddress(string ip, ushort port)
    {
        this.ip = ip;
        this.port = port;
    }
    public bool Equals(RNetIPAddress other)
    {
        if(other.ip.Equals(ip) && other.port == port)
            return true;
        return false;
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ip, port);
    }

    public override string ToString()
    {
        return base.ToString();
    }
}

namespace RapidNetworkLibrary.Workers
{
   
    public class LogicWorkerThread : WorkerThread
    {

        internal Dictionary<ushort, Serializer> serializers = new Dictionary<ushort, Serializer>();

        /// <summary>
        /// Called from the logic thread before the update loop begins.
        /// </summary>
        private Action onLogicInit;

        internal Action<Connection, ushort, IntPtr> onSocketReceive;
#if SERVER
        internal Action<Connection> onSocketConnect;
        
#elif CLIENT
        internal Action<Connection> onConnectedToServer;
#endif




        private WorkerCollection workers;

        private PacketFreeCallback packetFree;

        private Stopwatch stopWatch;

        private readonly ExtensionManager _extensionManager;
        internal LogicWorkerThread(Action logicInitAction, WorkerCollection wrk, ExtensionManager extensionManager)
        {

           
            workers = wrk;

            onLogicInit += logicInitAction;     
            _extensionManager = extensionManager;
            
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

        internal ConnectionHandler connectionHandler;
        

        protected override void Init()
        {
            shouldRun = true;

            packetFree += onPacketFree;
            while(workers == null)
            {
                Console.WriteLine("Waiting for main thread");
            }
            connectionHandler = new ConnectionHandler(workers, _extensionManager);
            GetAllSerializers();
            stopWatch = new Stopwatch();
            stopWatch.Start();
            if (onLogicInit != null)
                onLogicInit();
        }

       
        

        protected override void Tick()
        {

        }


        internal override void OnConsume(ushort messageID, IntPtr data)
        {

            switch (messageID)
            {
                case (ushort)WorkerThreadMessageID.SendConnection:
                    var msgData = MemoryHelper.Read<SendConnectionDataThreadMessage>(data);
                    connectionHandler.HandleSocketConnection(msgData.id,msgData.ip, msgData.port);

                    break;

                case (ushort)WorkerThreadMessageID.SendDisconnection:
                    connectionHandler.HandleDisconnect(MemoryHelper.Read<uint>(data));
                    break;

                case (ushort)WorkerThreadMessageID.SendTimeout:
                    connectionHandler.HandleTimeout(MemoryHelper.Read<uint>(data));
                    break;

                case (ushort)WorkerThreadMessageID.SendSerializeMessage:
                    

                    var msg = MemoryHelper.Read<SerializeNetworkMessageThreadMessage>(data);
                    //SendMessage(msg.target, msg.id, msg.channel, msg.flags, msg.messageObjectPointer);
                    SerializeOutgoingMessage(msg);
                    break;

                case (ushort)WorkerThreadMessageID.SendDeserializeNetworkMessage:
                    DeserializeIncomingMessage(MemoryHelper.Read<DeserializeNetworkMessageThreadMessage>(data));
                    break;

                default:
                    _extensionManager.OnThreadMessageReceived(ThreadType.Logic, messageID, data);
                    break;



            }
        }



       

        private unsafe void DeserializeIncomingMessage(DeserializeNetworkMessageThreadMessage data)
        {

            var buffer = BufferPool.GetBitBuffer();

            var span = new ReadOnlySpan<byte>(data.packet.Data.ToPointer(), data.packet.Length);

            buffer.FromSpan(ref span, data.packet.Length);
            var msgID = buffer.ReadUShort();
            Logger.Log(LogLevel.Info, "Packet received on logic thread, deserializing...");

            if (msgID == ushort.MaxValue)
            {


            }
            else
            {
                var ptr = serializers[msgID].Deserialize(buffer);
                var msgData = new NetworkMessageDataThreadMessage()
                {
                    messageID = msgID,
                    messageData = ptr,
                    sender = connectionHandler.GetConnection(data.sender)
                };

                _extensionManager.OnSocketReceive(ThreadType.Logic, connectionHandler.GetConnection(data.sender), msgID, ptr);
                if(onSocketReceive != null)
                    onSocketReceive(connectionHandler.GetConnection(data.sender), msgID, ptr);

                workers.gameWorker.Enqueue((ushort)WorkerThreadMessageID.SendNetworkMessageToGameThread, msgData);
            }

            data.packet.Dispose();
            data.ip.Free();
            buffer.Clear();

        }

        

        
        private unsafe void SerializeOutgoingMessage(SerializeNetworkMessageThreadMessage data)
        {


            var buffer = BufferPool.GetBitBuffer();
            buffer.AddUShort(data.id);
            serializers[data.id].Serialize(buffer, data.messageObjectPointer);
           

            var ptr = Marshal.AllocHGlobal(buffer.Length * Marshal.SizeOf<byte>());
            
            var span = new Span<byte>(ptr.ToPointer(), buffer.Length);
            buffer.ToSpan(ref span);
            Packet packet = default(Packet);
            packet.Create(ptr, buffer.Length, data.flags);
            packet.SetFreeCallback(packetFree);
            workers.socketWorker.Enqueue((ushort)WorkerThreadMessageID.SendSerializeMessage, new PacketDataThreadMessage()
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

            Marshal.FreeHGlobal(packet.Data);
            
        }

        protected override void Destroy()
        {
            workers = null;
            connectionHandler.Destroy();
        }
    }
}



