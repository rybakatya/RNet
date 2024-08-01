using RapidNetworkLibrary.Serialization;
using System;


namespace RapidNetworkLibrary.Connections
{
    [Serializer(NetworkMessages.informConnectionType)]
    internal class InformConnectionSerializer : Serializer
    {
        public override IntPtr Deserialize(BitBuffer buffer)
        {
            var data = new InformConnectionType()
            {
                type = (ConnectionType)buffer.ReadByte()
            };
            return MemoryHelper.Write(data);
        }

        public override void Serialize(BitBuffer buffer, IntPtr ptr)
        {
            var data = MemoryHelper.Read<InformConnectionType>(ptr);
            buffer.AddByte((byte)data.type);
        }
    }
}



