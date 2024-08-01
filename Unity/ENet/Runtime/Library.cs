

using System;

namespace ENet
{
    public static class Library
    {
        public const uint maxChannelCount = 0xFF;
        public const uint maxPeers = 0xFFF;
        public const uint maxPacketSize = 32 * 1024 * 1024;
        public const uint throttleThreshold = 40;
        public const uint throttleScale = 32;
        public const uint throttleAcceleration = 2;
        public const uint throttleDeceleration = 2;
        public const uint throttleInterval = 5000;
        public const uint timeoutLimit = 32;
        public const uint timeoutMinimum = 5000;
        public const uint timeoutMaximum = 30000;
        public const uint version = (2 << 16) | (5 << 8) | (2);

        public static uint Time
        {
            get
            {
                return Native.enet_time_get();
            }
        }

        public static bool Initialize()
        {

            return Native.enet_initialize() == 0;
        }


        public static bool Initialize(Callbacks callbacks)
        {
            if (callbacks == null)
                throw new ArgumentNullException("callbacks");



            ENetCallbacks nativeCallbacks = callbacks.NativeData;

            return Native.enet_initialize_with_callbacks(version, ref nativeCallbacks) == 0;
        }

        public static void Deinitialize()
        {
            Native.enet_deinitialize();
        }

        public static ulong CRC64(IntPtr buffers, int bufferCount)
        {
            return Native.enet_crc64(buffers, bufferCount);
        }
    }
}