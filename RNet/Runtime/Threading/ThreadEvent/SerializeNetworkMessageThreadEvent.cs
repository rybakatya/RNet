using System;
using ENet;
#if ENABLE_MONO || ENABLE_IL2CPP
using Unity.Collections.LowLevel.Unsafe;
#endif

namespace RapidNet.Threading.ThreadEvents
{
    internal struct SerializeNetworkMessageThreadEvent
    {
        public uint target;
        public ushort id;
        public ushort length;
        public byte channel;
        public PacketFlags flags;
        public IntPtr messageObjectPointer;
    }


}



