using ENet;
using System;


namespace RapidNetworkLibrary.Connections
{
    public struct Connection : IEquatable<Connection>
    {
        private readonly uint peer;
        


        private ulong bytesSent;
        private ulong bytesReceived;
        private NativeString ipAddress;
        private uint lastReceiveTime;
        private uint lastSendTime;
        private uint lastRoundTripTime;
        private uint mtu;
        private ulong packetsSent;
        private ulong packetsLost;

        private ushort port;

        public static Connection Create(uint id, NativeString ip, ushort port)
        {
            return new Connection(id, ip, port);
        }

        public static Connection Create(Connection connection)
        {
            return new Connection(connection);
        }
        
        public static void Destroy(Connection connection)
        {
            connection.ipAddress.Free();
        }
        private Connection(uint p, NativeString ip, ushort prt)
        {
            peer = p;
            
            bytesSent = 0;
            bytesReceived = 0;
            ipAddress = ip;
            lastReceiveTime = 0;
            lastSendTime = 0;
            lastRoundTripTime = 0;
            mtu = 0;
            packetsSent = 0;
            packetsLost = 0;


            port = prt;

        }


        private Connection(Connection con)
        {
            peer = con.peer;
            


            bytesSent = con.bytesSent;
            bytesReceived = con.bytesReceived;
            ipAddress = con.ipAddress;
            lastReceiveTime = con.lastReceiveTime;
            lastSendTime = con.LastSendTime;
            lastRoundTripTime = con.LastRoundTripTime;
            mtu = con.mtu;
            packetsSent = con.packetsSent;
            packetsLost = con.packetsLost;
            port = con.port;
        }
        public uint ID
        {
            get { return peer; }
        }

        

        public ulong BytesSent { get => bytesSent; internal set => bytesSent = value; }
        public ulong BytesReceived { get => bytesReceived; internal set => bytesReceived = value; }
        public uint LastReceiveTime { get => lastReceiveTime; internal set => lastReceiveTime = value; }
        public uint LastSendTime { get => lastSendTime; internal set => lastSendTime = value; }
        public uint LastRoundTripTime { get => lastRoundTripTime; internal set => lastRoundTripTime = value; }
        public uint Mtu { get => mtu; internal set => mtu = value; }
        public ulong PacketsSent { get => packetsSent; internal set => packetsSent = value; }
        public ulong PacketsLost { get => packetsLost; internal set => packetsLost = value; }

        public ushort Port { get => port; internal set => port =  value; }
        public NativeString IpAddress { get => ipAddress; set => ipAddress = value; }

        public bool Equals(Connection other)
        {
            return peer == other.peer;
        }

        public override int GetHashCode()
        {
            return (int)ID;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return "{ Connection : " + ID;
        }
    }
}



