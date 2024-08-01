

using System;
using System.Text;

namespace ENet
{
    public struct Address
    {
        private ENetAddress nativeAddress;

        internal ENetAddress NativeData
        {
            get
            {
                return nativeAddress;
            }

            set
            {
                nativeAddress = value;
            }
        }

        internal Address(ENetAddress address)
        {
            nativeAddress = address;
        }

        public ushort Port
        {
            get
            {
                return nativeAddress.port;
            }

            set
            {
                nativeAddress.port = value;
            }
        }

        public string GetIP()
        {
            StringBuilder ip = new StringBuilder(1025);

            if (Native.enet_address_get_ip(ref nativeAddress, ip, (IntPtr)ip.Capacity) != 0)
                return String.Empty;

            return ip.ToString();
        }

        public bool SetIP(string ip)
        {
            if (ip == null)
                throw new ArgumentNullException("ip");

            return Native.enet_address_set_ip(ref nativeAddress, ip) == 0;
        }

        public string GetHost()
        {
            StringBuilder hostName = new StringBuilder(1025);

            if (Native.enet_address_get_hostname(ref nativeAddress, hostName, (IntPtr)hostName.Capacity) != 0)
                return String.Empty;

            return hostName.ToString();
        }

        public bool SetHost(string hostName)
        {
            if (hostName == null)
                throw new ArgumentNullException("hostName");

            return Native.enet_address_set_hostname(ref nativeAddress, hostName) == 0;
        }
    }
}