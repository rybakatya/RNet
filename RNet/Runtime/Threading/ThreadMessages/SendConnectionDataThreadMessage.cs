namespace RapidNetworkLibrary.Threading.ThreadMessages
{
    public struct SendConnectionDataThreadMessage
    {
        public uint id;
        public NativeString ip;
        public ushort port;
    }
}



