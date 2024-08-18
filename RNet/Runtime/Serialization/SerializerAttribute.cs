namespace RapidNet.Serialization
{

    /// <summary>
    /// Attribute used on serializers to link them with specific message IDS.
    /// </summary>
    public class SerializerAttribute : System.Attribute
    {
        /// <summary>
        ///
        /// </summary>
        public ushort messageID;

        /// <summary>
        ///
        /// </summary>

        public SerializerAttribute(ushort id)
        {
            messageID = id;
        }
    }
}



