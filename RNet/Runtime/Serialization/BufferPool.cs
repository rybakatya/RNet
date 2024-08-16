﻿using RapidNetworkLibrary.Serialization;
using System;

internal static class BufferPool
{
    [ThreadStatic]
    private static BitBuffer bitBuffer;

    public static BitBuffer GetBitBuffer()
    {
        if (bitBuffer == null)
            bitBuffer = new BitBuffer(1024);

        return bitBuffer;
    }
}