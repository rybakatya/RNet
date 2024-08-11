using LocklessQueue.Queues;
using RapidNetworkLibrary.Logging;
using RapidNetworkLibrary.Memory;
using RapidNetworkLibrary.Runtime.Threading.ThreadMessages;
using System;


namespace RapidNetworkLibrary.Threading
{
    public abstract class Worker
    {
        internal bool shouldRun;
        internal MPSCQueue<IntPtr> messageQueue = new MPSCQueue<IntPtr>(1024);
        internal abstract void OnConsume(ushort message, IntPtr data);

        public abstract void OnDestroy();

        

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