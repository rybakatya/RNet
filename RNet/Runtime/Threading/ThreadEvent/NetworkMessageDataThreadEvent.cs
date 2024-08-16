using System;
using RapidNetworkLibrary.Connections;


namespace RapidNetworkLibrary.Threading.ThreadEvents
{
    internal struct NetworkMessageDataThreadEvent
    {
        public ushort messageID;
        public IntPtr messageData;
        public Connection sender;
    }
}



