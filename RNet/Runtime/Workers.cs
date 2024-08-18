using RapidNet.Connections;

using RapidNet.Workers;
using System;


namespace RapidNet
{
    /// <summary>
    /// Holds references to the game, logic, and network  threads.
    /// </summary>
    public class WorkerCollection
    {
        /// <summary>
        /// reference to game worker thread.
        /// </summary>
        public GameWorker gameWorker;

        /// <summary>
        /// refrence to network  worker thread.
        /// </summary>
        public SocketWorkerThread socketWorker;

        /// <summary>
        /// reference to logic worker  thread.
        /// </summary>
        public LogicWorkerThread logicWorker;


        
    }
}



