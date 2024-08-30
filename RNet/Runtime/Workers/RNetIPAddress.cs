using System;


namespace RapidNet
{
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
            if (other.ip.Equals(ip) && other.port == port)
                return true;
            return false;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ip, port);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}


