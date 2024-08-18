using System;
namespace RapidNet.Threading
{
    /// <summary>
    /// Structure used to wrap thread messages into unmanaged blittable data.
    /// </summary>
    public struct WorkerThreadEventHeader
    {
        /// <summary>
        /// the  thread event ID
        /// </summary>
        public ushort eventID;

        /// <summary>
        /// a pointer to unmanaged data to pass to the other thread.
        /// </summary>
        public IntPtr data;
    }
}