using System;
using ENet;
#if ENABLE_MONO || ENABLE_IL2CPP
using Unity.Collections.LowLevel.Unsafe;
#endif

namespace RapidNetworkLibrary.Runtime.Threading.ThreadMessages
{
    public struct SerializeNetworkMessageThreadMessage
    {
        public uint target;
        public ushort id;
        public byte channel;
        public PacketFlags flags;
        public IntPtr messageObjectPointer;
    }


}



