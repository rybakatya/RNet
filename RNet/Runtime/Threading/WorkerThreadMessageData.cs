using System;
namespace RapidNetworkLibrary.Threading
{
    /// <summary>
    /// Structure used to wrap thread messages into unmanaged blittable data.
    /// </summary>
    public struct WorkerThreadHeader
    {
        /// <summary>
        /// the  thread message ID
        /// </summary>
        public ushort messageID;

        /// <summary>
        /// a pointer to unmanaged data to pass to the other thread.
        /// </summary>
        public IntPtr data;
    }
}