namespace ENet
{
    public struct Event
    {
        private ENetEvent nativeEvent;

        internal ENetEvent NativeData
        {
            get
            {
                return nativeEvent;
            }

            set
            {
                nativeEvent = value;
            }
        }

        public Event(ENetEvent @event)
        {
            nativeEvent = @event;
        }

        public EventType Type
        {
            get
            {
                return nativeEvent.type;
            }
        }

        public Peer Peer
        {
            get
            {
                return new Peer(nativeEvent.peer);
            }
        }

        public byte ChannelID
        {
            get
            {
                return nativeEvent.channelID;
            }
        }

        public uint Data
        {
            get
            {
                return nativeEvent.data;
            }
        }

        public Packet Packet
        {
            get
            {
                return new Packet(nativeEvent.packet);
            }
        }
    }
}