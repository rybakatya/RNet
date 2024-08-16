using ENet;


namespace RapidNetworkLibrary.Threading.ThreadEvents
{
    internal struct PackDataThreadEvent
    {
        public uint target;
        public byte channel;
        public Packet payload;
    }
}



