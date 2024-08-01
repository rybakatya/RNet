

using System;
using System.Runtime.InteropServices;

namespace ENet
{
    public struct Packet : IDisposable
    {
        private IntPtr nativePacket;

        internal IntPtr NativeData
        {
            get
            {
                return nativePacket;
            }

            set
            {
                nativePacket = value;
            }
        }

        internal Packet(IntPtr packet)
        {
            nativePacket = packet;
        }

        public void Dispose()
        {
            if (nativePacket != IntPtr.Zero)
            {
                Native.enet_packet_dispose(nativePacket);
                nativePacket = IntPtr.Zero;
            }
        }

        public bool IsSet
        {
            get
            {
                return nativePacket != IntPtr.Zero;
            }
        }

        public IntPtr Data
        {
            get
            {
                ThrowIfNotCreated();

                return Native.enet_packet_get_data(nativePacket);
            }
        }

        public IntPtr UserData
        {
            get
            {
                ThrowIfNotCreated();

                return Native.enet_packet_get_user_data(nativePacket);
            }

            set
            {
                ThrowIfNotCreated();

                Native.enet_packet_set_user_data(nativePacket, value);
            }
        }

        public int Length
        {
            get
            {
                ThrowIfNotCreated();

                return Native.enet_packet_get_length(nativePacket);
            }
        }

        public bool HasReferences
        {
            get
            {
                ThrowIfNotCreated();

                return Native.enet_packet_check_references(nativePacket) != 0;
            }
        }

        internal void ThrowIfNotCreated()
        {
            if (nativePacket == IntPtr.Zero)
                throw new InvalidOperationException("Packet not created");
        }

        public void SetFreeCallback(IntPtr callback)
        {
            ThrowIfNotCreated();

            Native.enet_packet_set_free_callback(nativePacket, callback);
        }

        public void SetFreeCallback(PacketFreeCallback callback)
        {
            ThrowIfNotCreated();

            Native.enet_packet_set_free_callback(nativePacket, Marshal.GetFunctionPointerForDelegate(callback));
        }

        public void Create(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            Create(data, data.Length);
        }

        public void Create(byte[] data, int length)
        {
            Create(data, length, PacketFlags.None);
        }

        public void Create(byte[] data, PacketFlags flags)
        {
            Create(data, data.Length, flags);
        }

        public void Create(byte[] data, int length, PacketFlags flags)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            if (length < 0 || length > data.Length)
                throw new ArgumentOutOfRangeException("length");

            nativePacket = Native.enet_packet_create(data, (IntPtr)length, flags);
        }

        public void Create(IntPtr data, int length, PacketFlags flags)
        {
            if (data == IntPtr.Zero)
                throw new ArgumentNullException("data");

            if (length < 0)
                throw new ArgumentOutOfRangeException("length");

            nativePacket = Native.enet_packet_create(data, (IntPtr)length, flags);
        }

        public void Create(byte[] data, int offset, int length, PacketFlags flags)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset");

            if (length < 0 || length > data.Length)
                throw new ArgumentOutOfRangeException("length");

            nativePacket = Native.enet_packet_create_offset(data, (IntPtr)length, (IntPtr)offset, flags);
        }

        public void Create(IntPtr data, int offset, int length, PacketFlags flags)
        {
            if (data == IntPtr.Zero)
                throw new ArgumentNullException("data");

            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset");

            if (length < 0)
                throw new ArgumentOutOfRangeException("length");

            nativePacket = Native.enet_packet_create_offset(data, (IntPtr)length, (IntPtr)offset, flags);
        }

        public void CopyTo(byte[] destination)
        {
            if (destination == null)
                throw new ArgumentNullException("destination");

            Marshal.Copy(Data, destination, 0, Length);
        }
    }
}