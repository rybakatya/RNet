

using System;
using System.Text;

namespace ENet
{
    public struct Peer : IEquatable<Peer>
    {
        private IntPtr nativePeer;
        private uint nativeID;

        internal IntPtr NativeData
        {
            get
            {
                return nativePeer;
            }

            set
            {
                nativePeer = value;
            }
        }

        internal Peer(IntPtr peer)
        {
            nativePeer = peer;
            nativeID = nativePeer != IntPtr.Zero ? Native.enet_peer_get_id(nativePeer) : 0;
        }

        public bool IsSet
        {
            get
            {
                return nativePeer != IntPtr.Zero;
            }
        }

        public uint ID
        {
            get
            {
                return nativeID;
            }
        }

        public string IP
        {
            get
            {
                ThrowIfNotCreated();

                byte[] ip = ArrayPool.GetByteBuffer();

                if (Native.enet_peer_get_ip(nativePeer, ip, (IntPtr)ip.Length) == 0)
                    return Encoding.ASCII.GetString(ip, 0, ip.StringLength());
                else
                    return String.Empty;
            }
        }

        public ushort Port
        {
            get
            {
                ThrowIfNotCreated();

                return Native.enet_peer_get_port(nativePeer);
            }
        }

        public uint MTU
        {
            get
            {
                ThrowIfNotCreated();

                return Native.enet_peer_get_mtu(nativePeer);
            }
        }

        public PeerState State
        {
            get
            {
                return nativePeer == IntPtr.Zero ? PeerState.Uninitialized : Native.enet_peer_get_state(nativePeer);
            }
        }

        public uint RoundTripTime
        {
            get
            {
                ThrowIfNotCreated();

                return Native.enet_peer_get_rtt(nativePeer);
            }
        }

        public uint LastRoundTripTime
        {
            get
            {
                ThrowIfNotCreated();

                return Native.enet_peer_get_last_rtt(nativePeer);
            }
        }

        public uint LastSendTime
        {
            get
            {
                ThrowIfNotCreated();

                return Native.enet_peer_get_lastsendtime(nativePeer);
            }
        }

        public uint LastReceiveTime
        {
            get
            {
                ThrowIfNotCreated();

                return Native.enet_peer_get_lastreceivetime(nativePeer);
            }
        }

        public ulong PacketsSent
        {
            get
            {
                ThrowIfNotCreated();

                return Native.enet_peer_get_packets_sent(nativePeer);
            }
        }

        public ulong PacketsLost
        {
            get
            {
                ThrowIfNotCreated();

                return Native.enet_peer_get_packets_lost(nativePeer);
            }
        }

        public float PacketsThrottle
        {
            get
            {
                ThrowIfNotCreated();

                return Native.enet_peer_get_packets_throttle(nativePeer);
            }
        }

        public ulong BytesSent
        {
            get
            {
                ThrowIfNotCreated();

                return Native.enet_peer_get_bytes_sent(nativePeer);
            }
        }

        public ulong BytesReceived
        {
            get
            {
                ThrowIfNotCreated();

                return Native.enet_peer_get_bytes_received(nativePeer);
            }
        }

        public IntPtr Data
        {
            get
            {
                ThrowIfNotCreated();

                return Native.enet_peer_get_data(nativePeer);
            }

            set
            {
                ThrowIfNotCreated();

                Native.enet_peer_set_data(nativePeer, value);
            }
        }

        internal void ThrowIfNotCreated()
        {
            if (nativePeer == IntPtr.Zero)
                throw new InvalidOperationException("Peer not created");
        }

        public void ConfigureThrottle(uint interval, uint acceleration, uint deceleration, uint threshold)
        {
            ThrowIfNotCreated();

            Native.enet_peer_throttle_configure(nativePeer, interval, acceleration, deceleration, threshold);
        }

        public bool Send(byte channelID, ref Packet packet)
        {
            ThrowIfNotCreated();

            packet.ThrowIfNotCreated();

            return Native.enet_peer_send(nativePeer, channelID, packet.NativeData) == 0;
        }

        public bool Receive(out byte channelID, out Packet packet)
        {
            ThrowIfNotCreated();

            IntPtr nativePacket = Native.enet_peer_receive(nativePeer, out channelID);

            if (nativePacket != IntPtr.Zero)
            {
                packet = new Packet(nativePacket);

                return true;
            }

            packet = default(Packet);

            return false;
        }

        public void Ping()
        {
            ThrowIfNotCreated();

            Native.enet_peer_ping(nativePeer);
        }

        public void PingInterval(uint interval)
        {
            ThrowIfNotCreated();

            Native.enet_peer_ping_interval(nativePeer, interval);
        }

        public void Timeout(uint timeoutLimit, uint timeoutMinimum, uint timeoutMaximum)
        {
            ThrowIfNotCreated();

            Native.enet_peer_timeout(nativePeer, timeoutLimit, timeoutMinimum, timeoutMaximum);
        }

        public void Disconnect(uint data)
        {
            ThrowIfNotCreated();

            Native.enet_peer_disconnect(nativePeer, data);
        }

        public void DisconnectNow(uint data)
        {
            ThrowIfNotCreated();

            Native.enet_peer_disconnect_now(nativePeer, data);
        }

        public void DisconnectLater(uint data)
        {
            ThrowIfNotCreated();

            Native.enet_peer_disconnect_later(nativePeer, data);
        }

        public void Reset()
        {
            ThrowIfNotCreated();

            Native.enet_peer_reset(nativePeer);
        }

        public bool Equals(Peer other)
        {
            return ID == other.ID;
        }
    }
}