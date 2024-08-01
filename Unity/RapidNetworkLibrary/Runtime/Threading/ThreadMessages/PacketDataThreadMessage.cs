using ENet;


namespace RapidNetworkLibrary.Threading.ThreadMessages
{
    public struct PacketDataThreadMessage
    {
        public uint target;
        public byte channel;
        public Packet payload;
    }
}



