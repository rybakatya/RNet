using System;


namespace RapidNet.Serialization
{
    /// <summary>
    /// Base class used by the logic thread for serialization
    /// </summary>
    public abstract class Serializer
    {
        /// <summary>
        /// the message id that this serializer is serving
        /// </summary>
        protected ushort messageID;

        /// <summary>
        /// Called on the logic thread when an outoing network message is serialized
        /// </summary>
        /// <param name="buffer">The allocated buffer to write to.</param>
        /// <param name="data">The IMessageObject that was passed to RNet.SendMessage pointer.</param>
        public abstract void Serialize(ref BitBufferData buffer, IntPtr data);

        /// <summary>
        /// Called on the logic thread when an incoming network message is deserialized.
        /// </summary>
        /// <param name="buffer">the buffer to read from</param>
        /// <returns>a pointer to a struct holding the deserialized data, must implement IMessageObject</returns>
        public abstract IntPtr Deserialize(ref BitBufferData buffer);
    }
}



