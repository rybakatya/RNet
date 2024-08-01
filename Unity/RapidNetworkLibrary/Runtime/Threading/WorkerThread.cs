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