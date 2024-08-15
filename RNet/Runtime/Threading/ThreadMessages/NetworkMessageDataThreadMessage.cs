using System;
using RapidNetworkLibrary.Connections;


namespace RapidNetworkLibrary.Threading.ThreadMessages
{
    public struct NetworkMessageDataThreadMessage
    {
        public ushort messageID;
        public IntPtr messageData;
        public Connection sender;
    }
}



