using RapidNetworkLibrary.Connections;
using System;
using System.Collections;


namespace RapidNetworkLibrary.Extensions
{


    
    /// <summary>
    /// Base class used to implement extensions for RNet.
    /// </summary>
    public abstract class RNetExtension 
    {
        private WorkerCollection _workers;

        /// <summary>
        ///
        /// </summary>
        /// <param name="workers"></param>
        public RNetExtension(WorkerCollection workers)
        {
            _workers = workers;
        }

        /// <summary>
        /// called by RNet automatically after a client has connected.
        /// </summary>
        /// <param name="threadType">The thread that this method was called on.</param>
        /// <param name="connection">The connection that has connected.</param>
        public abstract void OnSocketConnect(ThreadType threadType, Connection connection);


        /// <summary>
        /// called by RNet automatically after a client has disconnected.
        /// </summary>
        /// <param name="threadType">The thread that this method was called on.</param>
        /// <param name="connection">The connection that has disconnected.</param>
        public abstract void OnSocketDisconnect(ThreadType threadType, Connection connection);


        /// <summary>
        /// called by RNet automatically after a client has timedout.
        /// </summary>
        /// <param name="threadType">The thread that this method was called on.</param>
        /// <param name="connection">The connection that has timedout.</param>
        public abstract void OnSocketTimeout(ThreadType threadType, Connection connection);


        /// <summary>
        /// Called by RNet automatically after a message  has been received.
        /// </summary>
        /// <param name="threadType">the thread that this method is being called from</param>
        /// <param name="sender"></param>
        /// <param name="messageID"></param>
        /// <param name="messageData"></param>
        /// <returns>returns true if you want to prevent this event being called in subsequent threads.</returns>
        public abstract bool OnSocketReceive(ThreadType threadType, Connection sender, ushort messageID, IntPtr messageData);

        /// <summary>
        /// Called by RNet automatically after a thread event was received. 
        /// </summary>
        /// <param name="threadType"></param>
        /// <param name="id"></param>
        /// <param name="eventData"></param>
        public abstract void OnThreadEventReceived(ThreadType threadType, ushort id, IntPtr eventData);
    }
}
