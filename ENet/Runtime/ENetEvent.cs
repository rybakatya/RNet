

using System;
using System.Runtime.InteropServices;

namespace ENet
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ENetEvent
    {
        public EventType type;
        public IntPtr peer;
        public byte channelID;
        public uint data;
        public IntPtr packet;
    }
}