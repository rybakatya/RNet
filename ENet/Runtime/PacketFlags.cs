

using System;

namespace ENet
{
    [Flags]
    public enum PacketFlags
    {
        None = 0,
        Reliable = 1 << 0,
        Unsequenced = 1 << 1,
        NoAllocate = 1 << 2,
        UnreliableFragmented = 1 << 3,
        Instant = 1 << 4,
        Unthrottled = 1 << 5,
        Sent = 1 << 8
    }
}