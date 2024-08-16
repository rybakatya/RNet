using LocklessQueue.Queues;
using RapidNetworkLibrary.Logging;
using RapidNetworkLibrary.Memory;
using RapidNetworkLibrary.Threading.ThreadEvents;
using System;


namespace RapidNetworkLibrary.Threading
{
    /// <summary>
    /// Base class for workers, are just classes you can enque events into.
    /// </summary>
    public abstract class Worker
    {
        internal bool shouldRun;
        internal MPSCQueue<IntPtr> eventQueue = new MPSCQueue<IntPtr>(1024);
        internal abstract void OnConsume(ushort eventID, IntPtr data);

        /// <summary>
        /// 
        /// </summary>
        public abstract void OnDestroy();

        
        /// <summary>
        /// Enqueues a event  into the queue. This is thread safe and can be called from any thread always.
        /// </summary>
        /// <param name="threadEventID">the event id for the event you're passing.</param>
        public void Enqueue(ushort threadEventID)
        {
            var header = new WorkerThreadEventHeader()
            {
                eventID = threadEventID,
                data = IntPtr.Zero
            };

            var eventPointer = MemoryHelper.Write(header);
            if (eventQueue.TryEnqueue(eventPointer) == false)
                Logger.Log(LogLevel.Error, "Failed to enqueue " + threadEventID.ToString() + " to the mpsc queue");
        }

        /// <summary>
        /// Enqueues a event  into the queue. This is thread safe and can be called from any thread always.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="threadEventID">the event id for the event you're passing.</param>
        /// <param name="data">the event data you're enqueue must be unmanaged</param>
        public void Enqueue<T>(ushort threadEventID, T data) where T : unmanaged
        {
            var dataPointer = MemoryHelper.Write<T>(data);
            var header = new WorkerThreadEventHeader()
            {
                eventID = threadEventID,
                data = dataPointer
            };

            var eventPointer = MemoryHelper.Write(header);
            if (eventQueue.TryEnqueue(eventPointer) == false)
                Logger.Log(LogLevel.Error, "Failed to enqueue " + threadEventID.ToString() + " to the mpsc queue");
        }
        
        /// <summary>
        /// 
        /// </summary>
        public unsafe void Consume()
        {
            var ptr = IntPtr.Zero;
            while (eventQueue.TryDequeue(out ptr) == true && shouldRun == true)
            {
                var header = MemoryHelper.Read<WorkerThreadEventHeader>(ptr);
                OnConsume(header.eventID, header.data);
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
            while (eventQueue.TryDequeue(out ptr) == true)
            {
                var header = MemoryHelper.Read<WorkerThreadEventHeader>(ptr);
                if(header.eventID == (ushort)WorkerThreadEventID.SendSerializeMessageEvent)
                {
                    var data = MemoryHelper.Read<SerializeNetworkMessageThreadEvent>(header.data);
                    MemoryHelper.Free(data.messageObjectPointer);
                }
                MemoryHelper.Free(header.data);
                MemoryHelper.Free(ptr);
            }
        }
    }
}