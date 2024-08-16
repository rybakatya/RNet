using ENet;


namespace RapidNetworkLibrary.Threading.ThreadMessages
{
    internal struct PacketDataThreadMessage
    {
        public uint target;
        public byte channel;
        public Packet payload;
    }
}



