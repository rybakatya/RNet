using System;

namespace RapidNetworkLibrary.Runtime.Zones
{
    [Serializable]
    public struct RNetIPAddress : IEquatable<RNetIPAddress>
    {
        public string ip;
        public ushort port;

        public RNetIPAddress(string ip, ushort port)
        {
            this.ip = ip;
            this.port = port;
        }
        public bool Equals(RNetIPAddress other)
        {
            if (ip.Equals(other.ip) && port == other.port)
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            return ip.GetHashCode() ^ port.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return ip + ":" + port;
        }
    }
}
