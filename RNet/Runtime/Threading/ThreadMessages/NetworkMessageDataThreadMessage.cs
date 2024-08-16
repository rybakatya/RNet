using System;
using RapidNetworkLibrary.Connections;


namespace RapidNetworkLibrary.Threading.ThreadMessages
{
    internal struct NetworkMessageDataThreadMessage
    {
        public ushort messageID;
        public IntPtr messageData;
        public Connection sender;
    }
}



