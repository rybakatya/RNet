namespace RapidNetworkLibrary.Threading.ThreadMessages
{
    public struct PeerDataThreadMessage
    {
        public ulong bytesSent;
        public ulong bytesReceived;

        public uint lastReceiveTime;
        public uint lastSendTime;
        public uint lastRoundTripTime;
        public uint mtu;
        public ulong packetsSent;
        public ulong packetsLost;
        public float packetsThrottle;
        public ushort port;
        internal uint id;
    }
}



