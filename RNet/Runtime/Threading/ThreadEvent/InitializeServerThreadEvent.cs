namespace RapidNet.Threading.ThreadEvents
{
    internal struct InitializeServerThreadEvent
    {
        public NativeString ip;
        public ushort port;
        public byte maxChannels;
        public ushort maxConnections;
    }
}



