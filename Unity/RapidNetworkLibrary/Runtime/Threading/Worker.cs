﻿using LocklessQueue.Queues;
using RapidNetworkLibrary.Logging;
using System;


namespace RapidNetworkLibrary.Threading
{
    public abstract class Worker
    {
        protected bool shouldRun;
        internal MPSCQueue<IntPtr> messageQueue = new MPSCQueue<IntPtr>(1024);
        internal abstract void OnConsume(WorkerThreadMessageID message, IntPtr data);

        internal abstract void OnDestroy();

        ~Worker()
        {
            OnDestroy();
        }

        internal void Enqueue(WorkerThreadMessageID threadMessageID)
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
        internal void Enqueue<T>(WorkerThreadMessageID threadMessageID, T data) where T : unmanaged
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
        
        internal unsafe void Consume()
        {
            var ptr = IntPtr.Zero;
            while (messageQueue.TryDequeue(out ptr) == true)
            {
                var header = MemoryHelper.Read<WorkerThreadHeader>(ptr);
                OnConsume(header.messageID, header.data);
                MemoryHelper.Free(header.data);
                MemoryHelper.Free(ptr);
            }
        }

        protected void Flush()
        {
            var ptr = IntPtr.Zero;
            while (messageQueue.TryDequeue(out ptr) == true)
            {
                var header = MemoryHelper.Read<WorkerThreadHeader>(ptr);
                MemoryHelper.Free(header.data);
                MemoryHelper.Free(ptr);
            }
        }
    }
}