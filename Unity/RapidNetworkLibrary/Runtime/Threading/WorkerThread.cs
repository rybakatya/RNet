using System.Diagnostics;
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

            
        }

        public int timeout = 0;
        private Stopwatch ticker;
        public void StartThread(int ticksPerSecond)
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
        protected abstract void Init();
        protected abstract void Tick();
        protected abstract void Destroy();
        internal override void OnDestroy()
        {
            Flush();
            Destroy();
            shouldRun = false;
            thread.Abort();
        }
    }
}