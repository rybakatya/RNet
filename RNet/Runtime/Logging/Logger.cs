using System;


namespace RapidNetworkLibrary.Logging
{
    /// <summary>
    /// Used to log strings of text to the console window.
    /// </summary>
    public class Logger
    {

        /// <summary>
        /// Logs a string of text to the console.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="msg"></param>
       
        public static void Log(LogLevel level, string msg)
        {
            msg = "[LOGGER]: " + msg;
            switch (level)
            {
                case LogLevel.Info:
#if ENABLE_MONO || ENABLE_IL2CPP
                    UnityEngine.Debug.Log(msg);
#else
                    Console.WriteLine(msg);
#endif
                    break;

                case LogLevel.Warning:
#if ENABLE_MONO || ENABLE_IL2CPP
                    UnityEngine.Debug.LogWarning(msg);
#else
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.White;
#endif
                    break;

                case LogLevel.Error:
#if ENABLE_MONO || ENABLE_IL2CPP
                    UnityEngine.Debug.LogError(msg);
#else
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.White;
#endif
                    break;

                case LogLevel.Exception:
                    throw new Exception(msg);

            }
        }
    }
}



