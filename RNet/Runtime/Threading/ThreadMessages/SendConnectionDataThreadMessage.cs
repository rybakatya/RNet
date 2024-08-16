namespace RapidNetworkLibrary.Threading.ThreadMessages
{
    internal struct SendConnectionDataThreadMessage
    {
        public uint id;
        public NativeString ip;
        public ushort port;
    }
}



