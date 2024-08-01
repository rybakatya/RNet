using ENet;
using System;


namespace RapidNetworkLibrary.Connections
{
    public struct Connection : IEquatable<Connection>
    {
        private readonly uint peer;
        private ConnectionType connectionType;


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

        public static Connection Create(uint id, ConnectionType type, string ip, ushort port)
        {
            return new Connection(id, type, ip, port);
        }

        public static Connection Create(Connection connection)
        {
            return new Connection(connection);
        }

        public static void Destroy(Connection connection)
        {
            connection.ipAddress.Free();
        }
        private Connection(uint p, ConnectionType type, string ip, ushort prt)
        {
            peer = p;
            connectionType = type;
            bytesSent = 0;
            bytesReceived = 0;
            ipAddress = new NativeString(ip);
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
            connectionType = con.connectionType;


            bytesSent = con.bytesSent;
            bytesReceived = con.bytesReceived;
            ipAddress = new NativeString(con.ipAddress.ToString());
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

        public ConnectionType ConnectionType
        {
            get
            {
                return connectionType;
            }
            internal set
            {
                connectionType = value;
            }
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

        public bool Equals(Connection other)
        {
            return peer == other.peer;
        }


    }
}



