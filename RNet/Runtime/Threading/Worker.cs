using LocklessQueue.Queues;
using RapidNetworkLibrary.Logging;
using RapidNetworkLibrary.Memory;
using RapidNetworkLibrary.Runtime.Threading.ThreadMessages;
using System;


namespace RapidNetworkLibrary.Threading
{
    /// <summary>
    /// Base class for workers, are just classes you can enque events into.
    /// </summary>
    public abstract class Worker
    {
        internal bool shouldRun;
        internal MPSCQueue<IntPtr> messageQueue = new MPSCQueue<IntPtr>(1024);
        internal abstract void OnConsume(ushort message, IntPtr data);

        /// <summary>
        /// 
        /// </summary>
        public abstract void OnDestroy();

        
        /// <summary>
        /// Enqueues a event  into the queue. This is thread safe and can be called from any thread always.
        /// </summary>
        /// <param name="threadMessageID">the event id for the event you're passing.</param>
        public void Enqueue(ushort threadMessageID)
        {
            var header = new WorkerThreadHeader()
            {
                messageID = threadMessageID,
                data = IntPtr.Zero
            };

            var messagePointer = MemoryHelper.Write(header);
            if (messageQueue.TryEnqueue(messagePointer) == false)
                Logger.Log(LogLevel.Error, "Failed to enqueue " + threadMessageID.ToString() + " to the mpsc queue");
        }

        /// <summary>
        /// Enqueues a event  into the queue. This is thread safe and can be called from any thread always.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="threadMessageID">the event id for the event you're passing.</param>
        /// <param name="data">the event data you're enqueue must be unmanaged</param>
        public void Enqueue<T>(ushort threadMessageID, T data) where T : unmanaged
        {
            var dataPointer = MemoryHelper.Write<T>(data);
            var header = new WorkerThreadHeader()
            {
                messageID = threadMessageID,
                data = dataPointer
            };

            var messagePointer = MemoryHelper.Write(header);
            if (messageQueue.TryEnqueue(messagePointer) == false)
                Logger.Log(LogLevel.Error, "Failed to enqueue " + threadMessageID.ToString() + " to the mpsc queue");
        }
        
        /// <summary>
        /// 
        /// </summary>
        public unsafe void Consume()
        {
            var ptr = IntPtr.Zero;
            while (messageQueue.TryDequeue(out ptr) == true && shouldRun == true)
            {
                var header = MemoryHelper.Read<WorkerThreadHeader>(ptr);
                OnConsume(header.messageID, header.data);
                MemoryHelper.Free(header.data);
                MemoryHelper.Free(ptr);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Flush()
        {
            
            
            var ptr = IntPtr.Zero;
            while (messageQueue.TryDequeue(out ptr) == true)
            {
                var header = MemoryHelper.Read<WorkerThreadHeader>(ptr);
                if(header.messageID == (ushort)WorkerThreadMessageID.SendSerializeMessage)
                {
                    var data = MemoryHelper.Read<SerializeNetworkMessageThreadMessage>(header.data);
                    MemoryHelper.Free(data.messageObjectPointer);
                }
                MemoryHelper.Free(header.data);
                MemoryHelper.Free(ptr);
            }
        }
    }
}