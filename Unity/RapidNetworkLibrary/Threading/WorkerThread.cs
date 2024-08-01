using System.Threading;


namespace RapidNetworkLibrary.Threading
{
    internal abstract class WorkerThread : Worker
    {

        protected Thread thread;


        public int GetThreadID()
        {
            return thread.ManagedThreadId;
        }

        public WorkerThread()
        {
            thread = new Thread(DoWork);
            thread.IsBackground = true;
            shouldRun = true;

            thread.Start();
        }

        private void DoWork()
        {
            Init();
            while (shouldRun == true)
            {
                Consume();
                Tick();
                Thread.Sleep(8);
            }
        }
        protected abstract void Init();
        protected abstract void Tick();

        internal override void OnDestroy()
        {
            shouldRun = false;
            thread.Join();
        }
    }
}