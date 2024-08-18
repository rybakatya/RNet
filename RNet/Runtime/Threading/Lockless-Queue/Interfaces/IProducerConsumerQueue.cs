namespace RapidNet.Threading.LocklessQueue
{
    /// <summary>
    /// A common interface used for concurrent queues.
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the queue.</typeparam>
    internal interface IProducerConsumerQueue<T> : IProducerQueue<T>, IConsumerQueue<T>
    {
    }
}
