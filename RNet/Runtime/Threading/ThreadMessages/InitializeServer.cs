namespace RapidNetworkLibrary.Threading.ThreadMessages
{
    internal struct InitializeServer
    {
        public NativeString ip;
        public ushort port;
        public byte maxChannels;
        public ushort maxConnections;
    }
}



