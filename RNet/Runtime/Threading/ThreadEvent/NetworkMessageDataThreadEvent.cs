using System;
using RapidNet.Connections;


namespace RapidNet.Threading.ThreadEvents
{
    internal struct NetworkMessageDataThreadEvent
    {
        public ushort messageID;
        public IntPtr messageData;
        public Connection sender;
    }
}



