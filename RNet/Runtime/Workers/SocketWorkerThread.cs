using ENet;
using System;
using System.Runtime.InteropServices;
using RapidNet.Threading;
using RapidNet.Threading.ThreadEvents;
using RapidNet.Logging;
using System.Collections.Generic;
using RapidNet.Memory;




namespace RapidNet.Workers
{
    /// <summary>
    /// 
    /// </summary>
    public class SocketWorkerThread : WorkerThread
    {
        private WorkerCollection workers;

        private Host enetHost;
        
        int test;

        internal Action onInit;

        internal Dictionary<uint, Peer> peers = new Dictionary<uint, Peer>();

        internal SocketWorkerThread(Action initCallback, WorkerCollection wrk)
        {
            onInit += initCallback;
            workers = wrk;
            
           
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Init()
        {
            shouldRun = true;
            
            InitializeENet();


            if (onInit != null)
            {
                onInit();
            }
        }

        private void InitializeENet()
        {
            Callbacks callbacks = new Callbacks(OnMemoryAllocate, OnMemoryFree, OnNoMemory);

            if (Library.Initialize(callbacks))
                Logger.Log(LogLevel.Info, "ENet successfully initialized using a custom memory allocator");

            else
                Logger.Log(LogLevel.Info, "Failed to initialize enet!");
        }

        private void DeInitialize()
        {
            Library.Deinitialize();
        }

#if SERVER
        private void InitializeENetServer(string ip, ushort port, int maxConnections, byte channelLimit)
        {
            enetHost = new Host();

            Address address = new Address();
            address.SetHost(ip);
            address.Port = port;

            
            enetHost.Create(address, maxConnections, channelLimit);
            Logger.Log(LogLevel.Info, "Initialized server at " + ip + ":" + port);
        }
#elif CLIENT
        private void InitializeENetClient(byte channelLimit)
        {
            enetHost = new Host();
            enetHost.Create(12, channelLimit);
           
        }
#endif
        private Event ENetPollEvents()
        {

            Event netEvent = new Event(new ENetEvent() { type = EventType.None });


            bool polled = false;

            while (!polled)
            {
                if (enetHost == null)
                    break;
                if (enetHost.CheckEvents(out netEvent) <= 0)
                {
                    if (enetHost.Service(0, out netEvent) <= 0)
                        break;

                    polled = true;
                }

            }
            return netEvent;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Tick()
        {

            var e = ENetPollEvents();
            
            switch (e.Type)
            {

                case EventType.Connect:
                    peers.Add(e.Peer.ID, e.Peer);
                    workers.logicWorker.Enqueue((ushort)WorkerThreadEventID.SendConnection, new SendConnectionDataThreadEvent()
                    {
                        id = (ushort)e.Peer.ID,
                        ip = new NativeString(e.Peer.IP),
                        port = e.Peer.Port
                    });
                    break;

                case EventType.Disconnect:
                    peers.Remove(e.Peer.ID);
                    workers.logicWorker.Enqueue((ushort)WorkerThreadEventID.SendDisconnection, e.Peer.ID);
                    break;

                case EventType.Timeout:
                    peers.Remove(e.Peer.ID);
                    workers.logicWorker.Enqueue((ushort)WorkerThreadEventID.SendTimeout, e.Peer.ID);
                    break;

                case EventType.Receive:
                    Logger.Log(LogLevel.Info, "Packet received on network thread, sending to logic thread for deserialization.");
                    workers.logicWorker.Enqueue((ushort)WorkerThreadEventID.SendDeserializeNetworkMessage, new DeserializeNetworkMessageThreadEvent()
                    {
                        packet = e.Packet,
                        sender = (ushort)e.Peer.ID,
                        ip = new NativeString(e.Peer.IP.ToString()),
                        port = e.Peer.Port,
                        bytesReceived = e.Peer.BytesReceived,
                        bytesSent = e.Peer.BytesSent,
                        lastReceiveTime = e.Peer.LastReceiveTime,
                        lastRoundTripTime = e.Peer.LastRoundTripTime,
                        lastSendTime = e.Peer.LastSendTime,
                        mtu = e.Peer.MTU,
                        packetsLost = e.Peer.PacketsLost,
                        packetsSent = e.Peer.PacketsSent,
                    });
                   
                    break;
            }

            if (me.IsSet == true)
            {
                RNet.BytesReceived = me.BytesReceived;
                RNet.BytesSent = me.BytesSent;
                RNet.LastReceiveTime = me.LastReceiveTime;
                RNet.LastRoundTripTime = me.LastRoundTripTime;
                RNet.LastSendTime = me.LastSendTime;
                RNet.Mtu = me.MTU;
                RNet.PacketsLost = me.PacketsLost;
                RNet.PacketsSent = me.PacketsSent;
            }
        }

        

        public override void OnConsume(ushort eventID, IntPtr data)
        {

            switch (eventID)
            {
             
#if SERVER
                case (ushort)WorkerThreadEventID.SendInitializeServer:
                    InitServer(data);
                    break;

                case (ushort)WorkerThreadEventID.SendDisconnection:
                    var peerID = MemoryHelper.Read<uint> (data);
                    if (peers.ContainsKey(peerID) == true)
                    {
                        peers[peerID].DisconnectNow(0);
                    }
                    break;
#endif
                case (ushort)WorkerThreadEventID.SendConnectToSocket:
                    ConnectToSocket(data);
                    break;

                case (ushort)WorkerThreadEventID.SendDisconnectionFromPeers:
                    me.DisconnectNow(0);
                    break;

#if CLIENT
                case (ushort)WorkerThreadEventID.SendInitClient:
                    InitializeENetClient(MemoryHelper.Read<byte>(data));
                    break;
#endif
                case (ushort)WorkerThreadEventID.SendSerializeMessageEvent:
                    var packetData = MemoryHelper.Read<PackDataThreadEvent>(data);
                    if(peers.ContainsKey(packetData.target) == true)
                    {
                        peers[packetData.target].Send(packetData.channel, ref packetData.payload);
                    }
                    else
                    {
                        if(packetData.target == uint.MaxValue)
                        {
                            enetHost.Broadcast(packetData.channel, ref packetData.payload);
                        }
                        else
                        {
                            packetData.payload.Dispose();
                        }
                    }
                    
                    break;

                
            }
        }


        Peer me;
        private void ConnectToSocket(IntPtr data)
        {
            var dataValue = MemoryHelper.Read<ConnectToSocketThreadEvent>(data);
            Logger.Log(LogLevel.Info, "Attempting to connect to " + dataValue.ip.ToString() + ":" + dataValue.port);
            var address = new Address();
            address.SetIP(dataValue.ip.ToString());
            address.Port = dataValue.port;

            me = enetHost.Connect(address, 255);
            dataValue.ip.Free();

        }

#if SERVER
        private void InitServer(IntPtr data)
        {
            var initData = MemoryHelper.Read<InitializeServerThreadEvent>(data);

            InitializeENetServer(initData.ip.ToString(), initData.port, initData.maxConnections, initData.maxChannels);
            initData.ip.Free();
        }


#endif
        /// <summary>
        /// 
        /// </summary>
        protected override void Destroy()
        {
            enetHost.Flush();
            enetHost.Dispose();
            ENet.Library.Deinitialize();
        }
        AllocCallback OnMemoryAllocate = (size) =>
        {
            return Marshal.AllocHGlobal(size);
        };

        FreeCallback OnMemoryFree = (memory) =>
        {
            Marshal.FreeHGlobal(memory);
        };

        NoMemoryCallback OnNoMemory = () =>
        {
            throw new OutOfMemoryException();
        };
    }
}



