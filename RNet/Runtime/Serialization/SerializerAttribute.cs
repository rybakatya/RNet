namespace RapidNetworkLibrary.Serialization
{
    public class SerializerAttribute : System.Attribute
    {
        public ushort messageID;


        public SerializerAttribute(ushort id)
        {
            messageID = id;
        }
    }
}



