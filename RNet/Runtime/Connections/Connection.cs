using ENet;
using System;


namespace RapidNet.Connections
{
    /// <summary>
    /// Contains a managed pointer to the connection instance and a cached ID.
    /// </summary>
    public struct Connection : IEquatable<Connection>
    {
        private ushort peer;
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

        internal static Connection Create(ushort id, NativeString ip, ushort port, ulong sentBytes, ulong receivedBytes, uint receivedLastTime, uint sendLastTime, uint lastRTT, uint _mtu, ulong sentPackets, ulong lostPackets)
        {
            return new Connection()
            {
                peer = id,
                bytesSent = sentBytes,
                bytesReceived = receivedBytes,
                lastReceiveTime = receivedLastTime,
                lastSendTime = sendLastTime,
                lastRoundTripTime = lastRTT,
                mtu = _mtu,
                ipAddress = ip,
                port = port,
                packetsSent = sentPackets,
                packetsLost = lostPackets
            };
        }
        internal static Connection Create(ushort id, NativeString ip, ushort port)
        {
            return new Connection(id, ip, port);
        }

        internal static Connection Create(Connection connection)
        {
            return new Connection(connection);
        }

        internal static void Destroy(Connection connection)
        {
            connection.ipAddress.Free();
        }
        internal Connection(ushort p, NativeString ip, ushort prt)
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

        /// <summary>
        /// Returns a numeric id representing this connection.
        /// </summary>
        public ushort ID
        {
            get { return peer; }
        }

        
        /// <summary>
        /// returns the total number of bytes sent from this connection.
        /// </summary>
        public ulong BytesSent { get => bytesSent; internal set => bytesSent = value; }

        /// <summary>
        /// returns the total number of bytes received on this connection.
        /// </summary>
        public ulong BytesReceived { get => bytesReceived; internal set => bytesReceived = value; }

        /// <summary>
        /// returns the last time a packet was received on this connection.
        /// </summary>
        public uint LastReceiveTime { get => lastReceiveTime; internal set => lastReceiveTime = value; }

        /// <summary>
        /// returns the last time a packet  was sent on this connection.
        /// </summary>
        public uint LastSendTime { get => lastSendTime; internal set => lastSendTime = value; }

        /// <summary>
        /// returns the last known round trip time for this connection.
        /// </summary>
        public uint LastRoundTripTime { get => lastRoundTripTime; internal set => lastRoundTripTime = value; }

        /// <summary>
        /// returns the connections MTU;
        /// </summary>
        public uint Mtu { get => mtu; internal set => mtu = value; }


        /// <summary>
        /// returns the total number of packets sent by this connection.
        /// </summary>
        public ulong PacketsSent { get => packetsSent; internal set => packetsSent = value; }

        /// <summary>
        /// returns the total number of packets received by this connection.
        /// </summary>
        public ulong PacketsLost { get => packetsLost; internal set => packetsLost = value; }

        /// <summary>
        /// returns the port this connection is using.
        /// </summary>
        public ushort Port { get => port; internal set => port =  value; }

        /// <summary>
        /// returns this connections IPAddress as a NativeString
        /// <seealso cref="NativeString"/>
        /// </summary>
        public NativeString IpAddress { get => ipAddress; set => ipAddress = value; }


        /// <summary>
        /// returns true if this connection equals other connection.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Connection other)
        {
            return peer == other.peer;
        }


        /// <summary>
        /// Used by Dictionary to determine if this connection is a key.>
        /// </summary>
        /// <returns cref="Connection.ID">id of the connection</returns>
        public override int GetHashCode()
        {
            return (int)ID;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "{ Connection : " + ID + "}";
        }
    }
}



