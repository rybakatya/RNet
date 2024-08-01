using ENet;


namespace RapidNetworkLibrary.Threading.ThreadMessages
{
    public struct DeserializeNetworkMessageThreadMessage
    {
        public uint sender;
        public NativeString ip;
        public ushort port;
        public Packet packet;
    }
}



