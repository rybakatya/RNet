using ENet;


namespace RapidNetworkLibrary.Threading.ThreadMessages
{
    internal struct DeserializeNetworkMessageThreadMessage
    {
        public uint sender;
        public NativeString ip;
        public ushort port;
        public Packet packet;


        public ulong bytesSent;
        public ulong bytesReceived;
        public uint lastReceiveTime;
        public uint lastSendTime;
        public uint lastRoundTripTime;
        public uint mtu;
        public ulong packetsSent;
        public ulong packetsLost;

    }
}



