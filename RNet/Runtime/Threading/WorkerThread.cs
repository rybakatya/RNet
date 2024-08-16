using System.Diagnostics;
using System.Threading;


namespace RapidNetworkLibrary.Threading
{

    /// <summary>
    /// A worker that is invoked on a seperate thread.
    /// </summary>
    public abstract class WorkerThread : Worker
    {
        /// <summary>
        /// 
        /// </summary>
        protected Thread thread;

        /// <summary>
        /// Gets the workers thread id.
        /// </summary>
        /// <returns>workers thread id.</returns>
        public int GetThreadID()
        {
            return thread.ManagedThreadId;
        }
        /// <summary>
        /// 
        /// </summary>
        public WorkerThread()
        {
            thread = new Thread(DoWork);
            thread.IsBackground = true;
            shouldRun = true;           
        }

        private int timeout = 0;
        private Stopwatch ticker;
        internal void StartThread(int ticksPerSecond)
        {
            timeout = 1000/ticksPerSecond;
            thread.Start();
        }
        private void DoWork()
        {
            
            Init();
            while (shouldRun == true)
            {
                Consume();
                Tick();
                Thread.Sleep(20);
                
            }
            

           
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract void Init();
        /// <summary>
        /// 
        /// </summary>
        protected abstract void Tick();
        /// <summary>
        /// 
        /// </summary>
        protected abstract void Destroy();
        /// <summary>
        /// 
        /// </summary>
        public override void OnDestroy()
        {
            shouldRun = false;
            Flush();
            Destroy();           
            thread.Join();
        }
    }
}