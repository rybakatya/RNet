using System;


namespace RapidNetworkLibrary.Serialization
{
    public abstract class Serializer
    {
        protected ushort messageID;

        public abstract void Serialize(BitBuffer buffer, IntPtr data);
        public abstract IntPtr Deserialize(BitBuffer buffer);
    }
}



